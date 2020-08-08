using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

// A behaviour that is attached to a playable
public class TalkCanvasControlPlayableBehaviour : PlayableBehaviour
{
    [Header("游玩屏幕变透明时间")]
    public float playCanvasAlphaTo0Time;
    [Header("游玩屏幕重现时间")]
    public float playCanvasAlphaTo1Time;
    [Header("对话框出现时间")]
    public float talkCanvasAlphaTo1Time;
    [Header("对话框变透明时间")]
    public float talkCanvasAlphaTo0Time;
    [Header("是否暂停下面的TimeLine")]
    public bool pauseTL;
    [Header("TIMELINE")]
    public act.game.TimeLineType timeline;

    public bool gameover = false;
    public bool isOver
    {
        get
        {
            return checkIsOver;
        }
        set
        {
            Debug.Log(checkIsOver);
            checkIsOver = value;
        }
    }

    private bool checkIsOver = false;
    public bool ZUJiao;

    public string content;
    public float time;
    private act.ui.TalkCanvas talkCanvas;
    private act.ui.PlayCanvas playCanvas;
    private bool talkCanvasPause = false;
    // Called when the owning graph starts playing
    public override void OnGraphStart(Playable playable)
    {
        talkCanvas = act.ui.UiManager.instance.CreateUi<act.ui.TalkCanvas>();
        playCanvas = act.ui.UiManager.instance.CreateUi<act.ui.PlayCanvas>();
        //act.ui.UiManager.instance.SetUIAlpha(act.ui.UiManager.instance.CreateUi<act.ui.PlayCanvas>(), 1, 0, playCanvasAlphaTo0Time);
        act.ui.UiManager.instance.SetUIAlpha(playCanvas, 0, time: playCanvasAlphaTo0Time);
    }

    // Called when the owning graph stops playing
    public override void OnGraphStop(Playable playable)
    {
        
    }

    // Called when the state of the playable is set to Play
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        act.ui.UiManager.instance.ControlMouseInput(playCanvas, false);
        talkCanvas.talkContentString = content;
        talkCanvas.talkTime = time;
        act.ui.UiManager.instance.OpenUi<act.ui.TalkCanvas>();
        act.ui.UiManager.instance.CreateUi<act.ui.TalkCanvas>().Refresh();
        act.ui.UiManager.instance.SetUIAlpha(talkCanvas,  1, time: talkCanvasAlphaTo1Time, immediate: false, onComplete: ()=> { });
        if(pauseTL)
        {
            talkCanvas.SetNextBtn(() => {
                act.game.TimeLineMgr.instance.ResumeTimeLine(act.game.GameController.instance.timelines[(int)timeline]);
            });
        }
        if(ZUJiao)
        {
            act.game.GameFlowMgr.instance.EnterToProcessTwo();
        }
        if(isOver)
        {
            act.ui.UiManager.instance.ControlMouseInput(playCanvas, true);
            act.ui.UiManager.instance.SetUIAlpha(playCanvas, 1, time: playCanvasAlphaTo1Time);
            act.ui.UiManager.instance.SetUIAlpha(talkCanvas, 0, time: talkCanvasAlphaTo0Time, immediate: false, onComplete: () => { act.ui.UiManager.instance.CloseUi<act.ui.TalkCanvas>(); });
        }
        if(gameover)
        {
            act.ui.UiManager.instance.CreateUi<act.ui.PlayCanvas>().ReturnToMain();
        }
    }

    // Called when the state of the playable is set to Paused
    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        if(talkCanvasPause)
        {
            act.game.TimeLineMgr.instance.PasueTimeline(act.game.GameController.instance.timelines[(int)timeline], () => {

            });
        }
    }

    // Called each frame while the state is set to Play
    public override void PrepareFrame(Playable playable, FrameData info)
    {
        if(info.weight > 0 && pauseTL)
        {
            talkCanvasPause = true;
            pauseTL = false;
        }
    }
}
