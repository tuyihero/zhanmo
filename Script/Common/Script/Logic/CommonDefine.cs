using UnityEngine;
using System.Collections;

using Tables;


public class CommonDefine
{
    public static string GetQualityColorStr(ITEM_QUALITY quality)
    {
        switch (quality)
        {
            case ITEM_QUALITY.WHITE:
                return "<color=#A5A5A5FF>";
            case ITEM_QUALITY.GREEN:
                return "<color=#10D200FF>";
            case ITEM_QUALITY.BLUE:
                return "<color=#3ba0ff>";
            case ITEM_QUALITY.PURPER:
                return "<color=#ca40e7>";
            case ITEM_QUALITY.ORIGIN:
                return "<color=#F0960EFF>";
        }
        return "<color=#ffffff>";
    }

    public static string GetQualityName(ITEM_QUALITY quality)
    {
        string name = GetQualityColorStr(quality);
        name += StrDictionary.GetFormatStr(5000 + (int)quality);
        name += "</color>";
        return name;
    }

    public static string GetMigicAttrColor()
    {
        return "<color=#3ba0ff>";
    }

    public static string GetEnableRedStr(int isEnable)
    {
        switch (isEnable)
        {
            case 1:
                return "<color=#00ff00>";
            case 0:
                return "<color=#ff0000>";
        }
        return "<color=#ffffff>";
    }

    public static string GetEnableGrayStr(int isEnable)
    {
        switch (isEnable)
        {
            case 1:
                return "<color=#A8CACAFF>";
            case 0:
                return "<color=#777777>";
        }
        return "<color=#ffffff>";
    }

    public static string GetQualityItemName(string itemDataID, bool withBrackets = false)
    {
        var itemRecord = TableReader.CommonItem.GetRecord(itemDataID);
        string itemName = StrDictionary.GetFormatStr(itemRecord.NameStrDict);
        if (withBrackets)
        {
            itemName = "[" + itemName + "]";
        }
        itemName = GetQualityColorStr(itemRecord.Quality) + itemName + "</color>";
        return itemName;
    }

    public static string GetQualityIcon(ITEM_QUALITY quality)
    {
        switch (quality)
        {
            case ITEM_QUALITY.WHITE:
                return "Quality/quality_1";
            case ITEM_QUALITY.GREEN:
                return "Quality/quality_2";
            case ITEM_QUALITY.BLUE:
                return "Quality/quality_3";
            case ITEM_QUALITY.PURPER:
                return "Quality/quality_4";
            case ITEM_QUALITY.ORIGIN:
                return "Quality/quality_5";
            default:
                return "Quality/quality_1";
        }
    }

    public static string GetQualityFramIcon(ITEM_QUALITY quality)
    {
        switch (quality)
        {
            case ITEM_QUALITY.WHITE:
                return "Quality/1";
            case ITEM_QUALITY.GREEN:
                return "Quality/2";
            case ITEM_QUALITY.BLUE:
                return "Quality/3";
            case ITEM_QUALITY.PURPER:
                return "Quality/4";
            case ITEM_QUALITY.ORIGIN:
                return "Quality/5";
            default:
                return "Quality/1";
        }
    }

    public static string GetMoneyIcon(MONEYTYPE moneyType)
    {
        switch (moneyType)
        {
            case MONEYTYPE.GOLD:
                return "money/money_gold";
            case MONEYTYPE.DIAMOND:
                return "money/money_diamond";
            default:
                return "money/money_gold";
        }
    }

    /// <summary>
    /// color 转换hex
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    public static string ColorToHex(Color color)
    {
        int r = Mathf.RoundToInt(color.r * 255.0f);
        int g = Mathf.RoundToInt(color.g * 255.0f);
        int b = Mathf.RoundToInt(color.b * 255.0f);
        int a = Mathf.RoundToInt(color.a * 255.0f);
        string hex = string.Format("{0:X2}{1:X2}{2:X2}{3:X2}", r, g, b, a);
        return hex;
    }

    /// <summary>
    /// hex转换到color
    /// </summary>
    /// <param name="hex"></param>
    /// <returns></returns>
    public static Color HexToColor(string hex)
    {
        byte br = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte bg = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte bb = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        byte cc = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
        float r = br / 255f;
        float g = bg / 255f;
        float b = bb / 255f;
        float a = cc / 255f;
        return new Color(r, g, b, a);
    }
}

