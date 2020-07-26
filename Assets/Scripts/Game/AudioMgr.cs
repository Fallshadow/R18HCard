using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum AudioClips
{
    AC_Envir = 0,//环境音
    AC_Title = 1,//尼古拉斯标题音
    AC_kuang = 2,//教程和对话框音
    AC_TitleBGM = 3,//标题界面BGN
    AC_ClickEvent = 4,//点击事件
    AC_OneBGM = 5,//第一阶段
    AC_TwoBGM = 6,//第二阶段
    AC_PlayClick = 7,//游玩界面按钮
    AC_ProcessGet = 8,//进度值获取
    AC_TitleBtn = 9,//标题界面BTN music
}

public class AudioMgr : SingletonMonoBehavior<AudioMgr>
{
    public AudioSource musicAS1;
    public AudioSource musicAS2;
    public AudioSource musicEnvir;
    public float dur;
    public float outDur;
    public AudioSource soundAS;

    [SerializeField] public AudioClip[] audioClips;
    private AudioSource isPlaying = null;
    private AudioSource isPauseing = null;

    private bool isOnEnvir = false;
    public float MaxVol = 0.5f;
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
        //isPauseing.volume = 0;
        isPlaying.clip = audioClip;

        //isPauseing.Pause();
        isPauseing.DOFade(MaxVol, outDur).OnComplete(()=> { isPauseing.Pause();
            isPlaying.Play();
            isPlaying.DOFade(MaxVol, dur);
        });
    }

    public void PlayEnvirMusic()
    {
        if(isOnEnvir)
        {
            return;        
        }

        isOnEnvir = true;
        PlayASFadeIn(musicEnvir, 0.5f, 0,1);
    }
    public void PauseEnvirMusic()
    {
        if(!isOnEnvir)
        {
            return;
        }

        isOnEnvir = false;
        PlayASFadeOut(musicEnvir, 0.5f, 0,1);
    }

    public void PlayASFadeIn(AudioSource audioSource,float asMaxVol,float asMinVol, float dur)
    {
        audioSource.DOKill();
        audioSource.Play();
        audioSource.volume = asMinVol;
        audioSource.DOFade(asMaxVol, dur);
    }
    public void PlayASFadeOut(AudioSource audioSource, float asMaxVol, float asMinVol,float dur)
    {
        audioSource.DOKill();
        audioSource.volume = asMaxVol;
        audioSource.DOFade(asMinVol, dur).OnComplete(()=> {
            audioSource.Pause();
        });
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
