using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Kent.Boogaart.KBCsv;

using UnityEngine;

namespace Tables
{
    public partial class AttrValueRecord : TableRecordBase
    {
        public EquipExAttr GetExAttr(int arg)
        {
            return TableReader.AttrValue.GetExAttr(this, arg);
        }
    }

    public partial class AttrValue : TableFileBase
    {

        public EquipExAttr GetExAttr(string id, int arg)
        {
            var record = TableReader.AttrValue.GetRecord(id);
            if (record == null)
                return null;

            return GetExAttr(record, arg);
        }

        public EquipExAttr GetExAttr(AttrValueRecord record, int arg)
        {
            if (record.AttrImpact == "RoleAttrImpactBaseAttr")
            {
                return RoleAttrImpactBaseAttr.GetExAttrByValue(record, arg);
            }
            if (record.AttrImpact == "RoleAttrImpactSetAttrByEquip")
            {
                return RoleAttrImpactSetAttrByEquip.GetSetAttr(record, arg);
            }
            else
            {
                EquipExAttr exAttr = new EquipExAttr(record.AttrImpact, 0, int.Parse(record.Id), arg);
                return exAttr;
            }
        }

    }

}