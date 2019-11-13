using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class UIStrDict : MonoBehaviour
{
    
    public Text _Text;
    public int _StrDictID;

    public void Start()
    {
        _Text = gameObject.GetComponent<Text>();
        _Text.text = Tables.StrDictionary.GetFormatStr(_StrDictID);
    }
}
