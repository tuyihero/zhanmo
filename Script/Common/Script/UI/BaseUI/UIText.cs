using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIText : MonoBehaviour
{
    public int _StrDict;

    private Text _Text;

	// Use this for initialization
	void Awake ()
    {
        _Text = gameObject.GetComponent<Text>();
        if (_Text == null)
            return;

        if (_StrDict > 0)
        {
            _Text.text = Tables.StrDictionary.GetFormatStr(_StrDict);
        }
    }
	
}
