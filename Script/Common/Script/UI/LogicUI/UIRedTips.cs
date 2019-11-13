using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRedTips : MonoBehaviour {

    public enum TipType
    {
        Equip,
        Gem,
        Soul,
        EquipCollect,
        Skill,
    }

    public TipType _TipType;
    public GameObject _TipGO;
	
	void OnEnable ()
    {
        if (_TipType == TipType.Equip)
        {
            GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_EQUIP_GET, EventHandle);
            GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_EQUIP_PUT_ON, EventHandle);
            GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_EQUIP_SELL, EventHandle);
            GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_EQUIP_REFRESH, EventHandle);
            GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_EQUIP_STORE, EventHandle);
            GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_EQUIP_DESTORY, EventHandle);
        }
        else if (_TipType == TipType.Gem)
        {
            GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_EQUIP_GEM_GET, EventHandle);
            GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_EQUIP_GEM_PUT_ON, EventHandle);
            GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_EQUIP_GEM_PUT_OFF, EventHandle);
            GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_EQUIP_GEM_COMBINE, EventHandle);
        }
        else if (_TipType == TipType.Soul)
        {
            GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_SOUL_REFRESH, EventHandle);
        }
        else if (_TipType == TipType.EquipCollect)
        {
            GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_EQUIP_GET, EventHandle);
            GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_EQUIP_PUT_ON, EventHandle);
            GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_EQUIP_SELL, EventHandle);
            GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_EQUIP_REFRESH, EventHandle);
            GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_EQUIP_STORE, EventHandle);
            GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_EQUIP_DESTORY, EventHandle);
        }
        else if (_TipType == TipType.Skill)
        {
            GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_LEVELUP_SKILL, EventHandle);
        }

        RefreshTip();

    }

    private void OnDisable()
    {
        if (_TipType == TipType.Equip)
        {
            GameCore.Instance.EventController.UnRegisteEvent(EVENT_TYPE.EVENT_LOGIC_EQUIP_GET, EventHandle);
            GameCore.Instance.EventController.UnRegisteEvent(EVENT_TYPE.EVENT_LOGIC_EQUIP_PUT_ON, EventHandle);
            GameCore.Instance.EventController.UnRegisteEvent(EVENT_TYPE.EVENT_LOGIC_EQUIP_SELL, EventHandle);
            GameCore.Instance.EventController.UnRegisteEvent(EVENT_TYPE.EVENT_LOGIC_EQUIP_REFRESH, EventHandle);
            GameCore.Instance.EventController.UnRegisteEvent(EVENT_TYPE.EVENT_LOGIC_EQUIP_STORE, EventHandle);
            GameCore.Instance.EventController.UnRegisteEvent(EVENT_TYPE.EVENT_LOGIC_EQUIP_DESTORY, EventHandle);
        }
        else if (_TipType == TipType.Gem)
        {
            GameCore.Instance.EventController.UnRegisteEvent(EVENT_TYPE.EVENT_LOGIC_EQUIP_GEM_GET, EventHandle);
            GameCore.Instance.EventController.UnRegisteEvent(EVENT_TYPE.EVENT_LOGIC_EQUIP_GEM_PUT_ON, EventHandle);
            GameCore.Instance.EventController.UnRegisteEvent(EVENT_TYPE.EVENT_LOGIC_EQUIP_GEM_PUT_OFF, EventHandle);
            GameCore.Instance.EventController.UnRegisteEvent(EVENT_TYPE.EVENT_LOGIC_EQUIP_GEM_COMBINE, EventHandle);
        }
        else if (_TipType == TipType.Soul)
        {
            GameCore.Instance.EventController.UnRegisteEvent(EVENT_TYPE.EVENT_LOGIC_SOUL_REFRESH, EventHandle);
        }
        else if (_TipType == TipType.EquipCollect)
        {
            GameCore.Instance.EventController.UnRegisteEvent(EVENT_TYPE.EVENT_LOGIC_EQUIP_GET, EventHandle);
            GameCore.Instance.EventController.UnRegisteEvent(EVENT_TYPE.EVENT_LOGIC_EQUIP_PUT_ON, EventHandle);
            GameCore.Instance.EventController.UnRegisteEvent(EVENT_TYPE.EVENT_LOGIC_EQUIP_SELL, EventHandle);
            GameCore.Instance.EventController.UnRegisteEvent(EVENT_TYPE.EVENT_LOGIC_EQUIP_REFRESH, EventHandle);
            GameCore.Instance.EventController.UnRegisteEvent(EVENT_TYPE.EVENT_LOGIC_EQUIP_STORE, EventHandle);
            GameCore.Instance.EventController.UnRegisteEvent(EVENT_TYPE.EVENT_LOGIC_EQUIP_DESTORY, EventHandle);
        }
        else if (_TipType == TipType.Skill)
        {
            GameCore.Instance.EventController.UnRegisteEvent(EVENT_TYPE.EVENT_LOGIC_LEVELUP_SKILL, EventHandle);
        }

    }

    void EventHandle(object go, Hashtable eventArgs)
    {
        RefreshTip();
    }

    void RefreshTip()
    {
        if (_TipType == TipType.Equip)
        {
            if (BackBagPack.Instance.IsAnyEquipBetter())
            {
                _TipGO.SetActive(true);
            }
            else
            {
                _TipGO.SetActive(false);
            }
        }
        else if (_TipType == TipType.Gem)
        {
            if (GemData.Instance.IsAnyGemGanEquip() || GemData.Instance.IsAnyGemGanLvUp())
            {
                _TipGO.SetActive(true);
            }
            else
            {
                _TipGO.SetActive(false);
            }
        }
        else if (_TipType == TipType.Soul)
        {
            if (SummonSkillData.Instance.IsCanAbsorb())
            {
                _TipGO.SetActive(true);
            }
            else
            {
                _TipGO.SetActive(false);
            }
        }
        else if (_TipType == TipType.EquipCollect)
        {
            if (BackBagPack.Instance.IsAnyEquipCollectBetter())
            {
                _TipGO.SetActive(true);
            }
            else
            {
                _TipGO.SetActive(false);
            }
        }
        else if (_TipType == TipType.Skill)
        {
            if (SkillData.Instance.IsCanAnySkillLvUp())
            {
                _TipGO.SetActive(true);
            }
            else
            {
                _TipGO.SetActive(false);
            }
        }

    }
}
