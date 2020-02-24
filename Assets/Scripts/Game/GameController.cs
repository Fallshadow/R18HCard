using UnityEngine;

namespace act
{
    namespace game
    {
        public class GameController : SingletonMonoBehaviorNoDestroy<GameController>
        {
            public readonly fsm.Fsm<GameController> FSM = new fsm.Fsm<GameController>();

#if IGG_DEBUG
            private GUIStyle style = new GUIStyle();
            //顯示據點人數
            public bool ShowUserRole = false;
#endif

            protected override void Awake()
            {
                base.Awake();
                initEnv();

#if IGG_DEBUG
                style.fontSize = 18;
                style.normal.textColor = Color.white;
                style.alignment = TextAnchor.UpperRight;
#endif
            }

            private void Start()
            {
                debug.PrintSystem.Log("GameControlleRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRR !!!");
                FSM.Initialize(this);
                FSM.AddState((int)fsm.GameFsmState.ENTRY, new fsm.Entry<GameController>());
                FSM.AddState((int)fsm.GameFsmState.MAINMENU, new fsm.MainMenu<GameController>());
                FSM.AddState((int)fsm.GameFsmState.BATTLE, new fsm.Battle<GameController>());
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

            private void initEnv()
            {
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
                 // set the resolution based on iphone 8's resolution
                float aspectRatio = (float)Screen.width / (float)Screen.height;
                int h = 750;
                int w = (int)((float)h * aspectRatio);
                Screen.SetResolution(w, h, true);
                debug.PrintSystem.Log("SetResolution = (" + w + ", " + h + ")");

                Application.targetFrameRate = 30;
#else
                Application.targetFrameRate = 60;
#endif
            }
        }
    }
}