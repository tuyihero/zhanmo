using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animation))]
public class UIModelAnim : MonoBehaviour
{

    private Animation _Animation;
    private List<AnimationClip> _Anims;

    public void Awake()
    {
        _Animation = GetComponent<Animation>();
    }

    public void InitAnim(List<AnimationClip> anims, bool initPlay = true)
    {
        _Anims = anims;
        _Animation.AddClip(_Anims[0], "0");
        _Animation.AddClip(_Anims[1], "1");

        if (initPlay)
        {
            PlayAnim();
        }
    }

    public void PlayAnim(int idx)
    {
        if (idx >= _Anims.Count || idx < 0)
            return;

        _Animation.Play(idx.ToString());
    }

    public void PlayAnim()
    {
        StartCoroutine(PlayerAnim());
    }

    private IEnumerator PlayerAnim()
    {
        
        _Animation.Play("0");

        yield return new WaitForSeconds(_Anims[0].length);

        _Animation.Play("1");

    }

    public void ColliderStart(int idx)
    { }
}
