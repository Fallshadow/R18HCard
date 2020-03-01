using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace act
{
    namespace fsm
    {
        public class Fsm<T>
        {
            Machine<T> m_machine;
            T m_owner;
            Dictionary<int, State<T>> m_stateMap = new Dictionary<int, State<T>>();

            public T OWNER
            {
                get { return m_owner; }
            }

            public void Initialize(T owner)
            {
                m_owner = owner;
                m_machine = new Machine<T>();
            }

            public void Finalize()
            {
                m_stateMap.Clear();
            }

            public void Update()
            {
                State<T> s = m_machine.GetCurrentState();
                if (s != null)
                {
                    s.Update();
                }
            }

            public void FixedUpdate()
            {
                State<T> s = m_machine.GetCurrentState();
                if (s != null)
                {
                    s.FixedUpdate();
                }
            }

            public void LateUpdate()
            {
                State<T> s = m_machine.GetCurrentState();
                if (s != null)
                {
                    s.LateUpdate();
                }
            }

            public State<T> GetCurrentState()
            {
                return m_machine.GetCurrentState();
            }

            public void AddState(int stateEnum, State<T> state)
            {
                state.Init(this);
                if (!m_stateMap.ContainsKey(stateEnum))
                {
                    m_stateMap[stateEnum] = state;
                }
            }

            public State<T> GetState(int stateEnum)
            {
                State<T> s = null;
                m_stateMap.TryGetValue(stateEnum, out s);
                return s;
            }

            public int GetCurrentStateEnum()
            {
                return GetStateEnum(GetCurrentState());
            }

            public int GetStateEnum(State<T> s)
            {
                if (m_stateMap.ContainsValue(s))
                {
                    Dictionary<int, State<T>>.Enumerator iter = m_stateMap.GetEnumerator();
                    while (iter.MoveNext())
                    {
                        State<T> state = iter.Current.Value;
                        if (state == s)
                        {
                            return iter.Current.Key;
                        }
                    }
                }
                return -1;
            }

            public void SwitchToState(int stateEnum)
            {
                State<T> s = GetState(stateEnum);
                if (s != m_machine.GetCurrentState())
                {
                    m_machine.SwitchToState(s);
                }
            }

            public void SwitchToState(State<T> s)
            {
                if (s != m_machine.GetCurrentState())
                {
                    m_machine.SwitchToState(s);
                }
            }

            public State<T> SwitchToPreviousState()
            {
                State<T> s = m_machine.GetPreviousState();
                if (s != m_machine.GetCurrentState())
                {
                    m_machine.SwitchToPreviousState();
                }
                return s;
            }
        }
    }
}


