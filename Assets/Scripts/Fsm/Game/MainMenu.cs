using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace act.fsm
{
    public class MainMenu<T> : State<T>
    {
        public override void Enter()
        {
            ui.UiManager.instance.OpenUi<ui.MainMenuCanvas>();
        }

        public override void Exit()
        {
            m_fsm.SwitchToState((int)fsm.GameFsmState.BATTLE);
        }

        public override void Update()
        {
        }
    }
}

