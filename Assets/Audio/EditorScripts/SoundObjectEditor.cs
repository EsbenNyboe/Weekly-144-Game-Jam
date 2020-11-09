using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(SoundObject), true)]
public class SoundObjectEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		EditorGUI.BeginDisabledGroup(serializedObject.isEditingMultipleObjects);

		if (GUILayout.Button("Play"))
		{
			((SoundObject)target).PlayInEditor();
		}
		EditorGUI.EndDisabledGroup();
	}
}
