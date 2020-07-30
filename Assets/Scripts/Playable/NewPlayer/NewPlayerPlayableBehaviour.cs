using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

// A behaviour that is attached to a playable
public class NewPlayerPlayableBehaviour : PlayableBehaviour
{
    public bool isPauseTimeLine;
    public bool ShowTalkWindow;
    public bool CloseTalkWindow;
    public bool ShowTalkWindowText;
    public bool ShowPlayCanvas;
    public bool CreatCard;
    public int CardId;
    public bool CleanEye;
    public bool FadePlayCanvasDark;
    public bool IsOver;
    public bool ShowTalkCanvas;
    public bool ShowTalkCanvasStopTimeLine;
    public bool ShowGuide;
    public bool StartRound;
    public GuideType guideType;
    public string TalkCText;
    public float Cdur;
    public string TalkText;
    public float dur;

    private bool perPause;
    private bool talkCanvasPause;
    private bool showGuidePause;
    private act.ui.TalkWindow TalkWindow;
    private act.ui.PlayCanvas PlayCanvas;
    private act.ui.TalkCanvas TalkCanvas;
    // Called when the owning graph starts playing
    public override void OnGraphStart(Playable playable)
    {
        if(act.game.GameFlowMgr.instance.processTwo)
        {
            PlayCanvas = act.ui.UiManager.instance.CreateUi<act.ui.PlayCanvas>();
            GuideController.instance.StartGuide();
        }
        else
        {
            TalkWindow = act.ui.UiManager.instance.CreateUi<act.ui.TalkWindow>();
            TalkWindow.ResetText();
            PlayCanvas = act.ui.UiManager.instance.CreateUi<act.ui.PlayCanvas>();
            PlayCanvas.ShowDrak(true);
            TalkCanvas = act.ui.UiManager.instance.CreateUi<act.ui.TalkCanvas>();
        }
    }

    // Called when the owning graph stops playing
    public override void OnGraphStop(Playable playable)
    {
        
    }

    // Called when the state of the playable is set to Play
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        act.ui.UiManager.instance.ControlMouseInput(PlayCanvas, false);

        if(ShowTalkWindow && ShowTalkWindowText)
        {
            TalkWindow.dur = dur;
            TalkWindow.ShowText(TalkText);
        }
        if(ShowPlayCanvas)
        {
            TalkWindow.FadeDark(2);
        }
        if(CloseTalkWindow)
        {
            TalkWindow.Close();
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
            if(act.game.GameController.instance.isInNewPlayFlow)
            {
                act.game.GameController.instance.isInNewPlayFlow = false;
            }
            act.ui.UiManager.instance.ControlMouseInput(PlayCanvas, true);
            GuideController.instance.OverGuide();
        }
        if(StartRound)
        {
            act.game.GameController.instance.FSM.SwitchToState((int)act.fsm.GameFsmState.GameFlowRoundStart);
        }
        if(ShowTalkCanvas)
        {
            act.ui.UiManager.instance.ControlMouseInput(PlayCanvas, false);
            TalkCanvas.talkContentString = TalkCText;
            TalkCanvas.talkTime = Cdur;
            act.ui.UiManager.instance.SetUIAlpha(PlayCanvas, 0, time: 1);
            act.ui.UiManager.instance.OpenUi<act.ui.TalkCanvas>();
            act.ui.UiManager.instance.SetUIAlpha(TalkCanvas, 1, time: 2, immediate: false, onComplete: () => {
                act.ui.UiManager.instance.SetUIAlpha(TalkCanvas, 0, time: 2, immediate: false, onComplete: () => {
                    act.ui.UiManager.instance.ControlMouseInput(PlayCanvas, true);
                    act.ui.UiManager.instance.CloseUi<act.ui.TalkCanvas>();
                    act.ui.UiManager.instance.SetUIAlpha(PlayCanvas, 1, time: 2);
                });
            });
        }
        if(ShowTalkCanvasStopTimeLine)
        {
            act.ui.UiManager.instance.ControlMouseInput(PlayCanvas, false);
            TalkCanvas.talkContentString = TalkCText;
            TalkCanvas.talkTime = Cdur;
            TalkCanvas.SetNextBtn(() => {
                act.game.TimeLineMgr.instance.ResumeTimeLine(act.game.TimeLineMgr.instance.newPlayerDir);
            });
            act.ui.UiManager.instance.SetUIAlpha(PlayCanvas, 0, time: 1);
            act.ui.UiManager.instance.OpenUi<act.ui.TalkCanvas>();
            act.ui.UiManager.instance.SetUIAlpha(TalkCanvas, 1, time: 2, immediate: false, onComplete: () => { });
        }

        if(ShowGuide)
        {
            act.ui.UiManager.instance.ControlMouseInput(PlayCanvas, true);
            GuideController.instance.Guide(guideType);
        }
    }

    // Called when the state of the playable is set to Paused
    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        if(perPause)
        {
            act.game.TimeLineMgr.instance.PasueTimeline(act.game.TimeLineMgr.instance.newPlayerDir,()=>{ 
                TalkWindow.HideText(); 
            });
            perPause = false;
        }
        if(talkCanvasPause)
        {
            act.game.TimeLineMgr.instance.PasueTimeline(act.game.TimeLineMgr.instance.newPlayerDir, () => {
                act.ui.UiManager.instance.ControlMouseInput(PlayCanvas, true);
                act.ui.UiManager.instance.CloseUi<act.ui.TalkCanvas>();
                act.ui.UiManager.instance.SetUIAlpha(PlayCanvas, 1, time: 2);
            });
            talkCanvasPause = false;
        }
        if(showGuidePause)
        {
            act.game.TimeLineMgr.instance.PasueTimeline(act.game.TimeLineMgr.instance.newPlayerDir);
            showGuidePause = false;
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

        if(info.weight > 0 && ShowTalkCanvasStopTimeLine)
        {
            talkCanvasPause = true;
            ShowTalkCanvasStopTimeLine = false;
        }
        if(info.weight > 0 && ShowGuide)
        {
            showGuidePause = true;
            ShowGuide = false;
        }
        
    }
}
