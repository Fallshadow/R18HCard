using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class TalkControlPlayableAsset : PlayableAsset
{
    public ExposedReference<act.worldui.TalkChildPrefab> talkExposedReference;
    public ExposedReference<GameObject> followItem;
    public string content;

    // Factory method that generates a playable based on this asset
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        var playable = ScriptPlayable<TalkControlPlayableBehaviour>.Create(graph);
        playable.GetBehaviour().talkChildPrefab = talkExposedReference.Resolve(graph.GetResolver());
        playable.GetBehaviour().followItem = followItem.Resolve(graph.GetResolver());
        playable.GetBehaviour().content = content;
        return playable;
    }
}
