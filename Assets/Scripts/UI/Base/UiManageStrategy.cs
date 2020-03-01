using System;
using System.Collections.Generic;
using UnityEngine;

namespace act.ui
{
    public enum UiOpenType
    {
        UOT_COMMON = 0,
        UOT_FULL_SCREEN = 1,
        UOT_POP_UP = 2,
    }

    public enum UiOperation
    {
        UO_CLOSE_TOP_CANVAS = 0,
        UO_CLOSE_ALL_CANVAS_LEFT_ONE = 1,
        UO_CLOSE_ALL_CANVAS = 2,
    }

    public class UiManageStrategy
    {
        public const float DefaultFrontMaskAlpha = 0.58f;

        public RectTransform MainRoot { get { return fullScreenRoot; } }

        private RectTransform fullScreenRoot = null;
        private RectTransform popUpWindowRoot = null;

        private UiFullScreenMask frontMask = null;

        private LinkedList<UiBase> fullScreenCavases = new LinkedList<UiBase>();
        private LinkedList<UiBase> popUpWindows = new LinkedList<UiBase>();
        private LinkedList<UiBase> commonUis = new LinkedList<UiBase>();

        public UiManageStrategy(RectTransform[] roots)
        {
            fullScreenRoot = roots[0];
            popUpWindowRoot = roots[1];
            frontMask = popUpWindowRoot.GetComponentInChildren<UiFullScreenMask>();
        }

        public void Clear()
        {
            frontMask.SetFrontMask(0f);
            popUpWindowRoot.gameObject.SetActive(false);
            fullScreenCavases.Clear();
            popUpWindows.Clear();
            commonUis.Clear();
        }

        public UiBase CreateUi(UiBase uiPrefab)
        {
            UiBase ui;
            switch (uiPrefab.OpenType)
            {
                case UiOpenType.UOT_FULL_SCREEN:
                case UiOpenType.UOT_COMMON:
                    {
                        ui = UnityEngine.Object.Instantiate(uiPrefab, fullScreenRoot);
                        break;
                    }
                case UiOpenType.UOT_POP_UP:
                    {
                        ui = UnityEngine.Object.Instantiate(uiPrefab, popUpWindowRoot);
                        break;
                    }
                default:
                    {
                        return null;
                    }
            }
            return ui;
        }

        public void OpenUi(UiBase ui, Action completeCb)
        {
            switch (ui.OpenType)
            {
                case UiOpenType.UOT_FULL_SCREEN:
                    {
                        openFullScreenCanvas(ui, completeCb);
                        break;
                    }
                case UiOpenType.UOT_POP_UP:
                    {
                        openPopUpWindow(ui, completeCb);
                        break;
                    }
                case UiOpenType.UOT_COMMON:
                    {
                        openCommonUi(ui, completeCb);
                        break;
                    }
            }
        }

        public void CloseUi(UiBase ui, Action completeCb)
        {
            switch (ui.OpenType)
            {
                case UiOpenType.UOT_FULL_SCREEN:
                    {
                        closeFullScreenCanvas(ui, completeCb);
                        break;
                    }
                case UiOpenType.UOT_POP_UP:
                    {
                        closePopUpWindow(ui, completeCb);
                        break;
                    }
                case UiOpenType.UOT_COMMON:
                    {
                        closeCommonUi(ui, completeCb);
                        break;
                    }
            }
        }

        public void OperateUi(int op)
        {
            switch ((UiOperation)op)
            {
                case UiOperation.UO_CLOSE_TOP_CANVAS:
                    closeTopCanvas();
                    break;
                case UiOperation.UO_CLOSE_ALL_CANVAS_LEFT_ONE:
                    closeAllCanvas(true);
                    break;
                case UiOperation.UO_CLOSE_ALL_CANVAS:
                    closeAllCanvas(false);
                    break;
            }
        }

        #region Main Canvas Related
        private void openFullScreenCanvas(UiBase ui, Action completeCb)
        {
            if (fullScreenCavases.Contains(ui))
            {
                debug.PrintSystem.LogWarning($"[MainCanvasRoot] UI has already open. UI: {ui.name}");
                completeCb?.Invoke();
                return;
            }

            if (fullScreenCavases.Count == 0 || fullScreenCavases.Last.Value.State == UiState.US_HIDE)
            {
                fullScreenCavases.AddLast(ui);
                ui.transform.SetParent(fullScreenRoot);
                ui.transform.SetAsFirstSibling();
                ui.Open(onFullScreenCanvasClose, completeCb);
                return;
            }

            fullScreenCavases.Last.Value.Hide(() => openFullScreenCanvas(ui, completeCb));
        }

        private void closeFullScreenCanvas(UiBase ui, Action completeCb)
        {
            if (!fullScreenCavases.Contains(ui))
            {
                debug.PrintSystem.LogWarning($"[MainCanvasRoot] UI is not open. UI: {ui.name}");
                completeCb?.Invoke();
                return;
            }

            ui.Close(completeCb);
        }

        private void onFullScreenCanvasClose(UiBase ui)
        {
            bool isOpenLast = (ui == fullScreenCavases.Last.Value);
            fullScreenCavases.Remove(ui);
            if (fullScreenCavases.Count == 0 || !isOpenLast)
            {
                return;
            }

            fullScreenCavases.Last.Value.transform.SetAsFirstSibling();
            fullScreenCavases.Last.Value.Show();
        }

        private void closeTopCanvas()
        {
            if (fullScreenCavases.Count == 0)
            {
                return;
            }

            closeFullScreenCanvas(fullScreenCavases.Last.Value, null);
        }

        private void closeAllCanvas(bool isLeaveOne)
        {
            int leftCount = isLeaveOne ? 1 : 0;
            while (fullScreenCavases.Count > leftCount)
            {
                fullScreenCavases.Last.Value.Close();
                fullScreenCavases.RemoveLast();
            }
        }

        private void openCommonUi(UiBase ui, Action completeCb)
        {
            commonUis.AddLast(ui);
            ui.transform.SetParent(fullScreenRoot);
            ui.transform.SetAsLastSibling();
            ui.Open(onCommonUiClose, completeCb);
        }

        private void closeCommonUi(UiBase ui, Action completeCb)
        {
            ui.Close(completeCb);
        }

        private void onCommonUiClose(UiBase ui)
        {
            popUpWindows.Remove(ui);
        }
        #endregion

        #region Pop-Up Windows Related
        private void openPopUpWindow(UiBase ui, Action completeCb)
        {
            if (popUpWindows.Contains(ui))
            {
                completeCb?.Invoke();
                return;
            }

            frontMask.SetFrontMask(DefaultFrontMaskAlpha, true); // TODO: Read setting in windows.
            frontMask.transform.SetAsLastSibling();
            ui.transform.SetParent(popUpWindowRoot);
            ui.transform.SetAsLastSibling();
            ui.Open(onPopUpWindowClose, completeCb);
            popUpWindows.AddLast(ui);
            popUpWindowRoot.gameObject.SetActive(true);
        }

        private void closePopUpWindow(UiBase ui, Action completeCb)
        {
            ui.Close(completeCb);
        }

        private void onPopUpWindowClose(UiBase ui)
        {
            popUpWindows.Remove(ui);
            if (popUpWindows.Count == 0)
            {
                frontMask.SetFrontMask(0f);
                popUpWindowRoot.gameObject.SetActive(false);
            }
            else
            {
                popUpWindows.Last.Value.transform.SetAsLastSibling();
            }
        }
        #endregion
    }
}