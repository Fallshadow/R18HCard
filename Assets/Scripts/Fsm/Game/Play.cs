using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;


namespace act.fsm
{
    public class Play<T> : State<T>
    {
        private bool isFirstGame = true;
        public override void Enter()
        {
            if(isFirstGame)
            {
                isFirstGame = false;
                act.game.GameController.instance.uiCamera.GetComponent<PostProcessVolume>().enabled = true;
                act.game.GameController.instance.uiCamera.GetComponent<PostProcessLayer>().enabled = true;
                game.GameController.instance.FSM.SwitchToState((int)fsm.GameFsmState.NewPlayerFlow);
            }
            else
            {
                ui.UiManager.instance.CreateUi<ui.PlayCanvas>().Show();
                act.game.GameController.instance.mainCamera.SetActive(true);
                act.game.GameController.instance.uiCamera.GetComponent<PostProcessVolume>().enabled = true;
                act.game.GameController.instance.uiCamera.GetComponent<PostProcessLayer>().enabled = true;
            }
        }

        public override void Exit()
        {

        }

        public override void Update()
        {
        }
    }
}

