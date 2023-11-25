using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            Debug.Log("bull");
            return;
        }
        Debug.Log("play");
        _seAudioSource.PlayOneShot(clip);
    }
}
