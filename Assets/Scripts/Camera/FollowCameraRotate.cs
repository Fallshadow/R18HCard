using UnityEngine;

namespace act.game
{
    /// <summary>
    ///     跟随相机旋转
    /// </summary>
    public class FollowCameraRotate : MonoBehaviour
    {
        /// <summary>
        ///     是否从Transform获取默认值，如果是否的话则从输入值获取
        /// </summary>
        public bool DefaultFromTransform = false;

        /// <summary>
        ///     默认相机角度
        /// </summary>
        public Vector3 DefaultCameraAngles;

        /// <summary>
        ///     默认相机旋转量
        /// </summary>
        private Quaternion defaultCameraRot;

        /// <summary>
        ///     默认本地角度
        /// </summary>
        public Vector3 DefaultLocalAngles;

        /// <summary>
        ///     默认本地旋转量
        /// </summary>
        private Quaternion defaultLocalRot;

        /// <summary>
        ///     相机Transform，未设置则取主相机
        /// </summary>
        public Transform CameraTransform = null;
        /// <summary>
        ///     跟随相机旋转的物体
        /// </summary>
        public Transform FollowObjectTransform = null;

        /// <summary>
        ///     水平最大角度
        /// </summary>
        public float HorizontalAngleMax = 60.0f;

        /// <summary>
        ///     水平最小角度
        /// </summary>
        public float HorizontalAngleMin = -60.0f;

        /// <summary>
        ///     垂直最大角度
        /// </summary>
        public float VerticalAngleMax = 0.0f;

        /// <summary>
        ///     垂直最小角度
        /// </summary>
        public float VerticalAngleMin = -30.0f;

        private void Awake()
        {
            if (DefaultFromTransform)
            {
                if (FollowObjectTransform != null)
                {
                    defaultLocalRot = FollowObjectTransform.localRotation;
                    DefaultLocalAngles = defaultLocalRot.eulerAngles;
                }

                if (CameraTransform != null)
                {
                    defaultCameraRot = CameraTransform.rotation;
                    DefaultCameraAngles = CameraTransform.rotation.eulerAngles;
                }
                else
                {
                    defaultCameraRot = Camera.main.transform.rotation;
                    DefaultCameraAngles = Camera.main.transform.rotation.eulerAngles;
                }
            }
            else
            {
                defaultLocalRot = Quaternion.Euler(DefaultLocalAngles);
                defaultCameraRot = Quaternion.Euler(DefaultCameraAngles);
            }
        }

        private void LateUpdate()
        {
            if (FollowObjectTransform == null) return;
            var deltaRot = Quaternion.Inverse(defaultCameraRot) *
                           (CameraTransform == null ? Camera.main.transform.rotation : CameraTransform.rotation);
            var deltaAngles = deltaRot.eulerAngles;
            var deltaHorizontalAngle = EulerAngleConversion.Angle_PN_To_PN180(deltaAngles.y);
            var deltaVerticalAngle = -EulerAngleConversion.Angle_PN_To_PN180(deltaAngles.x);
            deltaAngles.x = Mathf.Clamp(deltaHorizontalAngle, HorizontalAngleMin, HorizontalAngleMax);
            deltaAngles.y = Mathf.Clamp(deltaVerticalAngle, VerticalAngleMin, VerticalAngleMax);
            deltaAngles.z = 0.0f;
            deltaRot = Quaternion.Euler(deltaAngles);
            FollowObjectTransform.localRotation = defaultLocalRot * deltaRot;
        }
    }
}
