using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIRoleSelect2 : UIBase
{

    #region static funs

    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIRoleSelect2, UILayer.PopUI, hash);
    }

    #endregion

    #region 

    public UICameraTexture[] _UICameraTexture;
    public GameObject[] _SelectedGO;
    public GameObject[] _DisableGO;
    public AnimationClip[] _Anims;

    public override void Init()
    {
        base.Init();

        for (int i = 0; i < PlayerDataPack._MAX_ROLE_CNT; ++i)
        {
            InitCharModel(i);
            ShowModel(i);

            if (i == (int)PlayerDataPack.Instance._SelectedRole.Profession)
            {
                _SelectedGO[i].SetActive(true);
            }
            else
            {
                _SelectedGO[i].SetActive(false);
            }
        }
    }

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        if (RoleData.SelectRole.TotalLevel >= GameDataValue.ROLE_SELECT)
        {
            for (int i = 0; i < _DisableGO.Length; ++i)
            {
                _DisableGO[i].SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < _DisableGO.Length; ++i)
            {
                _DisableGO[i].SetActive(true);
            }
        }
    }

    #endregion

    #region interaction

    public void SelectRole(int idx)
    {
        for (int i = 0; i < _UICameraTexture.Length; ++i)
        {
            if (i == idx)
            {
                _ShowAnims[i].PlayAnim();
                _SelectedGO[i].SetActive(true);
            }
            else
            {
                _SelectedGO[i].SetActive(false);
            }
        }
    }

    public void ChangeRole(int idx)
    {
        PlayerDataPack.Instance.SelectRole(idx);
        Hide();
    }

    #endregion

    #region gameobj

    private List<UIModelAnim> _ShowAnims = new List<UIModelAnim>();

    private int _DefaultShowIdx = -1;


    public void ShowModel(int idx)
    {
        //for (int i = 0; i < _UICameraTexture.Length; ++i)
        //{
        //    if (i != idx)
        //    {
        //        _UICameraTexture[i].gameObject.SetActive(false);
        //    }
        //    else
        //    {
        //        _UICameraTexture[i].gameObject.SetActive(true);
        //    }
        //}

        _UICameraTexture[idx].gameObject.SetActive(true);
    }

    public void InitCharModel(int idx)
    {
        _ShowAnims.Add(null);
        string modelName = PlayerDataPack.Instance._RoleList[idx].ModelName;
        string weaponName = PlayerDataPack.Instance._RoleList[idx].DefaultWeaponModel;

        StartCoroutine(ResourcePool.Instance.LoadCharModel(modelName, weaponName, (resName, resGO, hash) =>
        {
            var modelAnim = resGO.AddComponent<UIModelAnim>();
            List<AnimationClip> anims = new List<AnimationClip>();
            anims.Add(_Anims[idx * 2]);
            anims.Add(_Anims[idx * 2 + 1]);
            modelAnim.InitAnim(anims, false);
            modelAnim.PlayAnim(1);
            _ShowAnims[idx] = (modelAnim);

            _UICameraTexture[idx].InitShowGO(resGO);
            if (_DefaultShowIdx == idx)
            {
                ShowModel(_DefaultShowIdx);
            }
        }, null));
    }

    #endregion
}

