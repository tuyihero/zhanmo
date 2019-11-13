using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Kent.Boogaart.KBCsv;

using UnityEngine;

namespace Tables
{
    public partial class EquipBaseAttrRecord : TableRecordBase
    {
        
    }

    public partial class EquipBaseAttr : TableFileBase
    {

        public EquipBaseAttrRecord GetEquipBaseAttr(int equipLv)
        {
            if (equipLv > Records.Count)
                return Records[Records.Count.ToString()];

            return Records[equipLv.ToString()];
        }

    }
}