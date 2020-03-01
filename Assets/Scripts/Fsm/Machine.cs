using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace act
{
    namespace fsm
    {
        public class Machine<T>
        {
            State<T> m_currentState = null;
            State<T> m_previousState = null;

            public State<T> GetCurrentState()
            {
                return m_currentState;
            }

            public State<T> GetPreviousState()
            {
                return m_previousState;
            }

            public void SwitchToState(State<T> newState)
            {
                if (m_currentState != null)
                {
                    m_previousState = m_currentState;
                    m_currentState.Exit();
                }

                if (newState != null)
                {
                    m_currentState = newState;
                    m_currentState.Enter();
                }
            }

            public void SwitchToPreviousState()
            {
                if (m_previousState != null)
                {
                    SwitchToState(m_previousState);
                }
            }
        }

    }
}


