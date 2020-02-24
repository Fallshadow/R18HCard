using UnityEngine;
using UnityEngine.UI;

namespace act.ui
{
    [RequireComponent(typeof(GraphicRaycaster))]
    public abstract class FullScreenCanvasBase : InteractableUiBase
    {
        public override UiOpenType OpenType { get { return UiOpenType.UOT_FULL_SCREEN; } }

        protected override void Reset()
        {
            base.Reset();
            graphicRaycaster = GetComponent<GraphicRaycaster>();
            SetCanvasTransform();
        }

        public override void OnCreate()
        {
            gameObject.SetActive(true);
            SetInteractable(false);
            setAnimations();
            SetCanvasTransform();
            State = UiState.US_HIDE;
            SetVisible(false);
            Initialize();
        }

        protected virtual void SetCanvasTransform()
        {
            RectTransform rt = transform as RectTransform;
            rt.localScale = Vector3.one;
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.anchoredPosition = Vector2.zero;
            rt.sizeDelta = -UiManager.DangerAreaSize;
        }
    }
}