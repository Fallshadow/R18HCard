﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class RealToolGetClipLength : MonoBehaviour
{
    [MenuItem("RealTool/获取某个状态机的动画时长到游戏默认参数类数据")]
    public static void GetAnimClipLengthToGameDefineClass()
    {
    
    }
    
    [MenuItem("RealTool/获取某个fbx的animationclip")]
    public static void GetAnimClip()
    {
        string tname = "Assets/事件1.anim";
            AnimationClip src = AssetDatabase.LoadAssetAtPath<AnimationClip>("Assets/model/Motion/Mixamo/18.FBX");
            AnimationClip temp = new AnimationClip();
            EditorUtility.CopySerialized(src, temp);
            Directory.CreateDirectory(tname);
            AssetDatabase.CreateAsset(temp, tname);
    }
}
