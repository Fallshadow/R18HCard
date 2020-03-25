//将以下代码绑定到相机上
using UnityEngine;
using System.Collections;

namespace act.game
{
    public class MoveCamera : MonoBehaviour
    {
        public float speed = 10;
        //void Update()
        //{
        //    float x = Input.GetAxis("Mouse X");//旋转相机
        //    float y = Input.GetAxis("Mouse Y");
        //    gameObject.transform.RotateAround(gameObject.transform.position, Vector3.up, x);
        //    gameObject.transform.RotateAround(gameObject.transform.position, Vector3.right, y);
        //    //GameObject cube = GameObject.Find("Cube");//要正对着相机的物体
        //    //cube.transform.RotateAround(gameObject.transform.position, Vector3.up, x);
        //    //cube.transform.RotateAround(gameObject.transform.position, Vector3.right, y);
        //}

        private void Update()
        {
            float x = Input.GetAxis("Mouse X");//鼠标横向移动,让Unity中摄像机绕Y轴转动      
            float y = Input.GetAxis("Mouse Y");//鼠标纵向移动,让Unity中摄像机绕X轴转动

            if(x != 0 || y != 0)
                RotateView(x, y);
            //需要限制沿X轴旋转角度
        }
        private void RotateView(float x, float y)
        {
            x *= speed * Time.deltaTime; //鼠标横向移动变化值
            transform.Rotate(0, x, 0, Space.World); //Unity中摄像机随着x的变化绕Y轴转动，必须是绕世界坐标的Y轴

            y *= speed * Time.deltaTime;//鼠标纵向移动变化值
            transform.Rotate(-y, 0, 0);//Unity中摄像机随着y的变化绕X轴转动

        }
    }
}
