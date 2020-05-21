using UnityEngine;

namespace act.game
{
    /// <summary>
    /// 欧拉角转换
    /// </summary>
    public static class EulerAngleConversion
    {
        /// <summary>
        ///     自轴旋转欧拉角计算
        /// </summary>
        /// <param name="originEulerAngle">当前物体欧拉角(transform.eulerAngles)</param>
        /// <param name="direction">旋转轴向量及角度大小（Vector3.up）</param>
        /// <returns>return : 自转后的欧拉角</returns>
        public static Vector3 AxisRotation_EulerAngles(Vector3 originEulerAngle, Vector3 direction)
        {
            return (Quaternion.Euler(originEulerAngle) * Quaternion.Euler(direction)).eulerAngles;
        }

        /// <summary>
        ///     向量转换为欧拉角
        /// </summary>
        /// <param name="angleVector3">指向向量</param>
        /// <returns>return : 欧拉角</returns>
        public static Vector3 Vector3ToEulerAngles(Vector3 angleVector3)
        {
            return Quaternion.LookRotation(angleVector3).eulerAngles;
        }

        /// <summary>
        ///     欧拉角转换：正负无限 转换成 正负0~180 的欧拉角
        /// </summary>
        /// <param name="eulerAngles">要转换的欧拉角</param>
        /// <returns>return ： 转换成 正负0~180 的欧拉角</returns>
        public static Vector3 EulerAngles_PN_To_PN180(Vector3 eulerAngles)
        {
            eulerAngles.x = Angle_PN_To_PN180(eulerAngles.x);
            eulerAngles.y = Angle_PN_To_PN180(eulerAngles.y);
            eulerAngles.z = Angle_PN_To_PN180(eulerAngles.z);
            return eulerAngles;
        }

        /// <summary>
        ///     欧拉角转换：正负无限 转换成 正0~360 的欧拉角
        /// </summary>
        /// <param name="eulerAngles">要转换的欧拉角</param>
        /// <returns>return ： 转换成 正0~360 的欧拉角</returns>
        public static Vector3 EulerAngles_PN_To_P360(Vector3 eulerAngles)
        {
            eulerAngles.x = Angle_PN_To_P360(eulerAngles.x);
            eulerAngles.y = Angle_PN_To_P360(eulerAngles.y);
            eulerAngles.z = Angle_PN_To_P360(eulerAngles.z);
            return eulerAngles;
        }

        /// <summary>
        ///     欧拉角转换：正0~360 转换成 正负0~180 的欧拉角
        /// </summary>
        /// <param name="eulerAngles">要转换的欧拉角</param>
        /// <returns>return ： 转换成 正负0~180 的欧拉角</returns>
        public static Vector3 EulerAngles_P360_To_PN180(Vector3 eulerAngles)
        {
            eulerAngles.x = Angle_P360_To_PN180(eulerAngles.x);
            eulerAngles.y = Angle_P360_To_PN180(eulerAngles.y);
            eulerAngles.z = Angle_P360_To_PN180(eulerAngles.z);
            return eulerAngles;
        }

        /// <summary>
        ///     角度转换：正负无限 转换成 正负0~180 的角度
        /// </summary>
        /// <param name="angle">要转换角度</param>
        /// <returns>return ： 转换成 正负0~180 的角度</returns>
        public static float Angle_PN_To_PN180(float angle)
        {
            angle = Angle_PN_To_P360(angle);
            return Angle_P360_To_PN180(angle);
        }

        /// <summary>
        ///     角度转换：正负无限 转换成 正0~360 的角度
        /// </summary>
        /// <param name="angle">要转换角度</param>
        /// <returns>return ： 转换成 正0~360 的角度</returns>
        public static float Angle_PN_To_P360(float angle)
        {
            return (angle %= 360) < 0 ? angle + 360 : angle;
        }

        /// <summary>
        ///     角度转换：正0~360 转换成 正负0~180 的角度
        /// </summary>
        /// <param name="angle">要转换角度</param>
        /// <returns>return ： 转换成 正负0~180 的角度</returns>
        public static float Angle_P360_To_PN180(float angle)
        {
            return angle >= 180 ? angle - 360 : angle;
        }
    }
}
