using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource _AudioSource;
    public AudioClip _LogicAudio;

    void OnEnable()
    {
        GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_SYSTEMSETTING_CHANGE, OnSettingChange);
    }

    public void PlayBGMusic(string music)
    {
        ResourceManager.Instance.LoadAudio(music, (resName, resGO, hash) =>
        {
            PlayBGMusic(resGO, 0.9f);
        }, null);
    }

    public void PlayBGMusic(AudioClip logicAudio, float volumn = 0.5f)
    {
        _AudioSource.clip = (logicAudio);
        _AudioSource.volume = volumn;
        _AudioSource.loop = true;
        _AudioSource.Play();
    }

    public void PlayEffectSound(AudioClip soundEffect)
    {

    }

    private void OnSettingChange(object go, Hashtable eventArgs)
    {
        AudioListener.volume = GlobalValPack.Instance.Volume;
    }

}
