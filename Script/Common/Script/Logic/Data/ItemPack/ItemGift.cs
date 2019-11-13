using UnityEngine;
using System.Collections;
using Tables;

public class ItemGift : ItemBase
{
    public ItemGift(string dataID) : base(dataID)
    {

    }

    public ItemGift() : base()
    {

    }

    #region base attr
    
    private GiftPacketRecord _GiftRecord;
    public GiftPacketRecord GiftRecord
    {
        get
        {
            if (_GiftRecord == null)
            {
                _GiftRecord = TableReader.GiftPacket.GetRecord(ItemDataID);
            }
            return _GiftRecord;
        }
    }
    
    #endregion 



}

