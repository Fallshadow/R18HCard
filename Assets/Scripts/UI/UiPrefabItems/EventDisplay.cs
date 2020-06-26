using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace act.ui
{
    [RequireComponent(typeof(EventReference))]
    public class EventDisplay : MonoBehaviour, IUiItemLifeInterface
    {
        [Header("Setting")]
        [SerializeField] private Canvas BaseCanvas;
        [SerializeField] private Canvas DescCanvas;
        [SerializeField] private GameObject rimlight;
        //展示位置
        [SerializeField] private bool isSettingBorn = true;
        [SerializeField] private Vector3 settingPos = new Vector3(0.64f, 0.802f, 1.401f);
        [SerializeField] private Vector3 settingRot = new Vector3(0f, 180f, 0f);
        [SerializeField] private Vector3 settingSca = new Vector3(0.315025f, 0.05390611f, 0.3635307f);
        [SerializeField] private UiStaticText wuxiantext = null;
        [SerializeField] private UiStaticText forwardtext = null;
        [SerializeField] private UiStaticText nowtext = null;
        [Space]
        [SerializeField] protected EventReference config = null;
        [SerializeField] private game.EventInst event_inst = null;

        private Animator anim;
        private Animator Anim
        {
            get
            {
                if(anim != null)
                {
                    return anim;
                }
                anim = GetComponentInParent<Animator>();
                return anim;
            }
        }
        
        private void Start()
        {
            BaseCanvas.worldCamera = Camera.main;
            DescCanvas.worldCamera = Camera.main;
            if(isSettingBorn)
            {
                transform.position = settingPos;
                transform.rotation = Quaternion.Euler(settingRot);
                transform.localScale = settingSca;
            }
        }

        public void Init()
        {
            config = GetComponent<EventReference>();

            //事件生命减少、完成、UI关闭 都需要判断是否破坏事件
            evt.EventManager.instance.Register(evt.EventGroup.EVENT, (short)evt.EventEvent.Event_ID_ROUNDNUM_CHANGE, CheckDestoryEventByEventRoundOver);
            evt.EventManager.instance.Register(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_CurEvent_Completed, CheckDestoryEventWithoutAnim);
            evt.EventManager.instance.Register(evt.EventGroup.UI, (short)evt.UiEvent.UI_Event_Desc_Hide, CheckDestoryEventWithoutAnim);

            evt.EventManager.instance.Register(evt.EventGroup.GAME, (short)evt.GameEvent.Limit_TwoOne, showEventID38);

        }

        public void Release()
        {
            //事件生命减少、完成、UI关闭 都需要判断是否破坏事件
            evt.EventManager.instance.Unregister(evt.EventGroup.EVENT, (short)evt.EventEvent.Event_ID_ROUNDNUM_CHANGE, CheckDestoryEventByEventRoundOver);
            evt.EventManager.instance.Unregister(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_CurEvent_Completed, CheckDestoryEventWithoutAnim);
            evt.EventManager.instance.Unregister(evt.EventGroup.UI, (short)evt.UiEvent.UI_Event_Desc_Hide, CheckDestoryEventWithoutAnim);

            evt.EventManager.instance.Unregister(evt.EventGroup.GAME, (short)evt.GameEvent.Limit_TwoOne, showEventID38);
        }
        public game.EventInst GetEventInst()
        {
            return event_inst;
        }

        public void EnterToTable()
        {
            event_inst.EnterToTable();
            Show();
        }
        public void SetInst(game.EventInst eventInst)
        {
            event_inst = eventInst;
            if (eventInst.conditionInsts.Count == 0)
            {
                config.Img_Type.sprite = UiManager.instance.GetSprite($"EventShowType{4}", "PlayCanvas");
            }
            else
            {
                //侦测第一个条件来显示事件卡面
                if (eventInst.conditionInsts[0][0].config.ID == 1)
                {
                    config.Img_Type.sprite = UiManager.instance.GetSprite($"EventShowType{eventInst.conditionInsts[0][0].numVars[0]}", "PlayCanvas");
                }
                else if (eventInst.conditionInsts[0][0].config.ID == 3)
                {
                    config.Img_Type.sprite = UiManager.instance.GetSprite($"EventShowType{game.CardMgr.instance.GetCardDataByID((int)eventInst.conditionInsts[0][0].numVars[0]).type}", "PlayCanvas");
                }
            }

            config.Text_Name.Localize(event_inst.config.name, "ui_system");
            config.Text_Round.text = event_inst.RoundNum.ToString();
            if(event_inst.RoundNum == -2)
            {
                config.Text_Round.text = "∞";
                showEventID38();
            }
        }
        private void showEventID38()
        {
            if(event_inst.config.ID == 38)
            {
                config.Text_Round.text = game.GameFlowMgr.instance.TwoOneNum.ToString();
            }
        }

        public void CheckDestoryEventByEventRoundOver()
        {
            if(event_inst.HasCompleteWaitRoundOver)
            {
                Hide();
                return;
            }
            if(event_inst.RoundNum == 0)
            {
                config.Img_Type.sprite = UiManager.instance.GetSprite($"card_sj_fail", "PlayCanvas");
            }
            if(event_inst.RoundNum == -1)
            {
                if(game.GameFlowMgr.instance.eventDesc == false)
                {
                    Hide();
                }
            }
            config.Text_Round.text = event_inst.RoundNum.ToString();
            if(event_inst.RoundNum == -2)
            {
                wuxiantext.text = "∞";
                config.Text_Round.text = "∞";
                showEventID38();
                ShowWuXian();
            }
            else
            {
                forwardtext.text = (event_inst.RoundNum + 1).ToString();
                nowtext.text = event_inst.RoundNum.ToString();
                ShowNomarl();
            }
        }
        public void CheckDestoryEvent()
        {
            if (event_inst.HasComplete)
            {
                config.Img_Type.sprite = UiManager.instance.GetSprite($"card_sj_success", "PlayCanvas");
                if(game.GameFlowMgr.instance.eventDesc == false)
                {
                    event_inst.HasCompleteWaitRoundOver = true;
                }
            }
            if(event_inst.RoundNum == 0)
            {
                config.Img_Type.sprite = UiManager.instance.GetSprite($"card_sj_fail", "PlayCanvas");
            }
            if(event_inst.RoundNum == -1)
            {
                if(game.GameFlowMgr.instance.eventDesc == false)
                {
                    Hide();
                }
            }
            config.Text_Round.text = event_inst.RoundNum.ToString();
            if(event_inst.RoundNum == -2)
            {
                wuxiantext.text = "∞";
                config.Text_Round.text = "∞";
                showEventID38();
                ShowWuXian();
            }
            else
            {
                forwardtext.text = (event_inst.RoundNum + 1).ToString();
                nowtext.text = event_inst.RoundNum.ToString();
                ShowNomarl();
            }
            
        }
        public void CheckDestoryEventWithoutAnim()
        {
            if(event_inst.HasComplete)
            {
                config.Img_Type.sprite = UiManager.instance.GetSprite($"card_sj_success", "PlayCanvas");
                if(game.GameFlowMgr.instance.eventDesc == false)
                {
                    event_inst.HasCompleteWaitRoundOver = true;
                }
                Hide();
            }
            if(event_inst.RoundNum == 0)
            {
                config.Img_Type.sprite = UiManager.instance.GetSprite($"card_sj_fail", "PlayCanvas");
            }
            if(event_inst.RoundNum == -1)
            {
                if(game.GameFlowMgr.instance.eventDesc == false)
                {
                    Hide();
                }
            }
            config.Text_Round.text = event_inst.RoundNum.ToString();
        }

        #region 表现
        public void Show()
        {
            Anim.Play("Show");
        }
        public void Hide()
        {
            Anim.enabled = true;
            Anim.Play("Hide");
            UiManager.instance.ControlMouseInput(false);
        }

        public void ShowWuXian()
        {
            Anim.Play("TextWuXian");
        }

        public void ShowNomarl()
        {
            Anim.Play("TextNomarl");
        }
        private void OnMouseEnter()
        {
            Debug.Log("自发光");
        }
        //TODO:写在动画最后一帧
        public void DestoryEvent()
        {
            if(event_inst == game.GameFlowMgr.instance.CurEvent)
            {
                game.GameFlowCdtAndEft.instance.CheckCdt(game.GameFlowCdtAndEft.instance.CardNumCheckSuccCEC);
                game.GameFlowMgr.instance.CurEvent.ExcuteResult(game.GameFlowMgr.instance.curEventResults);
            }
            Release();
            event_inst.DestorySelf();
            Destroy(this.gameObject);
            Destroy(this.transform.parent.gameObject);
            UiManager.instance.ControlMouseInput(true);
        }
        #endregion

        private void OnMouseOver()
        {
            if(game.GameFlowMgr.instance.eventDesc)
                return;
            rimlight.SetActive(true);
        }

        private void OnMouseExit()
        {
            rimlight.SetActive(false);
        }

        private void OnMouseDown()
        {
            if(game.GameFlowMgr.instance.eventDesc)
                return;
            evt.EventManager.instance.Send<game.EventInst>(evt.EventGroup.UI, (short)evt.UiEvent.UI_Event_Click,event_inst);
        }

        //private void OnCollisionStay(Collision collision)
        //{
        //    if(collision.transform.tag == "EventPrefab")
        //    {
        //        Anim.enabled = false;
        //        gameObject.transform.position = new Vector3(gameObject.transform.position.x + 1, gameObject.transform.position.y, gameObject.transform.position.z);
        //    }
        //}
        //private void OnCollisionEnter(Collision collision)
        //{
        //    if(collision.transform.tag == "EventPrefab")
        //    {
        //        Anim.enabled = false;
        //        gameObject.transform.position = new Vector3(gameObject.transform.position.x + 1, gameObject.transform.position.y, gameObject.transform.position.z);
        //    }
        //}
        //private void OnCollisionExit(Collision collision)
        //{
        //    if(collision.transform.tag == "EventPrefab")
        //    {
        //        Anim.enabled = false;
        //        gameObject.transform.position = new Vector3(gameObject.transform.position.x + 1, gameObject.transform.position.y, gameObject.transform.position.z);
        //    }
        //}
    }
}

