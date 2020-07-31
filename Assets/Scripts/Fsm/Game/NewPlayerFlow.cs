using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace act.fsm
{
    public class NewPlayerFlow<T> : State<T>
    {
        public override void Enter()
        {
            game.GameController.instance.isInNewPlayFlow = true;
            game.TimeLineMgr.instance.PlayTimeline(game.TimeLineMgr.instance.newPlayerDir);
            ui.UiManager.instance.CreateUi<ui.PlayCanvas>();
            ui.UiManager.instance.OpenUi<ui.PlayCanvas>();
        }

        public override void Exit()
        {

        }

        public override void Update()
        {
        }
    }
}

