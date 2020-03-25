﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace act.fsm
{

    public class GameFlowWaitForCheck<T> : State<T>
    {
        public override void Enter()
        {
            Debug.Log("进入状态：等待主要阶段开启（等待操作卡牌）");
        }

        public override void Exit()
        {
        }

        public override void Update()
        {
        }
    }
}