using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

// A behaviour that is attached to a playable
public class NewPlayerPlayableBehaviour : PlayableBehaviour
{
    public bool isPauseTimeLine;

    public bool ShowTalkWindow;
    public bool ShowTalkWindowText;
    public bool ShowPlayCanvas;
    public bool CreatCard;
    public int CardId;
    public bool CleanEye;
    public bool FadePlayCanvasDark;
    public bool IsOver;
    public string TalkText;
    public float dur;

    private bool perPause;
    private act.ui.TalkWindow TalkWindow;
    private act.ui.PlayCanvas PlayCanvas;
    // Called when the owning graph starts playing
    public override void OnGraphStart(Playable playable)
    {
        TalkWindow = act.ui.UiManager.instance.CreateUi<act.ui.TalkWindow>();
        TalkWindow.ResetText();
        PlayCanvas = act.ui.UiManager.instance.CreateUi<act.ui.PlayCanvas>();
        PlayCanvas.ShowDrak(true);
    }

    // Called when the owning graph stops playing
    public override void OnGraphStop(Playable playable)
    {
        
    }

    // Called when the state of the playable is set to Play
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        if(ShowTalkWindow && ShowTalkWindowText)
        {
            TalkWindow.dur = dur;
            TalkWindow.ShowText(TalkText);
        }
        if(ShowPlayCanvas)
        {
            TalkWindow.FadeDark(2);
        }

        if(CreatCard)
        {
            PlayCanvas.CreateCard(CardId);
        }
        if(CleanEye)
        {
            act.game.GameController.instance.mainCamera.SetActive(true);
        }
        if(FadePlayCanvasDark)
        {
            PlayCanvas.FadeDrak(2);
        }

        if(IsOver)
        {
            act.game.GameController.instance.FSM.SwitchToState((int)act.fsm.GameFsmState.GameFlowRoundStart);
        }
    }

    // Called when the state of the playable is set to Paused
    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        if(perPause)
        {
            act.game.TimeLineMgr.instance.PasueTimeline(act.game.TimeLineMgr.instance.newPlayerDir,()=>{ TalkWindow.HideText(); });
        }
    }

    // Called each frame while the state is set to Play
    public override void PrepareFrame(Playable playable, FrameData info)
    {
        if(info.weight > 0 && isPauseTimeLine)
        {
            perPause = true;
            isPauseTimeLine = false;
        }
    }
}
