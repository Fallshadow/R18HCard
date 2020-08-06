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
    public float musicVol = 0.5f;
    public float envirVol = 0.2f;
    public float soundVol = 0.2f;
    public void PlaySound(AudioClip audioClip)
    {
        soundAS.volume = soundVol;
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
        isPauseing.volume = musicVol;
        //isPauseing.volume = 0;
        isPlaying.clip = audioClip;

        //isPauseing.Pause();
        isPauseing.DOFade(musicVol, outDur).OnComplete(()=> { isPauseing.Pause();
            isPlaying.Play();
            isPlaying.DOFade(musicVol, dur);
        });
    }

    public void PlayEnvirMusic()
    {
        if(isOnEnvir)
        {
            return;        
        }

        isOnEnvir = true;
        PlayASFadeIn(musicEnvir, envirVol, 0,1);
    }
    public void PauseEnvirMusic()
    {
        if(!isOnEnvir)
        {
            return;
        }

        isOnEnvir = false;
        PlayASFadeOut(musicEnvir, envirVol, 0,1);
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

    public void SetMusicVoice(float vol)
    {
        musicVol = vol;
        musicAS1.volume = vol;
        musicAS2.volume = vol;
    }

    public void SetSoundVoice(float vol)
    {
        soundVol = vol;
        soundAS.volume = vol;
    }

    public void SetEnvirVoice(float vol)
    {
        envirVol = vol;
        musicEnvir.volume = vol;
    }
}
