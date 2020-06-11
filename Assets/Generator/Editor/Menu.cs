using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Generator))]
public class Menu : Editor
{
    public int seed;
    public override void OnInspectorGUI()
    {
        Generator generator = (Generator)target;

        generator.countRooms = EditorGUILayout.IntField("Count of rooms", generator.countRooms);
        generator.seed = EditorGUILayout.IntField("Seed", generator.seed);


        EditorGUILayout.PropertyField(serializedObject.FindProperty("Items"), true);

        EditorGUILayout.PropertyField(serializedObject.FindProperty("Enemies"), true);

        serializedObject.ApplyModifiedProperties();

        generator.teleport = (GameObject) EditorGUILayout.ObjectField("Teleport", generator.teleport, typeof(GameObject), true);
        generator.player = (GameObject)EditorGUILayout.ObjectField("Player", generator.player, typeof(GameObject), true);

        if (GUILayout.Button("Generate"))
        {
            EditorApplication.isPlaying = true;
        }

        if (GUILayout.Button("Copy seed to clipboard"))
        {
            generator.seedToCopy.ToString().copyToClipboard();
        }
    }
}
