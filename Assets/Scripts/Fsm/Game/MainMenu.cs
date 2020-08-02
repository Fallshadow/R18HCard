﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace act.fsm
{
    public class MainMenu<T> : State<T>
    {
        public override void Enter()
        {

            GameObject.Destroy(ui.UiManager.instance.CreateUi<ui.GameOverCanvas>().gameObject)
            ;

            AudioMgr.instance.PauseEnvirMusic();
            ui.UiManager.instance.CreateUi<ui.MainMenuCanvas>();
            ui.UiManager.instance.OpenUi<ui.MainMenuCanvas>();

            act.game.GameController.instance.mainCamera.SetActive(false);
            act.game.GameController.instance.mainCameraTwo.SetActive(false);
            act.game.GameController.instance.uiCamera.GetComponent<PostProcessVolume>().enabled = false;
            act.game.GameController.instance.uiCamera.GetComponent<PostProcessLayer>().enabled = false;
        }

        public override void Exit()
        {

        }

        public override void Update()
        {
        }
    }
}

