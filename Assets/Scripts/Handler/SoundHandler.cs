using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundHandler : MonoBehaviour
{
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

    void Start()
    {
        GameObject soundManager = CheckOtherSoundManager();
        bool checkResult = soundManager != null && soundManager != gameObject;

        if (checkResult)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    GameObject CheckOtherSoundManager()
    {
        return GameObject.FindGameObjectWithTag("SoundHandler");
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
