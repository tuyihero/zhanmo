using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIDialItem : UIItemBase
{

    public Text _NumText;

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        int num = (int)hash["InitObj"];
        _NumText.text = num.ToString();
    }
}
