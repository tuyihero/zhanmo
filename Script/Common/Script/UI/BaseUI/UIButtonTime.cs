using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIButtonTime : MonoBehaviour {

    public Button _Button;
    public Text _BtnText;
    private string _BtnOriginStr;

    void Awake()
    {
        _BtnOriginStr = _BtnText.text;
    }

	// Use this for initialization
	void Start ()
    {
        _BtnText.text = _BtnOriginStr;
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    #region interact
    private int _DisableCountdown;

    public void SetBtnDisableTime(int disableTime)
    {
        _DisableCountdown = disableTime;
        _Button.interactable = false;
        StartCoroutine(UpdateDisableTime());
    }

    private IEnumerator UpdateDisableTime()
    {
        _BtnText.text = _BtnOriginStr + "(" + _DisableCountdown + "s)";
        yield return new WaitForSeconds(1.0f);
        --_DisableCountdown;
        if (_DisableCountdown == 0)
        {
            _Button.interactable = true;
            _BtnText.text = _BtnOriginStr;
        }
        else
        {
            StartCoroutine(UpdateDisableTime());
        }
    }
    #endregion
}
