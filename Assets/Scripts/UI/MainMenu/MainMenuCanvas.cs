using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace act.ui
{
    [BindingResource("MainMenu/MainMenuCanvas")]
    public class MainMenuCanvas : InteractableUiBase
    {
        public override UiOpenType OpenType => base.OpenType;

        public override void Initialize()
        {

        }

        public override void Refresh()
        {

        }

        public override void Release()
        {

        }
        #region 
        public void TurnToPlay()
        {
            Hide();
            game.GameController.instance.FSM.SwitchToState((int)fsm.GameFsmState.PLAP);
        }
        #endregion
    }
}