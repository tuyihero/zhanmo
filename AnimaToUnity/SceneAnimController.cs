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
        _GroundLength = GetChildLength(ground);
        for (int i = 1; i < 4; ++i)
        {
            var farGround = transform.Find("Far" + i);
            if (farGround != null)
            {
                var farLength = GetChildLength(farGround);
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
            float trackLength = _GroundLength - farInfo.Value;
            farInfo.Key.localPosition = new Vector3(trackLength * relatPosX*0.01f, farInfo.Key.localPosition.y, 0);
        }
    }

    #endregion
}
