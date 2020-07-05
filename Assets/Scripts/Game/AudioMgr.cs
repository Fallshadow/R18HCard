using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum AudioClips
{
    AC_Btn = 0,
    AC_Menu = 1,
    AC_Title = 2,
    AC_RoundOver = 3,
}

public class AudioMgr : SingletonMonoBehavior<AudioMgr>
{
    public AudioSource musicAS1;
    public AudioSource musicAS2;
    public float dur;
    public AudioSource soundAS;

    [SerializeField] public AudioClip[] audioClips;
    private AudioSource isPlaying = null;
    private AudioSource isPauseing = null;
    private float MaxVol = 1;
    public void PlaySound(AudioClip audioClip)
    {
        soundAS.clip = audioClip;
        soundAS.Play();
    }
    public void PlayMusicImm(AudioClip audioClip)
    {
        if(isPlaying != null)
        {
            isPlaying = musicAS1;
        }

        isPlaying.clip = audioClip;
        isPlaying.Play();
        isPlaying.Pause();
    }
    public void PlayMusicFade(AudioClip audioClip)
    {
        musicAS1.DOKill();
        musicAS2.DOKill();
        if(isPlaying != null && isPauseing != null)
        {
            isPlaying = isPlaying == musicAS1 ? musicAS2 : musicAS1;
            isPauseing = isPauseing == musicAS1 ? musicAS2 : musicAS1;
        }
        else
        {
            isPlaying = musicAS1;
            isPauseing = musicAS2;
        }
        isPlaying.volume = 0;
        isPauseing.volume = MaxVol;
        isPlaying.clip = audioClip;
        isPlaying.Play();
        isPlaying.DOFade(MaxVol, dur);
        isPauseing.DOFade(MaxVol, dur).OnComplete(()=> { isPauseing.Pause(); });
    }

    public AudioClip GetAudioClip(AudioClips clip)
    {
        return audioClips[(int)clip];
    }

    public void PlaySound(AudioClips clip)
    {
        PlaySound(GetAudioClip(clip));
    }

    public void PlayMusicImm(AudioClips clip)
    {
        PlayMusicImm(GetAudioClip(clip));
    }
    public void PlayMusicFade(AudioClips clip)
    {
        PlayMusicFade(GetAudioClip(clip));
    }
}
