using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Kent.Boogaart.KBCsv;

using UnityEngine;

namespace Tables
{
    public partial class AttrValueLevelRecord : TableRecordBase
    {

    }

    public partial class AttrValueLevel : TableFileBase
    {

        public int GetBaseValue(int level)
        {
            int tempLv = Mathf.Clamp(level, 1, Records.Count);
            var record = GetRecord(tempLv.ToString());
            if (record == null)
            {
                return 0;
            }

            if (level <= tempLv)
            {

                if (record != null)
                {
                    return record.Values[0];
                }
            }
            else
            {
                int paw = (level - tempLv) / 20;
                int subRate = (level - tempLv) % 20;
                int rate = 1;
                if (paw > 0)
                {
                    rate = (int)Mathf.Pow(2, paw);
                }
                int tempValue = (int)(record.Values[0] * rate * (1 + (subRate * 0.05f)));

                Debug.Log("GetBaseValue:" + level + "," + tempValue);

                return tempValue;

            }
            return 0;
        }

        public int GetSpValue(int level, int idx)
        {
            int tempLv = Mathf.Clamp(level, 1, Records.Count);
            var record = GetRecord(tempLv.ToString());
            if (record != null)
            {
                return record.Values[idx];
            }
            return 0;
        }

    }

}