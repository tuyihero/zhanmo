using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Kent.Boogaart.KBCsv;

using UnityEngine;

namespace Tables
{
    public partial class SummonSkillLotteryRecord : TableRecordBase
    {

    }

    public partial class SummonSkillLottery : TableFileBase
    {
        private List<SummonSkillLotteryRecord> _RecordsList;
        public List<SummonSkillLotteryRecord> RecordsList
        {
            get
            {
                if (_RecordsList == null)
                {
                    _RecordsList = new List<SummonSkillLotteryRecord>();
                    foreach (var record in Records)
                    {
                        _RecordsList.Add(record.Value);
                    }
                }
                return _RecordsList;
            }
        }

        private List<int> _GoldRates;
        public List<int> GoldRates
        {
            get
            {
                if (_GoldRates == null)
                {
                    _GoldRates = new List<int>();
                    for(int i = 0; i< RecordsList.Count; ++i)
                    {
                        _GoldRates.Add(RecordsList[i].Lottery[0]);
                    }
                }
                return _GoldRates;
            }
        }

        private List<int> _DiamondRates;
        public List<int> DiamondRates
        {
            get
            {
                if (_DiamondRates == null)
                {
                    _DiamondRates = new List<int>();
                    for (int i = 0; i < RecordsList.Count; ++i)
                    {
                        _DiamondRates.Add(RecordsList[i].Lottery[1]);
                    }
                }
                return _DiamondRates;
            }
        }
    }

}