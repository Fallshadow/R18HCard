using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
public class unlocktest : MonoBehaviour
{
    bool ifr18;
    // Start is called before the first frame update
    void Start()
    {
        ifr18 = false;
        string path = System.Environment.CurrentDirectory+"/";
        if (Directory.Exists(path))
        {
            //获取文件信息
            DirectoryInfo direction = new DirectoryInfo(path);

            FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);
            for (int i = 0; i < files.Length; i++)
            {
                //检测后缀为txt的文件
                if (files[i].Name==("unlock.txt"))
                {
                    ifr18 = true;
                }
            }
        }
    }
    // Update is called once per frame
    void Update()
    {

    }

    public bool GetifR18()
    {
        return ifr18;
    }
}
