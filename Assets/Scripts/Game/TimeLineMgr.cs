using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;


namespace act.game
{
    public class TimeLineMgr : SingletonMonoBehavior<TimeLineMgr>
    {
        private PlayableDirector defaultPlayableDirector;
        private void Start()
        {
            defaultPlayableDirector = GetComponent<PlayableDirector>();
        }
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
                playableDirector = defaultPlayableDirector;
            }

            playableDirector.playableAsset = playableAsset;
            playableDirector.Play();
        }

        public void PlayPlayableAsset(string fileName, PlayableDirector playableDirector = null)
        {
            PlayableAsset playableAsset = act.utility.LoadResources.LoadAsset<PlayableAsset>("TimeLine/" + fileName);
            if(playableAsset == null)
            {
                Debug.Log("并没有读取到对应TimeLine");
                return;
            }

            if(playableDirector == null)
            {
                playableDirector = defaultPlayableDirector;
            }
            
            playableDirector.playableAsset = playableAsset;
            playableDirector.Play();
        }
    }
}

