using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace act.utility
{
    public static class AnimUtility
    {
        /// <summary>
        /// 获取该animator下该名字的动画时长
        /// </summary>
        /// <param name="animator">动画状态机</param>
        /// <param name="clipName">动画名称</param>
        /// <returns></returns>
        public static float GetAnimatorClipLength(Animator animator, string clipName)
        {
            //检测判空
            if(null == animator || string.IsNullOrEmpty(clipName) || null == animator.runtimeAnimatorController)
            {
                return 0f;
            }

            RuntimeAnimatorController ac = animator.runtimeAnimatorController;
            AnimationClip[] acs = ac.animationClips;

            //检测判空
            if(null == acs || acs.Length <= 0)
            {
                return 0f;
            }

            AnimationClip tAnimationClip;

            for(int tCounter = 0; tCounter < acs.Length; tCounter++)
            {
                tAnimationClip = acs[tCounter];
                if(null != tAnimationClip && tAnimationClip.name == clipName)
                {
                    return tAnimationClip.length;
                }
            }

            return 0f;
        }
    }

}
