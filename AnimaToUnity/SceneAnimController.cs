using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneAnimController : MonoBehaviour
{

    #region 

    public class FarPanelInfo
    {

    }

    #endregion

    public float _GroundLength;

    public Dictionary<Transform, float> _FarLength = new Dictionary<Transform, float>();

    void Awake()
    {
        InitLength();
    }

    
    void Update()
    {
        
    }

    #region init

    public void InitLength()
    {
        _FarLength.Clear();
        var ground = transform.Find("Ground");
        _GroundLength = GetChildLength(ground) * transform.localScale.x;
        for (int i = 1; i < 4; ++i)
        {
            var farGround = transform.Find("Far" + i);
            if (farGround != null)
            {
                var farLength = GetChildLength(farGround) * transform.localScale.x;
                if (farLength > 0)
                {
                    _FarLength.Add(farGround, farLength);
                }
            }
        }
    }

    private float GetChildLength(Transform trans)
    {
        float length = 0;
        var spriteRenders = trans.GetComponentsInChildren<SpriteRenderer>(trans.gameObject);
        foreach (var spriteRender in spriteRenders)
        {
            length += spriteRender.sprite.rect.width * spriteRender.transform.localScale.x;
        }

        Debug.Log("GetChildLength:" + trans.name + "," + length);
        return length;
    }

    #endregion

    #region update far

    public void UpdateFarPos(float relatPosX)
    {
        foreach (var farInfo in _FarLength)
        {
            float trackLength = (_GroundLength - farInfo.Value) / transform.localScale.x;
            farInfo.Key.localPosition = new Vector3(trackLength * relatPosX*0.01f, farInfo.Key.localPosition.y, 0);
        }
    }

    #endregion

    #region scene obj pos

    private float _SceneY = 0;
    public float SceneY
    {
        get
        {
            return _SceneY;
        }
    }

    private float _SceneZMin = 0;
    public float SceneZMin
    {
        get
        {
            return _SceneZMin;
        }
    }

    private float _SceneZMax = 0;
    public float SceneZMax
    {
        get
        {
            return _SceneZMax;
        }
    }

    private float _SceneXLimitMin = 0;
    public float SceneXLimitMin
    {
        get
        {
            return _SceneXLimitMin;
        }
    }

    private float _SceneXLimitMax = 0;
    public float SceneXLimitMax
    {
        get
        {
            return _SceneXLimitMax;
        }
    }

    public void InitSceneObjPos()
    {
        var startPos = transform.position;
        startPos += transform.transform.up * -(2.7f * transform.localScale.y);
        var endPos = startPos;
        endPos += new Vector3(0, 0, -1.8f * transform.localScale.y * 1.4f);

        _SceneY = startPos.y;
        _SceneZMin = endPos.z + 0.2f;
        _SceneZMax = startPos.z - 0.2f;
    }

    #endregion
}
