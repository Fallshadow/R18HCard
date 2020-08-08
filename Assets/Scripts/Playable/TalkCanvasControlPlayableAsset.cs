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
    [Header("是否暂停下面的TimeLine")]
    public bool pauseTL;
    [Header("TIMELINE")]
    public act.game.TimeLineType timeline;
    [Header("结束了么")]
    public bool isOver = false;
    [Header("游戏结束了么")]
    public bool gameover = false;

    [Header("---------足交回调---------")]
    public bool ZUJiao;
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
        playable.GetBehaviour().timeline = timeline;
        playable.GetBehaviour().pauseTL = pauseTL;
        playable.GetBehaviour().isOver = isOver;
        playable.GetBehaviour().ZUJiao = ZUJiao;
        playable.GetBehaviour().gameover = gameover;
        return playable;
    }
}
