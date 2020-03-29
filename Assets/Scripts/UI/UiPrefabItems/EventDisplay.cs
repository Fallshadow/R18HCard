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
        [SerializeField] private bool isSettingBorn = true;
        [SerializeField] private Vector3 settingPos = new Vector3(0.64f, 0.802f, 1.401f);
        [SerializeField] private Vector3 settingRot = new Vector3(0f, 180f, 0f);
        [SerializeField] private Vector3 settingSca = new Vector3(0.315025f, 0.05390611f, 0.3635307f);
        [SerializeField] private Vector3 settingShowDescPos = new Vector3(0.64f, 0.802f, 1.401f);
        [Space]
        [SerializeField] protected EventReference config = null;
        [SerializeField] private game.EventInst event_inst = null;
        private Animator Anim
        {
            get
            {
                if(anim != null)
                {
                    return anim;
                }
                return GetComponentInParent<Animator>();
            }
        }
        private Animator anim;
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
        public void Hide()
        {

        }

        public void Init()
        {
            config = GetComponent<EventReference>();
            evt.EventManager.instance.Register(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_IDEvent_ROUNDNUM_CHANGE, CheckDestoryEvent);
            evt.EventManager.instance.Register(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_CurEvent_Completed, CheckDestoryEvent);
            evt.EventManager.instance.Register(evt.EventGroup.UI, (short)evt.UiEvent.UI_Event_Desc_Hide, CheckDestoryEvent);
        }

        public void Release()
        {
            evt.EventManager.instance.Unregister(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_IDEvent_ROUNDNUM_CHANGE, CheckDestoryEvent);
            evt.EventManager.instance.Unregister(evt.EventGroup.GAME, (short)evt.GameEvent.Globe_CurEvent_Completed, CheckDestoryEvent);
            evt.EventManager.instance.Unregister(evt.EventGroup.UI, (short)evt.UiEvent.UI_Event_Desc_Hide, CheckDestoryEvent);
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
            config.Text_Desc.Localize(event_inst.config.desc, "ui_system");
            config.Text_SPDesc.Localize(event_inst.config.desc_SP, "ui_system");
            config.Text_CommonDesc.Localize(event_inst.config.desc_Common, "ui_system");
            config.Text_SuccResultDesc.Localize(event_inst.config.desc_SuccResult, "ui_system");
            config.Text_DefResultDesc.Localize(event_inst.config.desc_DefResult, "ui_system");
            config.Text_Round.text = event_inst.RoundNum.ToString();
            if(event_inst.RoundNum == -2)
            {
                config.Text_Round.text = "无限";
            }
            //int tempCSPCount = event_inst.conditionSpInsts.Count;
            //int tempCCount = event_inst.conditionInsts.Count;

            //for (int index = 0; index < tempCSPCount; index++)
            //{
            //    for (int indexY = 0; indexY < event_inst.conditionSpInsts[index].Count; indexY++)
            //    {
            //        string ss = localization.LocalizationManager.instance.GetLocalizedString(event_inst.conditionSpInsts[index][indexY].desc, "ui_system");
            //        config.Text_Conditions[index].text += ss + " ";
            //    }
            //}
            //for (int index = tempCSPCount; index < tempCSPCount + tempCCount; index++)
            //{
            //    for (int indexY = 0; indexY < event_inst.conditionInsts[index - tempCSPCount].Count; indexY++)
            //    {
            //        string ss = localization.LocalizationManager.instance.GetLocalizedString(event_inst.conditionInsts[index - tempCSPCount][indexY].desc, "ui_system");
            //        config.Text_Conditions[index].text += ss + " ";
            //    }
            //}
            //int tempESPCount = event_inst.effectSpInsts.Count;
            //int tempECount = event_inst.effectInsts.Count;

            //for (int index = 0; index < tempESPCount; index++)
            //{
            //    for (int indexY = 0; indexY < event_inst.effectSpInsts[index].Count; indexY++)
            //    {
            //        string ss = localization.LocalizationManager.instance.GetLocalizedString(event_inst.effectSpInsts[index][indexY].desc, "ui_system");
            //        config.Text_Effects[index].text += ss + " ";
            //    }
            //}
            //for (int index = tempESPCount; index < tempESPCount + tempECount; index++)
            //{
            //    for (int indexY = 0; indexY < event_inst.effectInsts[index - tempESPCount].Count; indexY++)
            //    {
            //        string ss = localization.LocalizationManager.instance.GetLocalizedString(event_inst.effectInsts[index - tempESPCount][indexY].desc, "ui_system");
            //        config.Text_Effects[index].text += ss + " ";
            //    }
            //}
        }

        public void Show()
        {
            Anim.Play("Show");
        }

        public void CheckDestoryEvent()
        {
            if (event_inst.HasComplete || event_inst.RoundNum == 0)
            {
                if(game.GameFlowMgr.instance.eventDesc == false)
                {
                    ShowDie();
                }
            }
            config.Text_Round.text = event_inst.RoundNum.ToString();
            if(event_inst.RoundNum == -2)
            {
                config.Text_Round.text = "无限";
            }
            
        }
        public void ShowDie()
        {
            Anim.enabled = true;
            Anim.Play("Hide");
        }

        //TODO:写在动画最后一帧
        public void DestoryEvent()
        {
            Release();
            event_inst.DestorySelf();
            Destroy(this.gameObject);
        }

        private void OnMouseEnter()
        {
            Debug.Log("自发光");
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

