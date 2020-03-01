using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace act.fsm
{
    public class Play<T> : State<T>
    {
        public override void Enter()
        {
            ui.UiManager.instance.CreateUi<ui.PlayCanvas>().Show();
        }

        public override void Exit()
        {

        }

        public override void Update()
        {
        }
    }
}

