using System;
using UnityEngine;

namespace act.ui
{
    public abstract class UiWindowBase : InteractableUiBase
    {
        public override UiOpenType OpenType { get { return UiOpenType.UOT_POP_UP; } }

        public bool IsEasyHide { get { return isEasyHide; } }

        [SerializeField] private bool isEasyHide = true;
    }
}