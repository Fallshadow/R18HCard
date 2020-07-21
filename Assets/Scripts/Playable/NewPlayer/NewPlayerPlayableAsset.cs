using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[System.Serializable]
public class NewPlayerPlayableAsset : PlayableAsset,ITimelineClipAsset
{
    NewPlayerPlayableBehaviour NewPlayableBehaviour = new NewPlayerPlayableBehaviour();
    [Header("---------展现dark旁白？---------")]
    public bool showTalkWindow;
    [Header("关闭窗口")]
    public bool CloseTalkWindow;
    [Header("展现旁白文本？")]
    public bool ShowTalkWindowText;
    [Header("那你配点数据吧")]
    [TextArea(2, 5)]
    public string text;
    public float dur;

    [Header("---------展现人物对话界面？---------")]
    public bool ShowTalkCanvas;
    public bool ShowTalkCanvasStopTimeLine;
    [Header("那你配点数据吧")]
    [TextArea(2, 5)]
    public string TalkCText;
    public float Cdur;

    [Header("---------展现游玩界面？---------")]
    public bool ShowPlayCanvas;

    [Header("---------想加入一些卡牌吧！---------")]
    public bool CreatCard;
    [Header("请输入卡牌ID！")]
    public int CardId;


    [Header("---------想康康清楚洛璃不---------")]
    public bool CleanEye;

    [Header("可是有这个黑幕在挡着！")]
    public bool FadePlayCanvasDark;
    [Header("---------进入第一回合？---------")]
    public bool StartRound;
    [Header("---------终结本次timeline？---------")]
    public bool IsOver;

    [Header("---------需要暂停？---------")]
    public bool isPauseTimeLine;

    [Header("---------GUIDE？---------")]
    public bool ShowGuide;
    public GuideType guideType;

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
        var playable = ScriptPlayable<NewPlayerPlayableBehaviour>.Create(graph, NewPlayableBehaviour);
        playable.GetBehaviour().ShowTalkWindow = showTalkWindow;
        playable.GetBehaviour().TalkText = text;
        playable.GetBehaviour().dur = dur;
        playable.GetBehaviour().isPauseTimeLine = isPauseTimeLine;
        playable.GetBehaviour().ShowTalkWindowText = ShowTalkWindowText;
        playable.GetBehaviour().ShowPlayCanvas = ShowPlayCanvas;
        playable.GetBehaviour().CreatCard = CreatCard;
        playable.GetBehaviour().CardId = CardId;
        playable.GetBehaviour().CleanEye = CleanEye;
        playable.GetBehaviour().FadePlayCanvasDark = FadePlayCanvasDark;
        playable.GetBehaviour().IsOver = IsOver;
        playable.GetBehaviour().ShowTalkCanvas = ShowTalkCanvas;
        playable.GetBehaviour().TalkCText = TalkCText;
        playable.GetBehaviour().Cdur = Cdur;
        playable.GetBehaviour().ShowTalkCanvasStopTimeLine = ShowTalkCanvasStopTimeLine;
        playable.GetBehaviour().guideType = guideType;
        playable.GetBehaviour().ShowGuide = ShowGuide;
        playable.GetBehaviour().CloseTalkWindow = CloseTalkWindow;
        playable.GetBehaviour().StartRound = StartRound;
        return playable;
    }
}
