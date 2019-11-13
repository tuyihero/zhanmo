using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.EventSystems;

 
using System;



public class UIPopBase : UIBase, IPointerClickHandler
{

    #region show

    public override void Show()
    {
        // if (!gameObject.activeSelf)
        {
            //Hashtable hash = new Hashtable();
            //hash.Add("UIName", gameObject.name);
            //hash.Add("UIObj", gameObject);
            //GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_UI_SHOWED, this, new Hashtable());

            gameObject.SetActive(true);
        }
    }

    public override void Hide()
    {
        // if (gameObject.activeSelf)
        {
            //Hashtable hash = new Hashtable();
            //hash.Add("UIName", gameObject.name);
            //hash.Add("UIObj", gameObject);
            //GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_UI_HIDED, this, new Hashtable());

            gameObject.SetActive(false);
        }
    }

    #endregion

    #region IPointerClickHandler

    public void OnPointerClick(PointerEventData eventData)
    {
        //do nothing;
        //throw new NotImplementedException();
    }


    #endregion

}

