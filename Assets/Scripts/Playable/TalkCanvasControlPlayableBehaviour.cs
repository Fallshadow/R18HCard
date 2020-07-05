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
    public string content;
    public float time;
    private act.ui.TalkCanvas talkCanvas;
    private act.ui.PlayCanvas playCanvas;

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
        act.ui.UiManager.instance.ControlMouseInput(playCanvas, true);
        act.ui.UiManager.instance.SetUIAlpha(playCanvas, 1,time:playCanvasAlphaTo1Time);
        act.ui.UiManager.instance.SetUIAlpha(talkCanvas, 0,time:talkCanvasAlphaTo0Time,immediate: false,onComplete: () => { act.ui.UiManager.instance.CloseUi<act.ui.TalkCanvas>(); });
    }

    // Called when the state of the playable is set to Play
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        act.ui.UiManager.instance.ControlMouseInput(playCanvas, false);
        talkCanvas.talkContentString = content;
        talkCanvas.talkTime = time;
        act.ui.UiManager.instance.OpenUi<act.ui.TalkCanvas>();
        act.ui.UiManager.instance.SetUIAlpha(talkCanvas,  1, time: talkCanvasAlphaTo1Time, immediate: false, onComplete: ()=> { });
    }

    // Called when the state of the playable is set to Paused
    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        
    }

    // Called each frame while the state is set to Play
    public override void PrepareFrame(Playable playable, FrameData info)
    {
        
    }
}
