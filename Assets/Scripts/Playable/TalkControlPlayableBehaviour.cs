using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

// A behaviour that is attached to a playable
public class TalkControlPlayableBehaviour : PlayableBehaviour
{
    public act.worldui.TalkChildPrefab talkChildPrefab;
    public GameObject followItem;
    public string content;
    // Called when the owning graph starts playing
    public override void OnGraphStart(Playable playable)
    {
        
    }

    // Called when the owning graph stops playing
    public override void OnGraphStop(Playable playable)
    {
        
    }

    // Called when the state of the playable is set to Play
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        talkChildPrefab.followGameObject = followItem;
        talkChildPrefab.ChangeContent(content);
        talkChildPrefab.ChangeAlpha(1);
    }

    // Called when the state of the playable is set to Paused
    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        talkChildPrefab.ChangeAlpha(0);
    }

    // Called each frame while the state is set to Play
    public override void PrepareFrame(Playable playable, FrameData info)
    {
        
    }
}
