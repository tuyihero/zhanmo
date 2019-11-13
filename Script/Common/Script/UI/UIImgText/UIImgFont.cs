using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIImgFont : MonoBehaviour
{
    public Dictionary<char, UIImgChar> _DictImgChars;

    public void InitChars()
    {
        if (_DictImgChars != null)
            return;

        _DictImgChars = new Dictionary<char, UIImgChar>();
        var childChars = GetComponentsInChildren<UIImgChar>();
        foreach (var childChar in childChars)
        {
            _DictImgChars.Add(childChar._Char, childChar);
        }
    }
}
