using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public enum GuideType
{
    Event1 = 0,
    CardTypeCon1,
    CardCon,
    CardSalt,
    CardCheckNum,
    OverRound,
    EventYear,
    Cost,
    qifen,
    Body,
    CheckBtn,
    Exit,
    CardCheckBtn,
}

public enum CheckType
{
    None = -1,
    OnlyMyBtn =0,
    UseYouBtn = 1,
}
public enum BtnControlType
{
    None = -1,
    ExitBtn = 0,
    RoundOverBtn = 1,
    StartCheckBtn = 2,
}

public class GuideController : SingletonMonoBehavior<GuideController>
{
    public GameObject[] texts;
    public GameObject[] images;
    public GameObject guideBG;
    public GuideEvent guideEvent;
    public CanvasGroup playCanvasGroup;
    public Button[] buttons;

    private List<Vector3> imageVecs = new List<Vector3>();
    private void Start()
    {
        StartGuide();
        foreach(var item in images)
        {
            item.SetActive(false);
            imageVecs.Add(item.transform.localScale);
        }
        foreach(var item in texts)
        {
            if(item == null)
            {
                continue;
            }
            item.SetActive(false);
            item.GetComponent<CanvasGroup>().alpha = 0;
        }
        guideBG.SetActive(false);
    }

    public void Guide(GuideType guideType)
    {

        playCanvasGroup.interactable = false;

        foreach(var item in images)
        {
            item.SetActive(false);
        }

        guideBG.SetActive(true);
        images[(int)guideType].GetComponent<Image>().raycastTarget = true;
        images[(int)guideType].SetActive(true);
        images[(int)guideType].transform.localScale = new Vector3(25, 25, 25);
        images[(int)guideType].GetComponent<Button>().enabled = false;
        images[(int)guideType].transform.DOScale(imageVecs[(int)guideType], 1.5f).OnComplete(() => {
            images[(int)guideType].GetComponent<Button>().enabled = true;
            playCanvasGroup.interactable = true;
        });


        foreach(var item in texts)
        {
            if(item == null)
            {
                continue;
            }
            item.SetActive(false);
            item.GetComponent<CanvasGroup>().alpha = 0;
        }
        if(texts[(int)guideType] != null)
        {
            texts[(int)guideType].SetActive(true);
            texts[(int)guideType].GetComponent<CanvasGroup>().DOFade(1, 1.5f);
        }
        images[(int)guideType].GetOrAddComponent<Button>().onClick.RemoveAllListeners();
        images[(int)guideType].GetOrAddComponent<Button>().onClick.AddListener(() =>
        {
            if(guideType == GuideType.Event1)
            {
                act.evt.EventManager.instance.Send<int>(act.evt.EventGroup.GAME, (short)act.evt.GameEvent.ShowDescByLogic, 1000002);
            }

            act.game.TimeLineMgr.instance.ResumeTimeLine(act.game.TimeLineMgr.instance.newPlayerDir);
            AudioMgr.instance.PlaySound(AudioClips.AC_kuang);
        });

        CheckType checkType = CheckGuideType(guideType);
        switch(checkType)
        {
            case CheckType.None:
                break;
            case CheckType.OnlyMyBtn:
                guideEvent.targets.Clear();
                guideEvent.targets.Add(images[(int)guideType]);
                break;
            case CheckType.UseYouBtn:
                guideEvent.targets.Clear();
                images[(int)guideType].GetComponent<Image>().raycastTarget = false;
                if(guideType == GuideType.CardCon)
                {
                    GameObject tr = GameObject.Find("CardPrefab(Clone)");
                    //GameObject tr = GameObject.Find("CardPrefab(Clone)").transform.Find("CardTypeBG").gameObject;
                    Image a = tr.AddComponent<Image>();
                    a.color = new Color(a.color.r, a.color.g, a.color.b, 0);
                    guideEvent.targets.Add(tr);
                }
                break;
            default:
                break;
        }

        if(guideType == GuideType.CardCon)
        {
            guideBG.SetActive(true);
            images[(int)guideType + 1].GetComponent<Image>().raycastTarget = true;
            images[(int)guideType + 1].SetActive(true);
            images[(int)guideType + 1].transform.localScale = new Vector3(25, 25, 25);
            images[(int)guideType + 1].GetComponent<Button>().enabled = false;
            images[(int)guideType + 1].transform.DOScale(imageVecs[(int)guideType + 1], 1.5f).OnComplete(() => {
                images[(int)guideType + 1].GetComponent<Button>().enabled = true;
            });
            images[(int)guideType + 1].GetComponent<Image>().raycastTarget = false;
            guideBG.GetComponent<Image>().raycastTarget = false;
            buttons[(int)BtnControlType.StartCheckBtn].interactable = false;
            buttons[(int)BtnControlType.RoundOverBtn].interactable = false;
            buttons[(int)BtnControlType.ExitBtn].interactable = false;
        }
        else
        {
            guideBG.GetComponent<Image>().raycastTarget = true;
            buttons[(int)BtnControlType.StartCheckBtn].interactable = true;
            buttons[(int)BtnControlType.RoundOverBtn].interactable = true;
            buttons[(int)BtnControlType.ExitBtn].interactable = true;
        }
    }
    public void StartGuide()
    {
        gameObject.GetOrAddComponent<CanvasGroup>().blocksRaycasts = true;
        gameObject.GetOrAddComponent<CanvasGroup>().interactable = true;
        guideBG.GetComponent<Image>().raycastTarget = true;
        guideBG.SetActive(true);
        texts[0].transform.parent.gameObject.SetActive(true);
        gameObject.SetActive(true);
    }
    public void OverGuide()
    {
        gameObject.GetOrAddComponent<CanvasGroup>().blocksRaycasts = false;
        gameObject.GetOrAddComponent<CanvasGroup>().interactable = false;
        guideBG.GetComponent<Image>().raycastTarget = false;
        guideBG.SetActive(false);
        texts[0].transform.parent.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
    public CheckType CheckGuideType(GuideType guideType)
    {
        switch(guideType)
        {
            case GuideType.Event1:
            case GuideType.CardTypeCon1:
            case GuideType.CardCheckNum:
            case GuideType.OverRound:
            case GuideType.EventYear:
            case GuideType.Cost:
            case GuideType.qifen:
            case GuideType.Body:
                return CheckType.OnlyMyBtn;
            case GuideType.CardCon:
            case GuideType.CardSalt:
            case GuideType.CheckBtn:
                return CheckType.UseYouBtn;
            default:
                break;
        }
        return CheckType.None;
    }
}
