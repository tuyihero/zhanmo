using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using UnityEngine.EventSystems;
using System;
using Tables;

public class UISellShopQualityItem : UIItemSelect
{
    public Text _QualityName;

    public ITEM_QUALITY ShowQuality { get; set; }
    public Vector2 LevelVector { get; set; }
    public override void Show(Hashtable hash)
    {
        base.Show();

        if (hash["InitObj"] is ITEM_QUALITY)
        {
            ShowQuality = (ITEM_QUALITY)hash["InitObj"];
            ShowItem(ShowQuality);
        }
        else if (hash["InitObj"] is Vector2)
        {
            LevelVector = (Vector2)hash["InitObj"];
            ShowLevel(LevelVector);
        }

        
    }

    public void ShowItem(ITEM_QUALITY showQuality)
    {
        _QualityName.text = CommonDefine.GetQualityName(showQuality);
    }

    public void ShowLevel(Vector2 level)
    {
        if (level.x == level.y)
        {
            _QualityName.text = "Lv." + level.x;
        }
        else
        {
            _QualityName.text = "Lv." + level.x + " - Lv." + level.y;
        }
    }
    
}

