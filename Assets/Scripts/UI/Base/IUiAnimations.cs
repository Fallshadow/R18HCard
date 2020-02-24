using System;

namespace act.ui
{
    public enum UiAnimationClip
    {
        UAC_CUSTOM = 0,
        UAC_SHOW = 1,
        UAC_HIDE = ~UAC_SHOW,
        UAC_IDLE = 2
    }

    public interface IUiAnimation
    {
        void Initialize(Action<IUiAnimation> completeCb);
        bool HasClip(UiAnimationClip clip, string suffix);
        void Play(UiAnimationClip clip, string suffix);
        void Stop();
    }
}