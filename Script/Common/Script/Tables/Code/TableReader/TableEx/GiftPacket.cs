using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Kent.Boogaart.KBCsv;

using UnityEngine;

namespace Tables
{
    public partial class GiftPacketRecord : TableRecordBase
    {

    }

    public partial class GiftPacket : TableFileBase
    {
        private Dictionary<int, List<GiftPacketRecord>> _GiftPacketGroup;
        public Dictionary<int, List<GiftPacketRecord>> GiftPacketGroup
        {
            get
            {
                if (_GiftPacketGroup == null)
                {
                    InitGiftGroup();
                }
                return _GiftPacketGroup;
            }
        }

        private List<int> _GroupIdx;
        public List<int> GroupIdx
        {
            get
            {
                if (_GroupIdx == null)
                {
                    InitGiftGroup();
                }
                return _GroupIdx;
            }
        }

        private void InitGiftGroup()
        {
            if (_GiftPacketGroup == null)
            {
                _GiftPacketGroup = new Dictionary<int, List<GiftPacketRecord>>();
                _GroupIdx = new List<int>();
                foreach (var giftPackt in Records.Values)
                {
                    if (!_GiftPacketGroup.ContainsKey(giftPackt.GroupID))
                    {
                        _GiftPacketGroup.Add(giftPackt.GroupID, new List<GiftPacketRecord>());
                        _GroupIdx.Add(giftPackt.GroupID);
                    }
                    _GiftPacketGroup[giftPackt.GroupID].Add(giftPackt);
                }
            }
        }
        

    }

}