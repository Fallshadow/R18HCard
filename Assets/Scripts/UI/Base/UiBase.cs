using System;
using System.Collections.Generic;
using UnityEngine;

namespace act.ui
{
    public enum UiState
    {
        US_NONE = 0,
        US_SHOWING,
        US_SHOW,
        US_HIDING,
        US_HIDE,
    }

    /// <summary>
    /// 掛載繼承這個基類的預製物，同型別同時只能載入一個。
    /// </summary>
    [DisallowMultipleComponent]
    public abstract class UiBase : MonoBehaviour
    {
        public const string IdleClipName = "Idle";
        public const string ShowClipName = "Show";
        public const string HideClipName = "Hide";
        public const string OnCompleteFunctionName = "onUiAnimationComplete";

        public virtual UiOpenType OpenType { get { return UiOpenType.UOT_COMMON; } }

        public bool IsDontDestroy { get { return isDontDestroy; } }
        public bool IsOpen { get { return onCloseCompleteHandler != null; } }
        public UiState State { get; protected set; }

        [SerializeField] protected bool isDontDestroy = false;

        protected Action onAnimationCompleteCallback = null;
        protected Action<UiBase> onCloseCompleteHandler = null;

        protected string uiAnimationSuffix = string.Empty;
        protected IUiAnimation[] uiAnimations = null;
        protected List<IUiAnimation> playingAnimations = null;

        protected virtual void Reset()
        {
            gameObject.layer = UiManager.VisibleUiLayer;
        }

        public virtual void OnCreate()
        {
            setAnimations();
            State = UiState.US_HIDE;
            SetVisible(false); // setVisible(UiState.US_HIDE);
            Initialize();
        }

        public virtual void OnRuin() // NOTE: 因為OnDestroy被MonoBehaviour使用了
        {
            if (State == UiState.US_NONE)
            {
                return;
            }

            if (onCloseCompleteHandler != null)
            {
                onClose();
                onCloseCompleteHandler = null;
            }

            if (State == UiState.US_SHOW || State == UiState.US_SHOWING)
            {
                onHide();
                onHideComplete();
            }
            Release();
        }

        /// <summary>
        /// UI創建時初始化物件
        /// </summary>
        public abstract void Initialize();

        /// <summary>
        /// UI銷毀時釋放物件資源
        /// </summary>
        public abstract void Release();

        /// <summary>
        /// 給UiManager統一呼叫刷新介面的接口
        /// </summary>
        public abstract void Refresh();

        /// <summary>
        /// 設定物件實際顯示狀態
        /// </summary>
        public virtual void SetVisible(bool isVisible)
        {
            if (isVisible)
            {
                gameObject.SetActive(true);
                return;
            }

            gameObject.SetActive(false);
        }

        public void Open(Action<UiBase> closeCompleteCb, Action openCompleteCb = null)
        {
            if (openCompleteCb != null)
            {
                //debug.PrintSystem.LogWarning($"[UserInterfaceBase] UI has already open. Name: {gameObject.name}");
                openCompleteCb?.Invoke();
                return;
            }

            onCloseCompleteHandler = closeCompleteCb;
            onOpen();
            Show(openCompleteCb);
        }

        protected virtual void onOpen() { }

        public void Close(Action completeCb = null)
        {
            if (onCloseCompleteHandler == null)
            {
                onClose();
                //debug.PrintSystem.LogWarning($"[UserInterfaceBase] UI has already closed. Name: {gameObject.name}");
                completeCb?.Invoke();
                return;
            }

            onClose();
            Hide(onCloseComplete + completeCb);
        }

        protected virtual void onClose() { }

        private void onCloseComplete()
        {
            onCloseCompleteHandler(this);
            onCloseCompleteHandler = null;
        }

        public virtual void Show(Action showCompleteCb = null)
        {
            if (State != UiState.US_HIDE)
            {
                //debug.PrintSystem.LogWarning($"[UserInterfaceBase] Show at wrong state. Name: {gameObject.name}, State: {State.ToString()}");
                showCompleteCb?.Invoke();
                return;
            }

            onAnimationCompleteCallback += showCompleteCb;
            State = UiState.US_SHOW;
            SetVisible(true);
            onShow();
            playAnimations(UiAnimationClip.UAC_SHOW);
        }

        public virtual void ShowImmediate()
        {
            if (State != UiState.US_HIDE)
            {
                //debug.PrintSystem.LogWarning($"[UserInterfaceBase] Show at wrong state. Name: {gameObject.name}, State: {State.ToString()}");
                return;
            }

            onShow();
            State = UiState.US_SHOW;
            SetVisible(true);
            onShowComplete();
        }

        protected virtual void onShow() { }

        protected virtual void onShowComplete() { }

        public virtual void Hide(Action hideCompleteCb = null)
        {
            if (State != UiState.US_SHOW)
            {
                //debug.PrintSystem.LogWarning($"[UserInterfaceBase] Hide at wrong state. Name: {gameObject.name}, State: {State.ToString()}");
                hideCompleteCb?.Invoke();
                return;
            }

            onAnimationCompleteCallback += hideCompleteCb;
            onHide();
            playAnimations(UiAnimationClip.UAC_HIDE);
        }

        /// <summary>
        /// 直接隱藏不播放動畫
        /// </summary>
        public virtual void HideImmediate()
        {
            if (State != UiState.US_SHOW)
            {
                //debug.PrintSystem.LogWarning($"[UserInterfaceBase] Hide at wrong state. Name: {gameObject.name}, State: {State.ToString()}");
                return;
            }

            onHide();
            State = UiState.US_HIDE;
            SetVisible(false);
            onHideComplete();
        }

        protected virtual void onHide() { }

        protected virtual void onHideComplete() { }

        // NOTE: DOTweenAnimation must call Awake() before calling this.
        protected void setAnimations()
        {
            uiAnimations = GetComponentsInChildren<IUiAnimation>();
            playingAnimations = new List<IUiAnimation>(uiAnimations.Length);
            for (int i = 0; i < uiAnimations.Length; ++i)
            {
                uiAnimations[i].Initialize(onAnimationComplete);
            }
        }

        protected void playAnimations(UiAnimationClip animationClip)
        {
            switch (animationClip)
            {
                case UiAnimationClip.UAC_SHOW:
                    {
                        State = UiState.US_SHOWING;
                        break;
                    }
                case UiAnimationClip.UAC_HIDE:
                    {
                        State = UiState.US_HIDING;
                        break;
                    }
                default:
                    {
                        //debug.PrintSystem.LogError("[UiBase] Play an incorrect animation.");
                        return;
                    }
            }

            // Check number of clips.
            for (int i = 0; i < uiAnimations.Length; ++i)
            {
                if (uiAnimations[i].HasClip(animationClip, uiAnimationSuffix))
                {
                    playingAnimations.Add(uiAnimations[i]);
                }
            }

            if (playingAnimations.Count == 0)
            {
                onTransitionComplete();
                return;
            }

            // Play clips.
            for (int i = 0, count = playingAnimations.Count; i < count; ++i)
            {
                playingAnimations[i].Play(animationClip, uiAnimationSuffix);
            }
        }

        public void StopAnimation()
        {
            onAnimationCompleteCallback = null;
            for (int i = 0; i < uiAnimations.Length; ++i)
            {
                uiAnimations[i].Stop();
            }
            playingAnimations.Clear();
        }

        protected void onAnimationComplete(IUiAnimation uiAnimation)
        {
            for (int i = playingAnimations.Count - 1; i >= 0; --i)
            {
                if (playingAnimations[i] == uiAnimation)
                {
                    playingAnimations.RemoveAt(i);
                    break;
                }
            }

            if (playingAnimations.Count != 0)
            {
                return;
            }

            onTransitionComplete();
        }

        // NOTE: Only for UnityEngine.Animaion.
        protected void onUiAnimationComplete(AnimationEvent animationEvent)
        {
            onAnimationComplete(animationEvent.objectReferenceParameter as IUiAnimation);
        }

        protected virtual void onTransitionComplete()
        {
            switch (State)
            {
                case UiState.US_SHOWING:
                    {
                        State = UiState.US_SHOW;
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