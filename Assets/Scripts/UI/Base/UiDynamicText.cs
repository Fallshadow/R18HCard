using UnityEngine.UI;

namespace act.ui
{
    public class UiDynamicText : Text
    {
#if UNITY_EDITOR // NOTE: 因應底層的define.
        protected override void Reset()
        {
            base.Reset();
            raycastTarget = false;
        }
#endif
    }
}