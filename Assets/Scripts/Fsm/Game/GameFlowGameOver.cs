using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace act.fsm
{

    public class GameFlowGameOver<T> : State<T>
    {
        public override void Enter()
        {
            act.ui.UiManager.instance.SetUIAlpha(ui.UiManager.instance.CreateUi<ui.PlayCanvas>(), 0, time: 1);
            ui.UiManager.instance.CreateUi<ui.GameOverCanvas>().Show();
            Debug.Log("进入状态：等待主要阶段开启（等待操作卡牌）");
        }

        public override void Exit()
        {
            act.ui.UiManager.instance.SetUIAlpha(ui.UiManager.instance.CreateUi<ui.PlayCanvas>(), 1, time: 0);
        }

        public override void Update()
        {
        }
    }
}