using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
 

 

public enum ShowDamageEnum
{
    Normal = 0,
    Critical,
    Hurt,
    Heal,
}

public class DamageItem : MonoBehaviour
{
    public GameObject[] _NumPrefab;
    public Transform _PanelTransform;
    public Transform _MainNumTransform;
    public Transform _SubNumTransform;
    public float offsetX;
    public float _AnimAlpha;
    public Image _ElementImage;
    public Sprite[] _ElementSprites;

    private static Color _NormalColor = Color.yellow;
    private static Color _CriticalColor = new Color(1,1,0,1);
    private static Color _HurtColor = Color.red;
    private static Color _HealColor = Color.green;
    private Color _NumColor;
    private Color _LastSetColor;

    private Vector3 _InitPos = Vector3.zero;
    public List<GameObject> _NumOBJs = new List<GameObject>();

    public void Show(Vector3 showWorldPos, int showValue1, int showValue2, RoleAttrManager.ShowDamageType showType, int baseSize)
    {
        gameObject.SetActive(true);
        transform.position = showWorldPos;
        transform.rotation = Quaternion.Euler(-Camera.main.transform.rotation.eulerAngles + new Vector3(0, 180, 0));
        _PanelTransform.position += new Vector3(0, 0, 0.001f);
        if (_PanelTransform.position.z > 1)
        {
            _PanelTransform.position = Vector3.zero;
        }

        _NumColor = _NormalColor;
        _AnimAlpha = 1;
        switch (showType)
        {
            case RoleAttrManager.ShowDamageType.Normal:
                _NumColor = _NormalColor;
                break;
            case RoleAttrManager.ShowDamageType.Criticle:
                _NumColor = _CriticalColor;
                break;
            case RoleAttrManager.ShowDamageType.Hurt:
                _NumColor = _HurtColor;
                break;
            case RoleAttrManager.ShowDamageType.Heal:
                _NumColor = _HealColor;
                break;
        }
        _NumColor.a = _AnimAlpha;

        SetNumItem(showValue1, _NumColor, _MainNumTransform);

        SetNumColor(_NumColor);
    }

    private void SetNumItem(int num, Color color, Transform parentTrans)
    {
        int copyNum = num;
        int offsetIdx = 0;
        while (copyNum > 0)
        {
            int numSingle = copyNum % 10;
            copyNum = (int)(copyNum * 0.1f);
            var numGO = GetNumSingle(numSingle, color);
            numGO.transform.SetParent(parentTrans);
            numGO.transform.localPosition = new Vector3(offsetX * offsetIdx, 0, 0);
            numGO.transform.localRotation = Quaternion.Euler(Vector3.zero);
            _NumOBJs.Add(numGO.gameObject);
            ++offsetIdx;
        }
    }

    private Transform GetNumSingle(int num, Color color)
    {
        var numPrefab = _NumPrefab[num];
        var numObj = ResourcePool.Instance.GetIdleUIItem<Transform>(numPrefab);
        numObj.gameObject.SetActive(true);
        return numObj;
    }

    private void SetNumColor(Color color)
    {
        if (color == _LastSetColor)
            return;

        _LastSetColor = color;
        for (int i = 0; i < _NumOBJs.Count; ++i)
        {
            var render = _NumOBJs[i].GetComponent<Renderer>();
            render.material.SetColor("_Color", color);
        }
    }

    public void Hide()
    {
        foreach (var numItem in _NumOBJs)
        {
            ResourcePool.Instance.RecvIldeUIItem(numItem);
        }
        _NumOBJs.Clear();
        DamagePanel.HideItem(this);
    }

    public void Update()
    {
        _NumColor.a = _AnimAlpha;
        SetNumColor(_NumColor);
    }
}

