using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace act.fsm
{
    public class Battle<T> : State<T>
    {
        public override void Enter()
        {

        }

        public override void Exit()
        {
            m_fsm.SwitchToState((int)fsm.GameFsmState.MAINMENU);
        }

        public override void Update()
        {
        }
    }
}

