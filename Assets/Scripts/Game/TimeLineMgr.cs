﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;


namespace act.game
{
    public class TimeLineMgr : SingletonMonoBehavior<TimeLineMgr>
    {
        private void Start()
        {
            gameDir = GetComponent<PlayableDirector>();
        }

        public PlayableDirector gameDir;
        public PlayableDirector newPlayerDir;
        private System.Action _pasuedCallBackFunction = null;

        public void PasueTimeline(System.Action callBack = null)
        {
            gameDir.Pause();
            _pasuedCallBackFunction += callBack;
        }

        public void ResumeTimeLine(bool exeCallBack = false)
        {
            gameDir.Resume();
            _pasuedCallBackFunction?.Invoke();
            _pasuedCallBackFunction = null;
        }

        public void JumpToTime(float pointTime)
        {
            gameDir.time = pointTime;
        }

        #region 指定导演系统，真的会有用到么,还真的有用到
        public void PlayTimeline(PlayableDirector playableDirector, System.Action callBack = null)
        {
            playableDirector.Play();
        }
        public void PlayTimeline(PlayableDirector playableDirector, PlayableAsset playableAsset)
        {
            playableDirector.playableAsset = playableAsset;
            playableDirector.Play();
        }
        public void PasueTimeline(PlayableDirector playableDirector, System.Action callBack = null)
        {
            playableDirector.Pause();
            _pasuedCallBackFunction += callBack;
        }

        public void ResumeTimeLine(PlayableDirector playableDirector, bool exeCallBack = false)
        {
            playableDirector.Resume();
            _pasuedCallBackFunction?.Invoke();
            _pasuedCallBackFunction = null;
        }

        public void JumpToTime(PlayableDirector playableDirector, float pointTime)
        {
            playableDirector.time = pointTime;
        }
        #endregion

        public PlayableAsset LoadTimeline(string fileName)
        {
            PlayableAsset playableAsset = act.utility.LoadResources.LoadAsset<PlayableAsset>("model/Motion/" + fileName);
            return playableAsset;
        }

        public void PlayPlayableAsset(PlayableAsset playableAsset, PlayableDirector playableDirector = null)
        {
            if(playableAsset == null)
            {
                Debug.Log("并没有读取到对应TimeLine");
                return;
            }

            if(playableDirector == null)
            {
                playableDirector = gameDir;
            }

            playableDirector.playableAsset = playableAsset;
            playableDirector.Play();
        }

        public void PlayPlayableAsset(string fileName, PlayableDirector playableDirector = null)
        {
            PlayableAsset playableAsset = act.utility.LoadResources.LoadAsset<PlayableAsset>("TimeLine/事件timeline/" + fileName);
            if(playableAsset == null)
            {
                Debug.Log("并没有读取到对应TimeLine");
                return;
            }

            if(playableDirector == null)
            {
                playableDirector = gameDir;
            }
            
            playableDirector.playableAsset = playableAsset;
            playableDirector.Play();
        }

        public void PlayActivePlayableAsset(string fileName, PlayableDirector playableDirector = null)
        {
            
            playableDirector.Play();
        }
    }
}

