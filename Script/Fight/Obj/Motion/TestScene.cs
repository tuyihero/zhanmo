using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScene : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        Init();

    }
	
	// Update is called once per frame
	void Update ()
    {
        UpdateAnim();

    }

    #region test monster

    Animator _BodyAnimator;
    Animator _EffectAnimator;

    public void Init()
    {
        if (_BodyAnimator != null)
            return;

        var body = transform.Find("Body");
        _BodyAnimator = body.GetComponent<Animator>();
        var effect = transform.Find("Body/Effect");
        _EffectAnimator = effect.GetComponent<Animator>();
    }

    public void UpdateAnim()
    {
        if (Input.GetKeyDown("1"))
        {
            _BodyAnimator.Play("skill1", 0);
            if (_EffectAnimator != null)
            {
                _EffectAnimator.Play("skill1", 0);
            }
        }
        else if (Input.GetKeyDown("2"))
        {
            _BodyAnimator.Play("skill2", 0);
            if (_EffectAnimator != null)
            {
                _EffectAnimator.Play("skill2", 0);
            }
        }
        else if (Input.GetKeyDown("3"))
        {
            _BodyAnimator.Play("run", 0);
            if (_EffectAnimator != null)
            {
                _EffectAnimator.Play("run", 0);
            }
        }
    }

    #endregion
}
