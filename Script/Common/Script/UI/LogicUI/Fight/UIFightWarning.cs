using UnityEngine;
using System.Collections;
using System.Collections.Generic;

 
 
using System;



public class UIFightWarning : UIBase
{

    #region static funs

    public static void ShowFightAsyn()
    {
        Hashtable hash = new Hashtable();
        hash.Add("ShowFight", true);
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIFightWarning, UILayer.BaseUI, hash);
    }

    public static void ShowBossAsyn()
    {
        Hashtable hash = new Hashtable();
        hash.Add("ShowBoss", true);
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIFightWarning, UILayer.BaseUI, hash);
    }

    public static void ShowDirectAsyn(Transform directFrom, Transform directTo)
    {
        Hashtable hash = new Hashtable();
        hash.Add("ShowDirectFrom", directFrom);
        hash.Add("ShowDirectTo", directTo);
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIFightWarning, UILayer.BaseUI, hash);
    }

    #endregion

    #region 

    void Update()
    {
        ShowDirectUpdate();
    }

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        if (hash.ContainsKey("ShowFight"))
        {
            ShowFight();
        }
        else if (hash.ContainsKey("ShowDirectFrom"))
        {
            Transform directFrom = (Transform)hash["ShowDirectFrom"];
            Transform directTo = (Transform)hash["ShowDirectTo"];
            ShowDirect(directFrom, directTo);
        }
        else if (hash.ContainsKey("ShowBoss"))
        {
            ShowBoss();
        }
    }



    #endregion

    #region 

    private const float _SHOW_FIGHT_LABEL_TIME = 2.0f;

    public GameObject _FightLabel;

    private void ShowFight()
    {
        StartCoroutine(ShowFightLabel());
    }

    private IEnumerator ShowFightLabel()
    {
        _FightLabel.gameObject.SetActive(true);
        yield return new WaitForSeconds(_SHOW_FIGHT_LABEL_TIME);
        _FightLabel.gameObject.SetActive(false);
    }
    #endregion

    #region 

    private const float _SHOW_BOSS_LABEL_TIME = 2.0f;

    public GameObject _BossLabel;

    private void ShowBoss()
    {
        StartCoroutine(ShowBossLabel());
    }

    private IEnumerator ShowBossLabel()
    {
        _BossLabel.gameObject.SetActive(true);
        yield return new WaitForSeconds(_SHOW_BOSS_LABEL_TIME);
        _BossLabel.gameObject.SetActive(false);
    }
    #endregion

    #region 

    public GameObject _GOLabel;
    public GameObject _DirectGO;

    public Transform _DirectFrom;
    public Transform _DirectTo;

    public Animation _Animation;

    public static Vector3 _Axis = Vector3.zero;

    private void ShowDirectUpdate()
    {
        if (_DirectFrom == null || _DirectTo == null)
        {
            //_GOLabel.SetActive(false);
            _DirectGO.SetActive(false);
            return;
        }

        if (_Axis == Vector3.zero)
        {
            _Axis = new Vector3(Screen.width * 0.5f, Screen.height, 0);
        }

        //if (Vector3.Distance(_DirectFrom.position, _DirectTo.position) < 6)
        //{
        //    _GOLabel.SetActive(false);
        //    _DirectGO.SetActive(false);
        //    return;
        //}

        _DirectGO.SetActive(true);
        var forward = Camera.main.transform.forward.normalized;
        forward.y = 0;
        var direct = _DirectTo.position - _DirectFrom.position;
        direct = direct.normalized;
        direct.y = 0;

        //var angle = Vector3.Angle(direct, forward);
        var angle = AngleSigned(direct, forward, Vector3.up);

        _DirectGO.transform.localRotation = Quaternion.Euler(0, 0, angle);


        //var direct2D = new Vector2(direct.x, direct.z) - new Vector2(forward.x, forward.z);
        //float atan = Mathf.Atan(direct2D.x / direct2D.y) * Mathf.Rad2Deg;
        //if (direct.y >= forward.y)
        //{
        //    atan = -atan;
        //}
        //else
        //{
        //    atan += 180;
        //    atan = -atan;
        //}

        //_DirectGO.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, atan));
    }

    public static float AngleSigned(Vector3 v1, Vector3 v2, Vector3 n)
    {
        return Mathf.Atan2(
            Vector3.Dot(n, Vector3.Cross(v1, v2)),
            Vector3.Dot(v1, v2)) * Mathf.Rad2Deg;
    }

    private void ShowDirect(Transform directFrom, Transform directTo)
    {
        _DirectFrom = directFrom;
        _DirectTo = directTo;

        //_GOLabel.SetActive(true);
        _DirectGO.SetActive(true);
        _Animation.Play("UIFightWarning");
    }

    public void ShowDirectEnd()
    {
        _DirectFrom = null;
        _DirectTo = null;

        //_GOLabel.SetActive(false);
        _DirectGO.SetActive(false);
    }

    #endregion

    #region event

    public void OnBtnExitFight()
    {
        FightManager.Instance.LogicFinish(true);
    }

    #endregion
}

