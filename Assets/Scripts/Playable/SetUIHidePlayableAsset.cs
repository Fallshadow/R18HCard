using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

[System.Serializable]
public class SetUIHidePlayableAsset : PlayableAsset
{
    //public ExposedReference<Text> targetText;

    // Factory method that generates a playable based on this asset
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        var playable = ScriptPlayable<SetUIHidePlayableBehaviour>.Create(graph);
        //targetText.Resolve(graph.GetResolver());
        //playable.GetBehaviour().其中的安排
        return playable;
    }
}
