using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace act.ui
{
    [BindingResource("Play/GameOverCanvas")]
    public class GameOverCanvas : InteractableUiBase
    {
        public Image Image;
        public void ExitToMenu()
        {
            game.GameFlowMgr.instance.ClearData();
            ui.UiManager.instance.CreateUi<PlayCanvas>().ReturnToMain();
        }

        public override void Initialize()
        {
        }

        public override void Refresh()
        {
        }

        public override void Release()
        {
        }

        protected override void onShow()
        {
            Image.raycastTarget = false;
            Image.DOFade(0, 0);
            Image.DOFade(0.5f, 1).OnComplete(()=> {
                Image.raycastTarget = true;
            });
        }
    }

}
