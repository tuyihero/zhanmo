using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Kent.Boogaart.KBCsv;

using UnityEngine;

namespace Tables
{
    public partial class EquipItemRecord : TableRecordBase
    {
        private CommonItemRecord _CommonItem;
        public CommonItemRecord CommonItem
        {
            get
            {
                if (_CommonItem == null)
                {
                    _CommonItem = TableReader.CommonItem.GetRecord(Id);
                }
                return _CommonItem;
            }
        }

        public string Model
        {
            get
            {
                return CommonItem.Model;
            }
        }
    }

    public partial class EquipItem : TableFileBase
    {
        private Dictionary<EQUIP_SLOT, List<EquipItemRecord>> _ClassedEquips = null;
        public Dictionary<EQUIP_SLOT, List<EquipItemRecord>> ClassedEquips
        {
            get
            {
                if (_ClassedEquips == null)
                {
                    InitClassedEquips();
                }
                return _ClassedEquips;
            }
        }

        private List<EquipItemRecord> _AxeWeapons = null;
        public List<EquipItemRecord> AxeWeapons
        {
            get
            {
                if (_AxeWeapons == null)
                {
                    InitClassedEquips();
                }
                return _AxeWeapons;
            }
        }

        private List<EquipItemRecord> _SwordWeapons = null;
        public List<EquipItemRecord> SwordWeapons
        {
            get
            {
                if (_SwordWeapons == null)
                {
                    InitClassedEquips();
                }
                return _SwordWeapons;
            }
        }

        private Dictionary<EQUIP_SLOT, List<EquipItemRecord>> _BaseAttrLegendary = null;
        public Dictionary<EQUIP_SLOT, List<EquipItemRecord>> BaseAttrLegendary
        {
            get
            {
                if (_BaseAttrLegendary == null)
                {
                    InitClassedEquips();
                }
                return _BaseAttrLegendary;
            }
        }

        private void InitClassedEquips()
        {
            _ClassedEquips = new Dictionary<EQUIP_SLOT, List<EquipItemRecord>>();
            _AxeWeapons = new List<EquipItemRecord>();
            _SwordWeapons = new List<EquipItemRecord>();
            _BaseAttrLegendary = new Dictionary<EQUIP_SLOT, List<EquipItemRecord>>();

            foreach (var record in Records.Values)
            {
                if (record.ExAttr[0] != null)
                {
                    if (record.ExAttr[0].AttrImpact.Equals("RoleAttrImpactBaseAttr"))
                    {
                        if (!_BaseAttrLegendary.ContainsKey(record.Slot))
                        {
                            _BaseAttrLegendary.Add(record.Slot, new List<EquipItemRecord>());
                        }
                        _BaseAttrLegendary[record.Slot].Add(record);
                    }
                    continue;
                }

                if (!_ClassedEquips.ContainsKey(record.Slot))
                {
                    _ClassedEquips.Add(record.Slot, new List<EquipItemRecord>());
                }
                _ClassedEquips[record.Slot].Add(record);
                if (record.Slot == EQUIP_SLOT.WEAPON && record.ProfessionLimit == 5)
                {
                    _AxeWeapons.Add(record);
                }
                else
                {
                    if (record.Slot == EQUIP_SLOT.WEAPON && record.ProfessionLimit == 10)
                    {
                        _SwordWeapons.Add(record);
                    }
                }

                    
                
            }

            
        }

    }

}