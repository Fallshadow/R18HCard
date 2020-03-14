using act.game;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(ModelController))]
public class ModelControllerEditor : Editor
{
    private bool showActions = true;
    private bool showTestChangeAction = true;
    private bool showFaces = true;
    private bool showTestChangeFace = true;
    private bool keepAction = true;
    private act.game.Action actionChangeTo = act.game.Action.IDLE;
    private bool keepFace = true;
    private act.game.Face faceChangeTo = act.game.Face.DEFAULT;
    public override void OnInspectorGUI()
    {
        ModelController controller = (ModelController)target;
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(target as MonoBehaviour), typeof(MonoScript), false);
        EditorGUI.EndDisabledGroup();
        controller.Animator = EditorGUILayout.ObjectField("Animator", controller.Animator, typeof(Animator), true) as Animator;
        showActions = EditorGUILayout.Foldout(showActions, "Actions", true);
        if (showActions)
        {
            var actionsList = new List<act.game.Action>(controller.Actions.Keys);
            foreach (var action in actionsList)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.Space(4);
                EditorGUILayout.LabelField(action.ToString());
                controller.Actions[action] = EditorGUILayout.ObjectField(controller.Actions[action], typeof(AnimationClip), true) as AnimationClip;
                EditorGUILayout.EndHorizontal();
            }
        }
        showTestChangeAction = EditorGUILayout.Foldout(showTestChangeAction, "TestChangeAction", true);
        var changeAction = false;
        if (showTestChangeAction)
        {
            actionChangeTo = (act.game.Action)EditorGUILayout.EnumPopup("Action", actionChangeTo);
            keepAction = EditorGUILayout.Toggle("KeepAction", keepAction);
            changeAction = GUILayout.Button("Change Action");
        }
        if (changeAction)
        {
            controller.ChangeAction(actionChangeTo, keepAction);
        }
        showFaces = EditorGUILayout.Foldout(showFaces, "Faces", true);
        if (showFaces)
        {
            var facesList = new List<act.game.Face>(controller.Faces.Keys);
            foreach (var face in facesList)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.Space(4);
                EditorGUILayout.LabelField(face.ToString());
                controller.Faces[face] = EditorGUILayout.ObjectField(controller.Faces[face], typeof(AnimationClip), true) as AnimationClip;
                EditorGUILayout.EndHorizontal();
            }
        }
        showTestChangeFace = EditorGUILayout.Foldout(showTestChangeFace, "TestChangeFace", true);
        var changeFace = false;
        if (showTestChangeFace)
        {
            faceChangeTo = (act.game.Face)EditorGUILayout.EnumPopup("Face", faceChangeTo);
            keepFace = EditorGUILayout.Toggle("KeepFace", keepFace);
            changeFace = GUILayout.Button("Change Face");
        }
        if (changeFace)
        {
            controller.ChangeFace(faceChangeTo, keepFace);
        }
    }
}
