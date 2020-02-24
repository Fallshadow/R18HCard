using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace act.ui
{
    [BindingResource("MainMenu/MainMenuCanvas")]
    public class MainMenuCanvas : InteractableUiBase
    {
        public override UiOpenType OpenType => base.OpenType;

        public override bool Equals(object other)
        {
            return base.Equals(other);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override void Hide(Action hideCompleteCb = null)
        {
            base.Hide(hideCompleteCb);
        }

        public override void HideImmediate()
        {
            base.HideImmediate();
        }

        public override void Initialize()
        {

        }

        public override void OnCreate()
        {
            base.OnCreate();
        }

        public override void OnRuin()
        {
            base.OnRuin();
        }

        public override void Refresh()
        {

        }

        public override void Release()
        {

        }

        public override void SetInteractable(bool isInteractable)
        {
            base.SetInteractable(isInteractable);
        }

        public override void SetVisible(bool isActive)
        {
            base.SetVisible(isActive);
        }

        public override void Show(Action showCompleteCb = null)
        {
            base.Show(showCompleteCb);
        }

        public override void ShowImmediate()
        {
            base.ShowImmediate();
        }

        public override string ToString()
        {
            return base.ToString();
        }

        protected override void onClose()
        {
            base.onClose();
        }

        protected override void onHide()
        {
            base.onHide();
        }

        protected override void onHideComplete()
        {
            base.onHideComplete();
        }

        protected override void onOpen()
        {
            base.onOpen();
        }

        protected override void onShow()
        {
            base.onShow();
        }

        protected override void onShowComplete()
        {
            base.onShowComplete();
        }

        protected override void onTransitionComplete()
        {
            base.onTransitionComplete();
        }

        protected override void Reset()
        {
            base.Reset();
        }
    }
}