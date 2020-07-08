using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[System.Serializable]
public class NewPlayableAsset : PlayableAsset,ITimelineClipAsset
{
    NewPlayableBehaviour NewPlayableBehaviour = new NewPlayableBehaviour();
    public ClipCaps clipCaps
    {
        get
        {
            return ClipCaps.All;
        }
    }

    // Factory method that generates a playable based on this asset
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        Playable playable = ScriptPlayable<NewPlayableBehaviour>.Create(graph, NewPlayableBehaviour);
        return playable;
    }
}
