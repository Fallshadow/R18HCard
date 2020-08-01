using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using act.ui;
using TMPro;
using DG.Tweening;

namespace act.game
{
    public class RollTouZiManager : SingletonMonoBehavior<RollTouZiManager>
    {
        public Image reTime;
        public Image Touzi1;
        public Image Touzi2;

        public Image SuccOrDefImg;
        public UiStaticText SuccOrDefText;
        public UiStaticText TextRoll;
        public RectTransform SuccOrDefTextShow;
        public RectTransform towGO;
        public RectTransform oneGO;

        public CanvasGroup succordefCG = null;

        public Touzi[] touzi;
        public List<float> resultList = new List<float>();
        public float touziNum = 0;
        public float maxNum = 0;
        public float addNum = 0;
        public float chengNum = 0;
        public float maxTouziNum = 0;
        CanvasGroup canvasGroup = null;
        CallBack continueCheck = null;
        private void Start()
        {

            evt.EventManager.instance.Register<int,bool>(evt.EventGroup.GAME, (short)evt.GameEvent.ShowOrDefCG, ShowCheckGO);
            evt.EventManager.instance.Register(evt.EventGroup.UI, (short)evt.UiEvent.UI_Event_Desc_Hide, hide);
            canvasGroup = gameObject.GetOrAddComponent<CanvasGroup>();
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
        }
        private void OnDestroy()
        {
            evt.EventManager.instance.Unregister<int, bool>(evt.EventGroup.GAME, (short)evt.GameEvent.ShowOrDefCG, ShowCheckGO);
            evt.EventManager.instance.Unregister(evt.EventGroup.UI, (short)evt.UiEvent.UI_Event_Desc_Hide, hide);
        }
        private void hide()
        {
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
        }

        public void PlayRoll(float touziNum, List<float> resultList, CallBack continueCheck, float maxNum, float addNum, float chengNum, float maxTouZiNum)
        {
            succordefCG.alpha = 0;
            succordefCG.interactable = false;
            TextRoll.GetOrAddComponent<CanvasGroup>().alpha = 0;
            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
            this.resultList = resultList;
            this.touziNum = touziNum;
            this.maxNum = maxNum;
            this.addNum = addNum;
            this.chengNum = chengNum;
            this.maxTouziNum = maxTouZiNum;
            TextRoll.text = $"({maxTouZiNum}+{addNum})*{chengNum}={maxNum}";
            switch(touziNum)
            {
                case 1:
                    ShowOneTouzi();
                    break;
                case 2:
                    ShowTwoTouzi();
                    break;
                default:
                    break;
            }

            this.continueCheck = continueCheck;
            evt.EventManager.instance.Send(evt.EventGroup.TOUZI, (short)evt.TouziEvent.T_Roll);
            //从前的
            //this.touziNum = touziNum;
            //for(int i = 0; i < touziNum; i++)
            //{
            //    touzi[i].gameObject.SetActive(true);
            //   touzi[i].rollTouZi(resultList[i],continueCheck, ResetTouzi);
            //}
            //this.maxNum = maxNum;
        }

        private void ShowOneTouzi()
        {
            Touzi1.gameObject.SetActive(true);
            Touzi2.gameObject.SetActive(false);
            Sprite resultSprite = null;
            switch(maxTouziNum)
            {
                case 1:
                    resultSprite = UiManager.instance.GetSprite($"骰子1", "PlayCanvas");
                    break;
                case 2:
                    resultSprite = UiManager.instance.GetSprite($"骰子2", "PlayCanvas");
                    break;
                case 3:
                    resultSprite = UiManager.instance.GetSprite($"骰子3", "PlayCanvas");
                    break;
                case 4:
                    resultSprite = UiManager.instance.GetSprite($"骰子4", "PlayCanvas");
                    break;
                case 5:
                    resultSprite = UiManager.instance.GetSprite($"骰子5", "PlayCanvas");
                    break;
                case 6:
                    resultSprite = UiManager.instance.GetSprite($"骰子6", "PlayCanvas");
                    break;
                default:
                    break;
            }
            reTime.DOFade(0, 2).OnComplete(() => { 
                Touzi1.DOKill(); Touzi1.sprite = resultSprite;
                continueCheck.Invoke();
                TextRoll.GetOrAddComponent<CanvasGroup>().DOFade(1, 1);
            });
            PingPongImg(Touzi1, 0.6f, 1, 2.5f);
        }

        private void ShowTwoTouzi()
        {
            Touzi1.gameObject.SetActive(true);
            Touzi2.gameObject.SetActive(true);
            Sprite resultSprite1 = null;
            Sprite resultSprite2 = null;

            switch(resultList[0])
            {
                case 1:
                    resultSprite1 = UiManager.instance.GetSprite($"骰子1", "PlayCanvas");
                    break;
                case 2:
                    resultSprite1 = UiManager.instance.GetSprite($"骰子2", "PlayCanvas");
                    break;
                case 3:
                    resultSprite1 = UiManager.instance.GetSprite($"骰子3", "PlayCanvas");
                    break;
                case 4:
                    resultSprite1 = UiManager.instance.GetSprite($"骰子4", "PlayCanvas");
                    break;
                case 5:
                    resultSprite1 = UiManager.instance.GetSprite($"骰子5", "PlayCanvas");
                    break;
                case 6:
                    resultSprite1 = UiManager.instance.GetSprite($"骰子6", "PlayCanvas");
                    break;
                default:
                    break;
            }
            switch(resultList[1])
            {
                case 1:
                    resultSprite2 = UiManager.instance.GetSprite($"骰子1", "PlayCanvas");
                    break;
                case 2:
                    resultSprite2 = UiManager.instance.GetSprite($"骰子2", "PlayCanvas");
                    break;
                case 3:
                    resultSprite2 = UiManager.instance.GetSprite($"骰子3", "PlayCanvas");
                    break;
                case 4:
                    resultSprite2 = UiManager.instance.GetSprite($"骰子4", "PlayCanvas");
                    break;
                case 5:
                    resultSprite2 = UiManager.instance.GetSprite($"骰子5", "PlayCanvas");
                    break;
                case 6:
                    resultSprite2 = UiManager.instance.GetSprite($"骰子6", "PlayCanvas");
                    break;
                default:
                    break;
            }

            reTime.DOFade(0, 2).OnComplete(() => {
                Touzi1.DOKill();
                Touzi1.sprite = resultSprite1;
                Touzi2.DOKill();
                Touzi2.sprite = resultSprite2;
                continueCheck.Invoke();
                TextRoll.GetOrAddComponent<CanvasGroup>().DOFade(1, 1);
            });
            PingPongImg(Touzi1, 0.6f, 1, 2.5f);
            PingPongImg(Touzi2, 0.6f, 1, 2.5f);
        }

        private int NUM;
        private bool SUCC;
        public void ShowCheckGO(int num, bool succ)
        {
            NUM = num;
            SUCC = succ;
            Invoke("ShowCheckGOReal",1);
        }
        public void ShowCheckGOReal()
        {

            succordefCG.alpha = 1;
            succordefCG.interactable = true;
            if(SUCC)
            {
                SuccOrDefImg.sprite = UiManager.instance.GetSprite($"成功", "PlayCanvas");
                SuccOrDefText.text = "检定成功";
            }
            else
            {
                SuccOrDefImg.sprite = UiManager.instance.GetSprite($"失败", "PlayCanvas");
                SuccOrDefText.text = "检定失败";
            }

            if(NUM == 2)
            {
                SuccOrDefTextShow.anchoredPosition = towGO.anchoredPosition;
            }
            else if(NUM == 1)
            {
                SuccOrDefTextShow.anchoredPosition = oneGO.anchoredPosition;
            }
        }


        public void ResetTouzi()
        {

            for(int i = 0; i < touziNum; i++)
            {
                touzi[i].gameObject.SetActive(false);
            }

        }

        private void PingPongImg(Image target, float from, float to, float dur)
        {
            target.DOFade(to, dur).OnComplete(() => { PingPongImg(target,to, from, dur); })
                .OnUpdate(()=> {
                    Touzi1.sprite = GetRandomSprite();
                });
        }

        private Sprite GetRandomSprite()
        {
            int num = UnityEngine.Random.Range(1, 7);
            switch(num)
            {
                case 1:
                    return UiManager.instance.GetSprite($"骰子1", "PlayCanvas");
                case 2:
                    return  UiManager.instance.GetSprite($"骰子2", "PlayCanvas");
                case 3:
                    return  UiManager.instance.GetSprite($"骰子3", "PlayCanvas");
                case 4:
                    return  UiManager.instance.GetSprite($"骰子4", "PlayCanvas");
                case 5:
                    return  UiManager.instance.GetSprite($"骰子5", "PlayCanvas");
                case 6:
                    return  UiManager.instance.GetSprite($"骰子6", "PlayCanvas");
                default:
                    break;
            }
            return null;
        }
    }
}