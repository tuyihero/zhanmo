using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class  UIImgAnimText: UIImgText
{
    #region 

    public UIImgFont[] _AnimFonts;
    public float _AnimInterval;

    private int _FontIdx = 0;

    public void PlayAnim()
    {
        _FontIdx = 0;
        StartCoroutine(AnimEnumerator());
    }

    private IEnumerator AnimEnumerator()
    {
        yield return new WaitForSeconds(_AnimInterval);
        _AnimFonts[_FontIdx].InitChars();
        for (int i = 0; i < text.Length; ++i)
        {
            var charImg = _AnimFonts[_FontIdx]._DictImgChars[text[i]];
            _CharImages[i].sprite = charImg._Image;
        }

        ++_FontIdx;

        if (_FontIdx >= _AnimFonts.Length)
            yield break;
    }

    #endregion

}
