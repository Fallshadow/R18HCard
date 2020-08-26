using UnityEngine;
using UnityEngine.Playables;
using System.Collections.Generic;
using Cinemachine;

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
            public bool isshowtalkcanvas = true;
            public GameObject[] goesCamera = null;
            public PlayableAsset xinShouEr = null;
            public unlocktest unlocktest = null;
            public readonly fsm.Fsm<GameController> FSM = new fsm.Fsm<GameController>();
            public GameObject mainCamera;
            public GameObject mainCameraTwo;
            public GameObject uiCamera;
            [Header("为了保存游戏要保存的游戏物体状态")]
            public GameObject[] SaveGO = null;
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
            [Header("是否开启事件timeline")]
            public bool isEventTL = true;
            [Header("初始摄像机")]
            public CinemachineVirtualCamera initCamera = null;

            public void initcamera()
            {
                initCamera.gameObject.SetActive(true);
                Invoke("inactivecamera",0.1f);
            }
            private void inactivecamera()
            {
                initCamera.gameObject.SetActive(false);
            }
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

            [Header("事件的位置")]
            public Vector3[] A1Z;
            public Vector3[] A1ZR;
            public Vector3[] A1ZS;
            public Vector3[] Z2B;
            public Vector3[] Z2BR;
            public Vector3[] Z2BS;
            public Vector3[] B3Q;
            public Vector3[] B3QR;
            public Vector3[] B3QS;
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

            public void SetPlayableAsset(TimeLineType timeLineType, TimeLineAssetType timeLineAssetType)
            {
                timelines[(int)timeLineType].playableAsset = timeLineAssets[(int)timeLineAssetType];
            }

            public void Carda1zPos()
            {
                if(GameObject.Find("EventPrefabBase39(Clone)") != null)
                {
                    GameObject.Find("EventPrefabBase39(Clone)").transform.position = A1Z[0];
                    GameObject.Find("EventPrefabBase39(Clone)").transform.eulerAngles = A1ZR[0];
                    GameObject.Find("EventPrefabBase39(Clone)").transform.localScale = A1ZS[0];
                }

                if(GameObject.Find("EventPrefabBase40(Clone)") != null)
                {
                    GameObject.Find("EventPrefabBase40(Clone)").transform.position = A1Z[0];
                    GameObject.Find("EventPrefabBase40(Clone)").transform.eulerAngles = A1ZR[0];
                    GameObject.Find("EventPrefabBase40(Clone)").transform.localScale = A1ZS[0];
                }

                if(GameObject.Find("EventPrefabBase41(Clone)") != null)
                {
                    GameObject.Find("EventPrefabBase41(Clone)").transform.position = A1Z[0];
                    GameObject.Find("EventPrefabBase41(Clone)").transform.eulerAngles = A1ZR[0];
                    GameObject.Find("EventPrefabBase41(Clone)").transform.localScale = A1ZS[0];
                }

                if(GameObject.Find("EventPrefabBase42(Clone)") != null)
                {
                    GameObject.Find("EventPrefabBase42(Clone)").transform.position = A1Z[0];
                    GameObject.Find("EventPrefabBase42(Clone)").transform.eulerAngles = A1ZR[0];
                    GameObject.Find("EventPrefabBase42(Clone)").transform.localScale = A1ZS[0];
                }

                if(GameObject.Find("EventPrefabBase43(Clone)") != null)
                {
                    GameObject.Find("EventPrefabBase43(Clone)").transform.position = A1Z[0];
                    GameObject.Find("EventPrefabBase43(Clone)").transform.eulerAngles = A1ZR[0];
                    GameObject.Find("EventPrefabBase43(Clone)").transform.localScale = A1ZS[0];
                }

                if(GameObject.Find("EventPrefabBase44(Clone)") != null)
                {
                    GameObject.Find("EventPrefabBase44(Clone)").transform.position = A1Z[0];
                    GameObject.Find("EventPrefabBase44(Clone)").transform.eulerAngles = A1ZR[0];
                    GameObject.Find("EventPrefabBase44(Clone)").transform.localScale = A1ZS[0];
                }







                if(GameObject.Find("EventPrefabBase45(Clone)") != null)
                {
                    GameObject.Find("EventPrefabBase45(Clone)").transform.position = A1Z[1];
                    GameObject.Find("EventPrefabBase45(Clone)").transform.eulerAngles = A1ZR[1];
                    GameObject.Find("EventPrefabBase45(Clone)").transform.localScale = A1ZS[1];
                }

                if(GameObject.Find("EventPrefabBase46(Clone)") != null)
                {
                    GameObject.Find("EventPrefabBase46(Clone)").transform.position = A1Z[1];
                    GameObject.Find("EventPrefabBase46(Clone)").transform.eulerAngles = A1ZR[1];
                    GameObject.Find("EventPrefabBase46(Clone)").transform.localScale = A1ZS[1];
                }

                if(GameObject.Find("EventPrefabBase47(Clone)") != null)
                {
                    GameObject.Find("EventPrefabBase47(Clone)").transform.position = A1Z[1];
                    GameObject.Find("EventPrefabBase47(Clone)").transform.eulerAngles = A1ZR[1];
                    GameObject.Find("EventPrefabBase47(Clone)").transform.localScale = A1ZS[1];
                }

                if(GameObject.Find("EventPrefabBase48(Clone)") != null)
                {
                    GameObject.Find("EventPrefabBase48(Clone)").transform.position = A1Z[1];
                    GameObject.Find("EventPrefabBase48(Clone)").transform.eulerAngles = A1ZR[1];
                    GameObject.Find("EventPrefabBase48(Clone)").transform.localScale = A1ZS[1];
                }









                if(GameObject.Find("EventPrefabBase49(Clone)") != null)
                {
                    GameObject.Find("EventPrefabBase49(Clone)").transform.position = A1Z[2];
                    GameObject.Find("EventPrefabBase49(Clone)").transform.eulerAngles = A1ZR[2];
                    GameObject.Find("EventPrefabBase49(Clone)").transform.localScale = A1ZS[2];
                }





                if(GameObject.Find("EventPrefabBase52(Clone)") != null)
                {
                    GameObject.Find("EventPrefabBase52(Clone)").transform.position = A1Z[3];
                    GameObject.Find("EventPrefabBase52(Clone)").transform.eulerAngles = A1ZR[3];
                    GameObject.Find("EventPrefabBase52(Clone)").transform.localScale = A1ZS[3];
                }




                if(GameObject.Find("EventPrefabBase53(Clone)") != null)
                {
                    GameObject.Find("EventPrefabBase53(Clone)").transform.position = A1Z[4];
                    GameObject.Find("EventPrefabBase53(Clone)").transform.eulerAngles = A1ZR[4];
                    GameObject.Find("EventPrefabBase53(Clone)").transform.localScale = A1ZS[4];
                }

                if(GameObject.Find("EventPrefabBase54(Clone)") != null)
                {
                    GameObject.Find("EventPrefabBase54(Clone)").transform.position = A1Z[4];
                    GameObject.Find("EventPrefabBase54(Clone)").transform.eulerAngles = A1ZR[4];
                    GameObject.Find("EventPrefabBase54(Clone)").transform.localScale = A1ZS[4];
                }

                if(GameObject.Find("EventPrefabBase55(Clone)") != null)
                {
                    GameObject.Find("EventPrefabBase55(Clone)").transform.position = A1Z[4];
                    GameObject.Find("EventPrefabBase55(Clone)").transform.eulerAngles = A1ZR[4];
                    GameObject.Find("EventPrefabBase55(Clone)").transform.localScale = A1ZS[4];
                }

                if(GameObject.Find("EventPrefabBase56(Clone)") != null)
                {
                    GameObject.Find("EventPrefabBase56(Clone)").transform.position = A1Z[4];
                    GameObject.Find("EventPrefabBase56(Clone)").transform.eulerAngles = A1ZR[4];
                    GameObject.Find("EventPrefabBase56(Clone)").transform.localScale = A1ZS[4];
                }
                if(GameObject.Find("EventPrefabBase62(Clone)") != null)
                {
                    GameObject.Find("EventPrefabBase62(Clone)").transform.position = A1Z[4];
                    GameObject.Find("EventPrefabBase62(Clone)").transform.eulerAngles = A1ZR[4];
                    GameObject.Find("EventPrefabBase62(Clone)").transform.localScale = A1ZS[4];
                }






                if(GameObject.Find("EventPrefabBase57(Clone)") != null)
                {
                    GameObject.Find("EventPrefabBase57(Clone)").transform.position = A1Z[5];
                    GameObject.Find("EventPrefabBase57(Clone)").transform.eulerAngles = A1ZR[5];
                    GameObject.Find("EventPrefabBase57(Clone)").transform.localScale = A1ZS[5];
                }
            }

            public void Cardz2bPos()
            {
                if(GameObject.Find("EventPrefabBase39(Clone)") != null)
                {
                    GameObject.Find("EventPrefabBase39(Clone)").transform.position = Z2B[0];
                    GameObject.Find("EventPrefabBase39(Clone)").transform.eulerAngles = Z2BR[0];
                    GameObject.Find("EventPrefabBase39(Clone)").transform.localScale = Z2BS[0];
                }

                if(GameObject.Find("EventPrefabBase40(Clone)") != null)
                {
                    GameObject.Find("EventPrefabBase40(Clone)").transform.position = Z2B[0];
                    GameObject.Find("EventPrefabBase40(Clone)").transform.eulerAngles = Z2BR[0];
                    GameObject.Find("EventPrefabBase40(Clone)").transform.localScale = Z2BS[0];
                }

                if(GameObject.Find("EventPrefabBase41(Clone)") != null)
                {
                    GameObject.Find("EventPrefabBase41(Clone)").transform.position = Z2B[0];
                    GameObject.Find("EventPrefabBase41(Clone)").transform.eulerAngles = Z2BR[0];
                    GameObject.Find("EventPrefabBase41(Clone)").transform.localScale = Z2BS[0];

                }

                if(GameObject.Find("EventPrefabBase42(Clone)") != null)
                {
                    GameObject.Find("EventPrefabBase42(Clone)").transform.position = Z2B[0];
                    GameObject.Find("EventPrefabBase42(Clone)").transform.eulerAngles = Z2BR[0];
                    GameObject.Find("EventPrefabBase42(Clone)").transform.localScale = Z2BS[0];

                }

                if(GameObject.Find("EventPrefabBase43(Clone)") != null)
                {
                    GameObject.Find("EventPrefabBase43(Clone)").transform.position = Z2B[0];
                    GameObject.Find("EventPrefabBase43(Clone)").transform.eulerAngles = Z2BR[0];
                    GameObject.Find("EventPrefabBase43(Clone)").transform.localScale = Z2BS[0];

                }

                if(GameObject.Find("EventPrefabBase44(Clone)") != null)
                {
                    GameObject.Find("EventPrefabBase44(Clone)").transform.position = Z2B[0];
                    GameObject.Find("EventPrefabBase44(Clone)").transform.eulerAngles = Z2BR[0];
                    GameObject.Find("EventPrefabBase44(Clone)").transform.localScale = Z2BS[0];

                }







                if(GameObject.Find("EventPrefabBase45(Clone)") != null)
                {
                    GameObject.Find("EventPrefabBase45(Clone)").transform.position = Z2B[1];
                    GameObject.Find("EventPrefabBase45(Clone)").transform.eulerAngles = Z2BR[1];
                    GameObject.Find("EventPrefabBase45(Clone)").transform.localScale = Z2BS[1];

                }

                if(GameObject.Find("EventPrefabBase46(Clone)") != null)
                {
                    GameObject.Find("EventPrefabBase46(Clone)").transform.position = Z2B[1];
                    GameObject.Find("EventPrefabBase46(Clone)").transform.eulerAngles = Z2BR[1];
                    GameObject.Find("EventPrefabBase46(Clone)").transform.localScale = Z2BS[1];

                }

                if(GameObject.Find("EventPrefabBase47(Clone)") != null)
                {
                    GameObject.Find("EventPrefabBase47(Clone)").transform.position = Z2B[1];
                    GameObject.Find("EventPrefabBase47(Clone)").transform.eulerAngles = Z2BR[1];
                    GameObject.Find("EventPrefabBase47(Clone)").transform.localScale = Z2BS[1];

                }

                if(GameObject.Find("EventPrefabBase48(Clone)") != null)
                {
                    GameObject.Find("EventPrefabBase48(Clone)").transform.position = Z2B[1];
                    GameObject.Find("EventPrefabBase48(Clone)").transform.eulerAngles = Z2BR[1];
                    GameObject.Find("EventPrefabBase48(Clone)").transform.localScale = Z2BS[1];
                }









                if(GameObject.Find("EventPrefabBase49(Clone)") != null)
                {
                    GameObject.Find("EventPrefabBase49(Clone)").transform.position = Z2B[2];
                    GameObject.Find("EventPrefabBase49(Clone)").transform.eulerAngles = Z2BR[2];
                    GameObject.Find("EventPrefabBase49(Clone)").transform.localScale = Z2BS[2];
                }





                if(GameObject.Find("EventPrefabBase52(Clone)") != null)
                {
                    GameObject.Find("EventPrefabBase52(Clone)").transform.position = Z2B[3];
                    GameObject.Find("EventPrefabBase52(Clone)").transform.eulerAngles = Z2BR[3];
                    GameObject.Find("EventPrefabBase52(Clone)").transform.localScale = Z2BS[3];
                }




                if(GameObject.Find("EventPrefabBase53(Clone)") != null)
                {
                    GameObject.Find("EventPrefabBase53(Clone)").transform.position = Z2B[4];
                    GameObject.Find("EventPrefabBase53(Clone)").transform.eulerAngles = Z2BR[4];
                    GameObject.Find("EventPrefabBase53(Clone)").transform.localScale = Z2BS[4];

                }

                if(GameObject.Find("EventPrefabBase54(Clone)") != null)
                {
                    GameObject.Find("EventPrefabBase54(Clone)").transform.position = Z2B[4];
                    GameObject.Find("EventPrefabBase54(Clone)").transform.eulerAngles = Z2BR[4];
                    GameObject.Find("EventPrefabBase54(Clone)").transform.localScale = Z2BS[4];

                }

                if(GameObject.Find("EventPrefabBase55(Clone)") != null)
                {
                    GameObject.Find("EventPrefabBase55(Clone)").transform.position = Z2B[4];
                    GameObject.Find("EventPrefabBase55(Clone)").transform.eulerAngles = Z2BR[4];
                    GameObject.Find("EventPrefabBase55(Clone)").transform.localScale = Z2BS[4];

                }

                if(GameObject.Find("EventPrefabBase56(Clone)") != null)
                {
                    GameObject.Find("EventPrefabBase56(Clone)").transform.position = Z2B[4];
                    GameObject.Find("EventPrefabBase56(Clone)").transform.eulerAngles = Z2BR[4];
                    GameObject.Find("EventPrefabBase56(Clone)").transform.localScale = Z2BS[4];

                }
                if(GameObject.Find("EventPrefabBase62(Clone)") != null)
                {
                    GameObject.Find("EventPrefabBase62(Clone)").transform.position = Z2B[4];
                    GameObject.Find("EventPrefabBase62(Clone)").transform.eulerAngles = Z2BR[4];
                    GameObject.Find("EventPrefabBase62(Clone)").transform.localScale = Z2BS[4];
                }







                if(GameObject.Find("EventPrefabBase57(Clone)") != null)
                {
                    GameObject.Find("EventPrefabBase57(Clone)").transform.position = Z2B[5];
                    GameObject.Find("EventPrefabBase57(Clone)").transform.eulerAngles = Z2BR[5];
                    GameObject.Find("EventPrefabBase57(Clone)").transform.localScale = Z2BS[5];
                }






                if(GameObject.Find("EventPrefabBase58(Clone)") != null)
                {
                    GameObject.Find("EventPrefabBase58(Clone)").transform.position = Z2B[6];
                    GameObject.Find("EventPrefabBase58(Clone)").transform.eulerAngles = Z2BR[6];
                    GameObject.Find("EventPrefabBase58(Clone)").transform.localScale = Z2BS[6];
                }





                if(GameObject.Find("EventPrefabBase59(Clone)") != null)
                {
                    GameObject.Find("EventPrefabBase59(Clone)").transform.position = Z2B[7];
                    GameObject.Find("EventPrefabBase59(Clone)").transform.eulerAngles = Z2BR[7];
                    GameObject.Find("EventPrefabBase59(Clone)").transform.localScale = Z2BS[7];
                }
            }

            public void Cardb3qPos()
            {
                if(GameObject.Find("EventPrefabBase39(Clone)") != null)
                {
                    GameObject.Find("EventPrefabBase39(Clone)").transform.position = B3Q[0];
                    GameObject.Find("EventPrefabBase39(Clone)").transform.eulerAngles = B3QR[0];
                    GameObject.Find("EventPrefabBase39(Clone)").transform.localScale = B3QS[0];
                }

                if(GameObject.Find("EventPrefabBase40(Clone)") != null)
                {
                    GameObject.Find("EventPrefabBase40(Clone)").transform.position = B3Q[0];
                    GameObject.Find("EventPrefabBase40(Clone)").transform.eulerAngles = B3QR[0];
                    GameObject.Find("EventPrefabBase40(Clone)").transform.localScale = B3QS[0];
                }

                if(GameObject.Find("EventPrefabBase41(Clone)") != null)
                {
                    GameObject.Find("EventPrefabBase41(Clone)").transform.position = B3Q[0];
                    GameObject.Find("EventPrefabBase41(Clone)").transform.eulerAngles = B3QR[0];
                    GameObject.Find("EventPrefabBase41(Clone)").transform.localScale = B3QS[0];

                }

                if(GameObject.Find("EventPrefabBase42(Clone)") != null)
                {
                    GameObject.Find("EventPrefabBase42(Clone)").transform.position = B3Q[0];
                    GameObject.Find("EventPrefabBase42(Clone)").transform.eulerAngles = B3QR[0];
                    GameObject.Find("EventPrefabBase42(Clone)").transform.localScale = B3QS[0];

                }

                if(GameObject.Find("EventPrefabBase43(Clone)") != null)
                {
                    GameObject.Find("EventPrefabBase43(Clone)").transform.position = B3Q[0];
                    GameObject.Find("EventPrefabBase43(Clone)").transform.eulerAngles = B3QR[0];
                    GameObject.Find("EventPrefabBase43(Clone)").transform.localScale = B3QS[0];

                }

                if(GameObject.Find("EventPrefabBase44(Clone)") != null)
                {
                    GameObject.Find("EventPrefabBase44(Clone)").transform.position = B3Q[0];
                    GameObject.Find("EventPrefabBase44(Clone)").transform.eulerAngles = B3QR[0];
                    GameObject.Find("EventPrefabBase44(Clone)").transform.localScale = B3QS[0];

                }







                if(GameObject.Find("EventPrefabBase45(Clone)") != null)
                {
                    GameObject.Find("EventPrefabBase45(Clone)").transform.position = B3Q[1];
                    GameObject.Find("EventPrefabBase45(Clone)").transform.eulerAngles = B3QR[1];
                    GameObject.Find("EventPrefabBase45(Clone)").transform.localScale = B3QS[1];

                }

                if(GameObject.Find("EventPrefabBase46(Clone)") != null)
                {
                    GameObject.Find("EventPrefabBase46(Clone)").transform.position = B3Q[1];
                    GameObject.Find("EventPrefabBase46(Clone)").transform.eulerAngles = B3QR[1];
                    GameObject.Find("EventPrefabBase46(Clone)").transform.localScale = B3QS[1];

                }

                if(GameObject.Find("EventPrefabBase47(Clone)") != null)
                {
                    GameObject.Find("EventPrefabBase47(Clone)").transform.position = B3Q[1];
                    GameObject.Find("EventPrefabBase47(Clone)").transform.eulerAngles = B3QR[1];
                    GameObject.Find("EventPrefabBase47(Clone)").transform.localScale = B3QS[1];

                }

                if(GameObject.Find("EventPrefabBase48(Clone)") != null)
                {
                    GameObject.Find("EventPrefabBase48(Clone)").transform.position = B3Q[1];
                    GameObject.Find("EventPrefabBase48(Clone)").transform.eulerAngles = B3QR[1];
                    GameObject.Find("EventPrefabBase48(Clone)").transform.localScale = B3QS[1];
                }









                if(GameObject.Find("EventPrefabBase49(Clone)") != null)
                {
                    GameObject.Find("EventPrefabBase49(Clone)").transform.position = B3Q[2];
                    GameObject.Find("EventPrefabBase49(Clone)").transform.eulerAngles = B3QR[2];
                    GameObject.Find("EventPrefabBase49(Clone)").transform.localScale = B3QS[2];
                }





                if(GameObject.Find("EventPrefabBase52(Clone)") != null)
                {
                    GameObject.Find("EventPrefabBase52(Clone)").transform.position = B3Q[3];
                    GameObject.Find("EventPrefabBase52(Clone)").transform.eulerAngles = B3QR[3];
                    GameObject.Find("EventPrefabBase52(Clone)").transform.localScale = B3QS[3];
                }




                if(GameObject.Find("EventPrefabBase53(Clone)") != null)
                {
                    GameObject.Find("EventPrefabBase53(Clone)").transform.position = B3Q[4];
                    GameObject.Find("EventPrefabBase53(Clone)").transform.eulerAngles = B3QR[4];
                    GameObject.Find("EventPrefabBase53(Clone)").transform.localScale = B3QS[4];

                }

                if(GameObject.Find("EventPrefabBase54(Clone)") != null)
                {
                    GameObject.Find("EventPrefabBase54(Clone)").transform.position = B3Q[4];
                    GameObject.Find("EventPrefabBase54(Clone)").transform.eulerAngles = B3QR[4];
                    GameObject.Find("EventPrefabBase54(Clone)").transform.localScale = B3QS[4];

                }

                if(GameObject.Find("EventPrefabBase55(Clone)") != null)
                {
                    GameObject.Find("EventPrefabBase55(Clone)").transform.position = B3Q[4];
                    GameObject.Find("EventPrefabBase55(Clone)").transform.eulerAngles = B3QR[4];
                    GameObject.Find("EventPrefabBase55(Clone)").transform.localScale = B3QS[4];

                }

                if(GameObject.Find("EventPrefabBase56(Clone)") != null)
                {
                    GameObject.Find("EventPrefabBase56(Clone)").transform.position = B3Q[4];
                    GameObject.Find("EventPrefabBase56(Clone)").transform.eulerAngles = B3QR[4];
                    GameObject.Find("EventPrefabBase56(Clone)").transform.localScale = B3QS[4];

                }
                if(GameObject.Find("EventPrefabBase62(Clone)") != null)
                {
                    GameObject.Find("EventPrefabBase62(Clone)").transform.position = B3Q[4];
                    GameObject.Find("EventPrefabBase62(Clone)").transform.eulerAngles = B3QR[4];
                    GameObject.Find("EventPrefabBase62(Clone)").transform.localScale = B3QS[4];
                }






                if(GameObject.Find("EventPrefabBase57(Clone)") != null)
                {
                    GameObject.Find("EventPrefabBase57(Clone)").transform.position = B3Q[5];
                    GameObject.Find("EventPrefabBase57(Clone)").transform.eulerAngles = B3QR[5];
                    GameObject.Find("EventPrefabBase57(Clone)").transform.localScale = B3QS[5];
                }







                if(GameObject.Find("EventPrefabBase59(Clone)") != null)
                {
                    GameObject.Find("EventPrefabBase59(Clone)").transform.position = B3Q[6];
                    GameObject.Find("EventPrefabBase59(Clone)").transform.eulerAngles = B3QR[6];
                    GameObject.Find("EventPrefabBase59(Clone)").transform.localScale = B3QS[6];
                }
                
                
                
                
                if(GameObject.Find("EventPrefabBase60(Clone)") != null)
                {
                    GameObject.Find("EventPrefabBase60(Clone)").transform.position = B3Q[7];
                    GameObject.Find("EventPrefabBase60(Clone)").transform.eulerAngles = B3QR[7];
                    GameObject.Find("EventPrefabBase60(Clone)").transform.localScale = B3QS[7];
                }  
                
                
                
                
                if(GameObject.Find("EventPrefabBase61(Clone)") != null)
                {
                    GameObject.Find("EventPrefabBase61(Clone)").transform.position = B3Q[8];
                    GameObject.Find("EventPrefabBase61(Clone)").transform.eulerAngles = B3QR[8];
                    GameObject.Find("EventPrefabBase61(Clone)").transform.localScale = B3QS[8];
                }
            }
            

            public bool[] SaveGOpos()
            {
                bool[] activeGOs = new bool[20];
                for(int i = 0; i < SaveGO.Length; i++)
                {
                    if(SaveGO[i] != null)
                    {
                        activeGOs[i] = SaveGO[i].activeSelf;
                    }
                    else
                    {
                        activeGOs[i] = false;
                    }
                }
                return activeGOs;
            }

            public void LoadGOpos(bool[] activeGOs)
            {
                for(int i = 0; i < activeGOs.Length; i++)
                {
                    if(SaveGO[i] != null)
                    {
                        SaveGO[i].SetActive(activeGOs[i]);
                    }
                }
            }

            public void LoadEventPos(data.SaveData saveData)
            {
                if(!saveData.ThrPlea0 || !saveData.ThrVit0)
                {
                    Cardb3qPos();
                }
                else if(!saveData.SecondVit0 || !saveData.SecondPlea0)
                {
                    Cardz2bPos();
                }
                else if(!saveData.FirstVit0 || !saveData.FirstPlea0)
                {
                    Carda1zPos();
                }
            }

            public void LoadEventPos()
            {
                if(!GameFlowMgr.instance.ThrPlea0 || !GameFlowMgr.instance.ThrVit0)
                {
                    Cardb3qPos();
                }
                else if(!GameFlowMgr.instance.SecondVit0 || !GameFlowMgr.instance.SecondPlea0)
                {
                    Cardz2bPos();
                }
                else if(!GameFlowMgr.instance.FirstVit0 || !GameFlowMgr.instance.FirstPlea0)
                {
                    Carda1zPos();
                }
            }
        }
    }
}