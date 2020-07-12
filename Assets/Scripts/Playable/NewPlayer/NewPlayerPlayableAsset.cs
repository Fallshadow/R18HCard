using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[System.Serializable]
public class NewPlayerPlayableAsset : PlayableAsset,ITimelineClipAsset
{
    NewPlayerPlayableBehaviour NewPlayableBehaviour = new NewPlayerPlayableBehaviour();
    public bool showTalkWindow;
    public bool ShowTalkWindowText;
    public bool ShowPlayCanvas;
    public bool CreatCard;
    public bool CleanEye;
    public bool FadePlayCanvasDark;
    public bool IsOver;



    public int CardId;
    [TextArea(2, 5)]
    public string text;
    public float dur;
    public bool isPauseTimeLine;

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
        return playable;
    }
}
