using UnityEngine;
using UnityEngine.Playables;
using System.Collections.Generic;

namespace act
{
    namespace game
    {
        public enum ModelType
        {
            Common = 0,
            YuJinBody,
            AiFu,
            ZhengMianWei,
            BeiMianWei,
            QiChengWei,
        }
        public enum TimeLineType
        {
            ZuJiaoHard = 0,
            AiFu = 1,
            ZhengMian,
            BeiMian,
            QiCheng,
        }
        public enum TimeLineAssetType
        {
            ZuJiaoHard = 0,
            AiFuSucc,
            AiFuDef,
            ZhengMianSucc,
            ZhengMianDef,
            BeiMianSucc,
            BeiMianDef,
            QiChengSucc,
            QiChengDef,
            AiFu0,
            ZhengMian0,
            BeiMian0,
            QiCheng0,
        }


        public class GameController : SingletonMonoBehaviorNoDestroy<GameController>
        {
            public PlayableAsset xinShouEr = null;

            public readonly fsm.Fsm<GameController> FSM = new fsm.Fsm<GameController>();
            public GameObject mainCamera;
            public GameObject mainCameraTwo;
            public GameObject uiCamera;

            [Header("模型们")]
            public GameObject[] models = null;
            public Animator[] modelsAnimtor = null;
            [Header("Timeline们")]
            public PlayableDirector[] timelines = null;
            [Header("TimelineAssest们")]
            public PlayableAsset[] timeLineAssets = null;
            [Header("是否开启新手教程")]
            public bool isInNewPlayFlow = false;
            public bool isInNewPlayFlow2 = false;
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
                FSM.AddState((int)fsm.GameFsmState.GameFlowRoundStart, new fsm.GameFlowRoundStart<GameController>());
                FSM.AddState((int)fsm.GameFsmState.GameFlowWaitForCheck, new fsm.GameFlowWaitForCheck<GameController>());
                FSM.AddState((int)fsm.GameFsmState.GameFlowCardCheck, new fsm.GameFlowCardCheck<GameController>());
                FSM.AddState((int)fsm.GameFsmState.GameFlowCardNumCheck, new fsm.GameFlowCardNumCheck<GameController>());
                FSM.AddState((int)fsm.GameFsmState.GameFlowCardUseOver, new fsm.GameFlowCardUseOver<GameController>());
                FSM.AddState((int)fsm.GameFsmState.GameFlowRoundEnd, new fsm.GameFlowRoundEnd<GameController>());
                FSM.AddState((int)fsm.GameFsmState.NewPlayerFlow, new fsm.NewPlayerFlow<GameController>());
                FSM.AddState((int)fsm.GameFsmState.GameOver, new fsm.GameFlowGameOver<GameController>());
                FSM.SwitchToState((int)fsm.GameFsmState.ENTRY);
                modelsAnimtor = new Animator[models.Length];
                for(int i = 0; i < models.Length; i++)
                {
                    modelsAnimtor[i] = models[i].GetComponent<Animator>();
                }


            }
            [Header("这个是时间计时器")]
            public float timer = 0;
            public float timerDel = 5;
            public bool useTimerNorModel = true;
            [Header("洛璃TimelineAssest们")]
            public PlayableAsset[] loliTimeLineAssets = null;
            public PlayableDirector loliTimeline = null;

            [Header("洛璃浴巾TimelineAssest们")]
            public bool YUJINuseTimerNorModel = true;
            public PlayableAsset[] loliYUJINTimeLineAssets = null;
            public PlayableDirector loliYUJINTimeline = null;
            private void Update()
            {
                // TODO: should be remove after
                //Physics.SyncTransforms();

                FSM.Update();

                if(models[0].gameObject.activeSelf && useTimerNorModel)
                {
                    timer += Time.deltaTime;
                    if(timer > timerDel)
                    {
                        timer = 0;
                        int random = UnityEngine.Random.Range(0, 3);
                        string animName = "";
                        switch(random)
                        {
                            case 0:
                                animName = "坐姿呼吸";
                                break;
                            case 1:
                                animName = "晃动";
                                break;
                            case 2:
                                animName = "俯身";
                                break;
                            default:
                                break;
                        }
                        Debug.Log(random + animName);
                        loliTimeline.playableAsset = loliTimeLineAssets[random];
                        loliTimeline.Play();
                    }
                }
                else if(models[1].gameObject.activeSelf && YUJINuseTimerNorModel)
                {
                    timer += Time.deltaTime;
                    if(timer > timerDel)
                    {
                        timer = 0;
                        int random = UnityEngine.Random.Range(0, 3);
                        loliYUJINTimeline.playableAsset = loliYUJINTimeLineAssets[random];
                        loliYUJINTimeline.Play();
                    }
                }
                else
                {
                    timer = 0;
                }
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

            public void PlayActivePlayableAsset(TimeLineType timeLineType, TimeLineAssetType timeLineAssetType, DirectorWrapMode directorWrapMode = DirectorWrapMode.None)
            {
                timelines[(int)timeLineType].playableAsset = timeLineAssets[(int)timeLineAssetType];
                timelines[(int)timeLineType].extrapolationMode = directorWrapMode;
                timelines[(int)timeLineType].Play();
            }
        }
    }
}