
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using System.Collections;
 



public class UIJoyStick : UIBase, IDragHandler, IPointerUpHandler, IPointerDownHandler
{

    #region static

    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIJoyStick, UILayer.BaseUI, hash);
    }

    #endregion

    public Image _JoyStickSprite;
    public Image _BackGround;
    public float _MaxRadius = 50.0f;

    private Vector2 _TouchPos = Vector2.zero;
    private Vector2 _LastTouchPos = Vector2.zero;
    private RectTransform _JoyStickRectTransform;
    private int _TouchFingerID = -1;

    public override void Init()
    {
        base.Init();

        _JoyStickRectTransform = _JoyStickSprite.GetComponent<RectTransform>();
        SetImageAlpah(_JoyStickSprite, 0.5f);
        SetImageAlpah(_BackGround, 0.5f);
    }


    public void OnDrag(PointerEventData data)
    {
        //if (_TouchFingerID < 0)
        //    return;

        //for (int i = 0; i < Input.touchCount; ++i)
        {
            //if (Input.GetTouch(i).fingerId == _TouchFingerID)
            {
                //_TouchPos.x = Input.GetTouch(i).position.x;
                //_TouchPos.y = Input.GetTouch(i).position.y;
                _TouchPos.x = data.position.x;
                _TouchPos.y = data.position.y;

                float nDeltaX = _TouchPos.x - _LastTouchPos.x;
                float nDeltaY = _TouchPos.y - _LastTouchPos.y;
                // 新的XY 先不设置
                float nNewX = _JoyStickRectTransform.anchoredPosition.x + nDeltaX;
                float nNewY = _JoyStickRectTransform.anchoredPosition.y + nDeltaY;
                // 计算距离
                float nDistance = Vector2.Distance(new Vector2(nNewX, nNewY), Vector2.zero);
                if (nDistance <= _MaxRadius)
                {
                    // 若拖动位置在最大半径以内 则直接设置newXY
                    _JoyStickRectTransform.anchoredPosition = new Vector2(nNewX, nNewY);
                }
                else
                {
                    // 若拖动位置超出最大半径 则设置连线和圆的交点位置
                    Vector3 nResult = new Vector3(0, 0, 0);
                    // 计算鼠标和摇杆连线的直线方程 与摇杆移动范围为半径的圆的交点坐标
                    var rate = _MaxRadius / nDistance;

                    nResult.x = nNewX * rate;
                    nResult.y = nNewY * rate;

                    _JoyStickRectTransform.anchoredPosition = nResult;
                }
                SendMoveDirection();
                // 更新鼠标位置
                _LastTouchPos = _TouchPos;
            }
        }
    }

    public void OnPointerUp(PointerEventData data)
    {
        _TouchFingerID = -1;

        // 恢复摇杆精灵透明度
        SetImageAlpah(_JoyStickSprite, 0.5f);
        SetImageAlpah(_BackGround, 0.5f);
        _JoyStickRectTransform.anchoredPosition = Vector2.zero;
        SendMoveDirection();
    }

    public void OnPointerDown(PointerEventData data)
    {
        if (_TouchFingerID > 0)
            return;

        for (int i = 0; i < Input.touchCount; ++i)
        {
            if (Input.GetTouch(i).position == data.position)
            {
                _TouchFingerID = Input.GetTouch(i).fingerId;
            }
        }


        // 记录鼠标位置
        _LastTouchPos = Input.mousePosition;
        // 拖动时重设精灵透明度
        SetImageAlpah(_JoyStickSprite, 1);
        SetImageAlpah(_BackGround, 1);

    }

    public void SetImageAlpah(Image image, float alpha)
    {
        if (image == null)
            return;
        image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
    }

    private void SendMoveDirection()
    {
        InputManager.Instance.Axis = _JoyStickRectTransform.anchoredPosition / _MaxRadius;
    }
}

