using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIRoleSelect : UIBase
{

    #region static funs

    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIRoleSelect, UILayer.PopUI, hash);
    }

    #endregion

    #region 

    public UICameraTexture _UICameraTexture;
    public GameObject[] _ProMain;
    public AnimationClip[] _Anims;
    public GameObject _SubProGO;

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        //for (int i = 0; i < PlayerDataPack._MAX_ROLE_CNT; ++i)
        //{
        //    InitCharModel(i);
        //}
        _SelectRoleID = PlayerDataPack.Instance._LastSelectRole;
        SelectRole(_SelectRoleID);

        if (PlayerDataPack.Instance.RoleLevel >= GameDataValue._ROLE_OPEN_LEVEL)
        {
            _SubProGO.SetActive(true);
        }
        else
        {
            _SubProGO.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        //foreach (var animGO in _ShowAnims)
        //{
        //    GameObject.Destroy(animGO);
        //}
    }

    #endregion

    #region event

    private int _SelectRoleID = 0;
    private int _SelectSex = 0;
    private List<int> _SelectRecords = new List<int>() { 0, 1,2};

    public void SelectRole(int roleID)
    {
        _SelectRoleID = roleID;
        if (_SelectRoleID == (int)Tables.PROFESSION.WARRIOR)
        {
            _SelectSex = 0;
            _ProMain[0].SetActive(true);
            _ProMain[1].SetActive(false);
            _ProMain[2].SetActive(false);
        }
        else if(_SelectRoleID == (int)Tables.PROFESSION.ASSASSIN)
        {
            _SelectSex = 0;
            _ProMain[0].SetActive(false);
            _ProMain[1].SetActive(true);
            _ProMain[2].SetActive(false);

        }
        else if (_SelectRoleID == (int)Tables.PROFESSION.MAGE)
        {
            _SelectSex = 1;
            _ProMain[0].SetActive(false);
            _ProMain[1].SetActive(false);
            _ProMain[2].SetActive(true);

        }
        
        _SelectRecords[_SelectSex] = _SelectRoleID;
        ShowModel(roleID);
    }

    public void OnProSelect1(int idx)
    {
        //_SelectSex = idx;
        SelectRole(_SelectRecords[idx]);
    }

    public void OnProSelect2(int idx)
    {

        _SelectRoleID = idx;

        SelectRole(_SelectRoleID);
    }

    public void OnBtnOK()
    {
        LogicManager.Instance.StartLoadRole(_SelectRoleID);
        Hide();
        UIManager.Instance.DestoryUI(UIConfig.UIRoleSelect.AssetPath);
    }
    #endregion

    #region gameobj

    private Dictionary<int, UIModelAnim> _ShowAnims = new Dictionary<int, UIModelAnim>();

    private int _DefaultShowIdx = -1;

    public void ShowModel(int idx)
    {

        //StartCoroutine(InitCharModel(idx));
    }

    public IEnumerator InitCharModel(int idx)
    {
        if (!_ShowAnims.ContainsKey(idx))
        {
            _ShowAnims.Add(idx, null);
            string modelName = PlayerDataPack.Instance._RoleList[idx].ModelName;
            string weaponName = PlayerDataPack.Instance._RoleList[idx].DefaultWeaponModel;

            yield return (ResourcePool.Instance.LoadCharModel(modelName, weaponName, (resName, resGO, hash) =>
            {
                Debug.Log("UIRoleSelect InitCharModel");
                var modelAnim = resGO.AddComponent<UIModelAnim>();
                List<AnimationClip> anims = new List<AnimationClip>();
                anims.Add(_Anims[idx * 2]);
                anims.Add(_Anims[idx * 2 + 1]);
                modelAnim.InitAnim(anims, false);

                _ShowAnims[idx] = (modelAnim);
                _UICameraTexture.InitShowGO(resGO);
                _ShowAnims[idx].PlayAnim();

            }, null));
        }
        else
        {
            _UICameraTexture.InitShowGO(_ShowAnims[idx].gameObject);
            _ShowAnims[idx].PlayAnim();
        }
        
    }

    #endregion
}

