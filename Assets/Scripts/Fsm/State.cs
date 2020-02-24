using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace act
{
    namespace fsm
    {
        public enum GameFsmState
        {
            ENTRY = 0,
            MAINMENU,
            BATTLE,
        }

        public enum PlayerFsmState
        {
            PFS_IDLE = 0,

        }

        public class State<T>
        {
            protected Fsm<T> m_fsm;

            public void Init(Fsm<T> f)
            {
                m_fsm = f;
                onInit();
            }

            protected virtual void onInit()
            {
            }

            public virtual void Enter() { }
            public virtual void Exit() { }
            public virtual void Update() { }
            public virtual void LateUpdate() { }
            public virtual void FixedUpdate() { }
        }
    }
}


