using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class TalkCanvasControlPlayableAsset : PlayableAsset
{
    [Header("游玩屏幕变透明时间")]
    private float playCanvasAlphaTo0Time = 1.78f;
    [Header("游玩屏幕重现时间")]
    private float playCanvasAlphaTo1Time = 1.84f;
    [Header("对话框出现时间")]
    private float talkCanvasAlphaTo1Time = 1.78f;
    [Header("对话框变透明时间")]
    private float talkCanvasAlphaTo0Time = 1.75f;
    [Header("对话框文字内容")]
    [Multiline(5)]
    public string content;
    [Header("对话框文字打字出现速度 单位s")]
    public float time;

    // Factory method that generates a playable based on this asset
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        var playable = ScriptPlayable<TalkCanvasControlPlayableBehaviour>.Create(graph);
        playable.GetBehaviour().content = content;
        playable.GetBehaviour().time = time;
        playable.GetBehaviour().playCanvasAlphaTo0Time = playCanvasAlphaTo0Time;
        playable.GetBehaviour().playCanvasAlphaTo1Time = playCanvasAlphaTo1Time;
        playable.GetBehaviour().talkCanvasAlphaTo1Time = talkCanvasAlphaTo1Time;
        playable.GetBehaviour().talkCanvasAlphaTo0Time = talkCanvasAlphaTo0Time;
        return playable;
    }
}
