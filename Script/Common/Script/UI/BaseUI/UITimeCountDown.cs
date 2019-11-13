using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Text))]
public class UITimeCountDown : MonoBehaviour
{
    
    public Text _CountDownTip;

    void Awake()
    {
        _CountDownTip = gameObject.GetComponent<Text>();
    }

	// Use this for initialization
	void Start ()
    {
        
    }

    #region interact

    public enum CountDownType
    {
        None,
    }

    private System.Action _FinishCallBack;
    private System.TimeSpan _CountDownTime;
    private bool _StartCountDown = false;

    public void SetCountDownSecond(int second, CountDownType type, System.Action finishCallBack)
    {
        _FinishCallBack = finishCallBack;
        _CountDownTime = System.TimeSpan.FromSeconds(second);
        _StartCountDown = true;

        StopCoroutine("UpdateCountDown");
        StartCoroutine("UpdateCountDown");
    }

    private IEnumerator UpdateCountDown()
    {
        while (true)
        {
            _CountDownTime = _CountDownTime - System.TimeSpan.FromSeconds(1);
            _CountDownTip.text = string.Format("{0:MM:ss}", _CountDownTime);

            if (_CountDownTime.TotalSeconds == 0)
            {
                if (_FinishCallBack != null)
                {
                    _FinishCallBack.Invoke();
                }
                yield break;
            }
            else
            {
                yield return new WaitForSeconds(1);
            }
        }
    }

    #endregion
}
