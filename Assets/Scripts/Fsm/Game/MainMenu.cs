using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace act.fsm
{
    public class MainMenu<T> : State<T>
    {
        public override void Enter()
        {
            ui.UiManager.instance.CreateUi<ui.MainMenuCanvas>().Show();
        }

        public override void Exit()
        {

        }

        public override void Update()
        {
        }
    }
}

