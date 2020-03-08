using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace act
{
    namespace game
    {
        public class ModelController : SingletonMonoBehaviorNoDestroy<GameController>
        {
            public enum Action
            {
                IDLE,
                JUMP,
                WALK
            };
            public enum Face
            {
                DEFAULT,
                ANGRY,
                SMILE,
                ASHAMED,
                SURPRISE,
                EYE_CLOSE,
                MOUTH_A,
                MOUTH_E,
                MOUTH_I,
                MOUTH_O,
                MOUTH_U
            };
            public Animator Animator = null;
            [Serializable]
            public class ActionAnimationClipDictionary : SerializableDictionary<Action, AnimationClip> { }
            public ActionAnimationClipDictionary Actions = new ActionAnimationClipDictionary
            {
                {Action.IDLE ,null},
                {Action.JUMP ,null},
                {Action.WALK ,null}
            };
            [Serializable]
            public class FaceAnimationClipDictionary : SerializableDictionary<Face, AnimationClip> { }
            public FaceAnimationClipDictionary Faces = new FaceAnimationClipDictionary
            {
                {Face.DEFAULT,null},
                {Face.ANGRY,null},
                {Face.SMILE,null},
                {Face.ASHAMED,null},
                {Face.SURPRISE,null},
                {Face.EYE_CLOSE,null},
                {Face.MOUTH_A,null},
                {Face.MOUTH_E,null},
                {Face.MOUTH_I,null},
                {Face.MOUTH_O,null},
                {Face.MOUTH_U,null},
            };
            private static int FACE_LAYER = 1;
            private bool keepAction = true;
            private bool keepFace = true;
            private void Update()
            {
                if (Animator == null)
                    return;
                if (Actions[Action.IDLE] != null && !keepAction)
                {
                    var actionStateInfo = Animator.GetCurrentAnimatorStateInfo(0);
                    if (!actionStateInfo.IsName(Actions[Action.IDLE].name) && actionStateInfo.normalizedTime >= 1.0f)
                        Animator.CrossFade(Actions[Action.IDLE].name, 0.0f);
                }
                if (Faces[Face.DEFAULT] != null && !keepFace)
                {
                    var faceStateInfo = Animator.GetCurrentAnimatorStateInfo(FACE_LAYER);
                    if (!faceStateInfo.IsName(Faces[Face.DEFAULT].name) && faceStateInfo.normalizedTime >= 1.0f)
                        Animator.CrossFade(Faces[Face.DEFAULT].name, 0.0f, FACE_LAYER);
                }
            }

            public void ChangeAction(Action action, bool keep = true)
            {
                if (Animator == null)
                    return;
                if (!Enum.IsDefined(typeof(Action), action))
                    return;
                if (Actions[action] == null)
                    return;
                keepAction = keep;
                Animator.CrossFade(Actions[action].name, 0.0f);
            }

            public void ChangeFace(Face face, bool keep = true)
            {
                if (Animator == null)
                    return;
                if (!Enum.IsDefined(typeof(Face), face))
                    return;
                if (Faces[face] == null)
                    return;
                keepFace = keep;
                Animator.SetLayerWeight(FACE_LAYER, 1.0f);
                Animator.CrossFade(Faces[face].name, 0.0f, FACE_LAYER);
            }
        }
    }
}
