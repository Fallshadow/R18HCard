//将以下代码绑定到相机上
using UnityEngine;
using System.Collections;

namespace act.game
{

    public class MoveCamera2 : SingletonMonoBehavior<MoveCamera>
    {
        [Header("这里是相机对应的位置哦")]
        public Vector3[] pos;

        public float speed = 100;

        public game.CameraType ct = game.CameraType.TableMain;
        public Vector3 TableMainPos = Vector3.zero;
        public Vector3 TableMainRot = Vector3.zero;
        public Vector2 TableMainPosMaxX = Vector2.zero;
        public Vector2 TableMainPosMaxY = Vector2.zero;
        public bool CanMove = false;

        protected override void Awake()
        {
            base.Awake();
            evt.EventManager.instance.Register<bool>(evt.EventGroup.INPUT, (short)evt.InputEvent.IE_Mouse_Right_Down, SetCanMove);
            evt.EventManager.instance.Register< bool>(evt.EventGroup.INPUT, (short)evt.InputEvent.IE_Mouse_Right_Up, SetCanMove);
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            evt.EventManager.instance.Unregister< bool>(evt.EventGroup.INPUT, (short)evt.InputEvent.IE_Mouse_Right_Down, SetCanMove);
            evt.EventManager.instance.Unregister< bool>(evt.EventGroup.INPUT, (short)evt.InputEvent.IE_Mouse_Right_Up, SetCanMove);
        }

        public void SetCanMove(bool trigger)
        {
            CanMove = trigger;
            switch(ct)
            {
                case CameraType.TableMain:
                    if(trigger == false)
                    {
                        Cursor.visible = true;
                        //SetPosAndRot(TableMainPos, TableMainRot);
                    }
                    break;
                default:
                    break;
            }

        }
        public void SetCameraPos(Vector3 camPos)
        {
            transform.position = camPos;
        }

        public void SetPosAndRot(Vector3 pos, Vector3 rot)
        {
            transform.localPosition = pos;
            transform.localRotation = Quaternion.Euler(rot);
        }

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
            if(CanMove)
            {
                Cursor.visible = false;
                float x = Input.GetAxis("Mouse X");//鼠标横向移动,让Unity中摄像机绕Y轴转动      
                float y = Input.GetAxis("Mouse Y");//鼠标纵向移动,让Unity中摄像机绕X轴转动

                if(x != 0 || y != 0)
                    RotateView(x, y);
                //需要限制沿X轴旋转角度
            }
        }
        private void RotateView(float x, float y)
        {
            x *= speed * Time.deltaTime; //鼠标横向移动变化值
            

            
            transform.Rotate(0, x, 0, Space.World); //Unity中摄像机随着x的变化绕Y轴转动，必须是绕世界坐标的Y轴

            y *= speed * Time.deltaTime;//鼠标纵向移动变化值
            
            transform.Rotate(-y, 0, 0);//Unity中摄像机随着y的变化绕X轴转动

            Vector3 tempRot = transform.rotation.eulerAngles;
            //Debug.Log(tempRot.x);
            if(tempRot.x > TableMainPosMaxX.x + 180)
            {
                
                tempRot.x -= 360;
            }
            float tempy = Mathf.Clamp(tempRot.y, TableMainPosMaxY.x, TableMainPosMaxY.y);
            float tempx = Mathf.Clamp(tempRot.x, TableMainPosMaxX.x, TableMainPosMaxX.y);
            tempRot = new Vector3(tempx, tempy, tempRot.z);
            
            transform.rotation =Quaternion.Euler(tempRot);

        }
    }
}
