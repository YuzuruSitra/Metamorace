using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundHandler : MonoBehaviour
{
    public static SoundHandler InstanceSoundHandler;
    [SerializeField]
    private AudioSource _bgmAudioSource;
    [SerializeField]
    private AudioSource _seAudioSource;

    // BGM音量変更
    public void SetNewValueBGM(float newValueBGM)
    {
        _bgmAudioSource.volume = Mathf.Clamp01(newValueBGM);
    }

    // BGM音量変更
    public void SetNewValueSE(float newValueSE)
    {
        _seAudioSource.volume = Mathf.Clamp01(newValueSE);
    }

    // スライダーの値を変更
    public void ChangeSliderValue(Slider BGM, Slider SE)
    {
        BGM.value = _bgmAudioSource.volume;
        SE.value = _seAudioSource.volume;
    }

    void Awake()
    {
        if (InstanceSoundHandler != null)
        {
            Destroy(gameObject);
            return;
        }

        InstanceSoundHandler = this;
        DontDestroyOnLoad(gameObject);
    }


    public void PlayBGM(AudioClip clip)
    {
        _bgmAudioSource.clip = clip;
        if(clip == null)
        {
            return;
        }
        _bgmAudioSource.Play();
    }

    public void PlaySE(AudioClip clip)
    {   
        if(clip == null)
        {
            return;
        }

        _seAudioSource.PlayOneShot(clip);
    }
}
