using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Kent.Boogaart.KBCsv;

using UnityEngine;

namespace Tables
{
    public partial class FiveElementCoreRecord : TableRecordBase
    {

    }

    public partial class FiveElementCore : TableFileBase
    {
        public FiveElementCoreRecord GetElementCoreRecord(ITEM_QUALITY quality, FIVE_ELEMENT type)
        {
            foreach (var record in Records.Values)
            {
                if (record.Quality == quality && record.ElementType == type)
                    return record;
            }

            return null;
        }
    }

}