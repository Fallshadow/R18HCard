using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace act.utility
{
    public class CoroutineUtility : SingletonMonoBehavior<CoroutineUtility>
    {
        public float endRoundEventTime = 2;
        public void EndRoundCoroutine()
        {
            StartCoroutine(endRoundCoroutineReal());

        }

        IEnumerator endRoundCoroutineReal()
        {
            yield return new WaitForSeconds(endRoundEventTime);
            game.GameController.instance.FSM.SwitchToState((int)fsm.GameFsmState.GameFlowRoundStart);
        }
    }
}

