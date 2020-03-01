using System;
using UnityEngine;
using UnityEngine.UI;

namespace act.ui
{
    [RequireComponent(typeof(GraphicRaycaster))]
    public abstract class InteractableUiBase : UiBase
    {
        [SerializeField] protected GraphicRaycaster graphicRaycaster;

        protected override void Reset()
        {
            base.Reset();
            graphicRaycaster = GetComponent<GraphicRaycaster>();
        }

        public override void OnCreate()
        {
            gameObject.SetActive(true);
            SetInteractable(false);
            setAnimations();
            State = UiState.US_HIDE;
            SetVisible(false);
            Initialize();
        }

        public virtual void SetInteractable(bool isInteractable)
        {
            graphicRaycaster.enabled = isInteractable;
            // TODO: Joystick input.
        }

        public override void SetVisible(bool isActive)
        {
            if (isActive)
            {
                State = UiState.US_SHOW;
                gameObject.layer = UiManager.VisibleUiLayer;
                return;
            }

            State = UiState.US_HIDE;
            gameObject.layer = UiManager.InvisibleUiLayer;
        }

        public override void Show(Action showCompleteCb = null)
        {
            if (State != UiState.US_HIDE)
            {
                //debug.PrintSystem.LogWarning($"[InteractableUiBase] Show at wrong state. Name: {gameObject.name}, State: {State.ToString()}");
                showCompleteCb?.Invoke();
                return;
            }

            SetInteractable(false);
            onAnimationCompleteCallback += showCompleteCb;
            State = UiState.US_SHOW;
            SetVisible(true);
            onShow();
            playAnimations(UiAnimationClip.UAC_SHOW);
        }

        public override void ShowImmediate()
        {
            if (State != UiState.US_HIDE)
            {
                //debug.PrintSystem.LogError($"[InteractableUiBase] Show at wrong state. Name: {gameObject.name}, State: {State.ToString()}");
                return;
            }

            SetInteractable(true);
            onShow();
            State = UiState.US_SHOW;
            SetVisible(true);
        }

        public override void Hide(Action hideCompleteCb = null)
        {
            if (State != UiState.US_SHOW)
            {
                //debug.PrintSystem.LogWarning($"[InteractableUiBase] Hide at wrong state. Name: {gameObject.name}, State: {State.ToString()}");
                hideCompleteCb?.Invoke();
                return;
            }

            SetInteractable(false);
            onAnimationCompleteCallback += hideCompleteCb;
            onHide();
            playAnimations(UiAnimationClip.UAC_HIDE);
        }

        public override void HideImmediate()
        {
            if (State != UiState.US_SHOW)
            {
                return;
            }

            SetInteractable(false);
            onHide();
            State = UiState.US_HIDE;
            SetVisible(false);
        }

        protected override void onTransitionComplete()
        {
            switch (State)
            {
                case UiState.US_SHOWING:
                    {
                        State = UiState.US_SHOW;
                        SetInteractable(true);
                        onShowComplete();
                        break;
                    }
                case UiState.US_HIDING:
                    {
                        State = UiState.US_HIDE;
                        SetVisible(false);
                        onHideComplete();
                        break;
                    }
            }

            if (onAnimationCompleteCallback != null)
            {
                onAnimationCompleteCallback();
                onAnimationCompleteCallback = null;
            }
        }
    }
}