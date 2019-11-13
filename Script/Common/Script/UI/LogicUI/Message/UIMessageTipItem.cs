using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMessageTipItem : UIItemBase
{

    #region 

    public Text ShowText;
    public Animator _Animator;

    public void SetMessage(string tip)
    {
        ShowText.text = tip;
        StartCoroutine(HideItem());
        _Animator.Play("MessageTip");
    }

    public IEnumerator HideItem()
    {
        yield return new WaitForSeconds(2.0f);
        ResourcePool.Instance.RecvIldeUIItem(gameObject);
    }

    public void RecvImmediate()
    {
        StopAllCoroutines();
        ResourcePool.Instance.RecvIldeUIItem(gameObject);
    }

    #endregion
}
