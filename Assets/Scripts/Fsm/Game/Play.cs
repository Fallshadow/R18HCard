using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;


namespace act.fsm
{
    public class Play<T> : State<T>
    {
        public override void Enter()
        {
            if(game.GameController.instance.isInNewPlayFlow)
            {
                game.GameController.instance.isInNewPlayFlow = false;
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

