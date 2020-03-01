﻿using UnityEngine;

namespace act
{
    namespace game
    {
        public class GameController : SingletonMonoBehaviorNoDestroy<GameController>
        {
            public readonly fsm.Fsm<GameController> FSM = new fsm.Fsm<GameController>();

            protected override void Awake()
            {
                base.Awake();
            }

            private void Start()
            {
                debug.PrintSystem.Log("GameControlleRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRR !!!");
                FSM.Initialize(this);
                FSM.AddState((int)fsm.GameFsmState.ENTRY, new fsm.Entry<GameController>());
                FSM.AddState((int)fsm.GameFsmState.MAINMENU, new fsm.MainMenu<GameController>());
                FSM.AddState((int)fsm.GameFsmState.PLAP, new fsm.Play<GameController>());
                FSM.SwitchToState((int)fsm.GameFsmState.ENTRY);
            }

            private void Update()
            {
                // TODO: should be remove after
                //Physics.SyncTransforms();

                FSM.Update();
            }

            private void FixedUpdate()
            {
                FSM.FixedUpdate();
            }

            private void LateUpdate()
            {
                FSM.LateUpdate();
            }

            private void OnApplicationQuit()
            {
                FSM.Finalize();
            }
        }
    }
}