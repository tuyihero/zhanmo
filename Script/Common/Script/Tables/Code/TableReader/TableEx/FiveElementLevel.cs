using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Kent.Boogaart.KBCsv;

using UnityEngine;

namespace Tables
{
    public partial class FiveElementLevelRecord : TableRecordBase
    {
        
    }

    public partial class FiveElementLevel : TableFileBase
    {

        public FiveElementLevelRecord GetElementLevel(int level)
        {
            if (level > Records.Count)
                return Records[(Records.Count - 1).ToString()];

            return Records[level.ToString()];
        }

    }
}