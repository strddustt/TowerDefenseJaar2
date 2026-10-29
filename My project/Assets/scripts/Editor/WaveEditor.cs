using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.UIElements;
using UnityEngine;

public class WaveEditor : EditorWindow
{
    private WaveSO wave;
    SerializedObject serializedWave;
    private WaveSO newWave;
    private WaveSpawner spawner;
    SerializedObject serializedSpawner;
    private Dictionary<WaveSO, int> waveLog = new Dictionary<WaveSO, int>();
    [SerializeField, HideInInspector] private int selectedIndex = 0;
    private Vector2 scrollPos;

    private void OnEnable() => Init();


    [InitializeOnLoadMethod]
    private static void Hook()
    {
        EditorSceneManager.sceneOpened += (scene, mode) =>
            EditorApplication.delayCall += Refresh;
        EditorSceneManager.newSceneCreated += (scene, setup, mode) =>
            EditorApplication.delayCall += Refresh;
    }

    private static void Refresh() //opens or closes window on scene change, depending on if the scene contains a wave spawner
    {
        bool hasSpawner = FindFirstObjectByType<WaveSpawner>() != null;

        if (HasOpenInstances<WaveEditor>())
            GetWindow<WaveEditor>().Close();

        if (hasSpawner)
        {
            GetWindow<WaveEditor>().Init();
        }
    }
    private void Init()
    {
        if (serializedSpawner == null)
        {
            spawner = FindFirstObjectByType<WaveSpawner>();
            serializedSpawner = new SerializedObject(spawner);
        }
    }

    [MenuItem("Window/Wave Timeline")]
    public static void ShowWindow()
    {
        GetWindow<WaveEditor>();
    }
    private void OnGUI()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        serializedSpawner.Update();
        selectedIndex = EditorGUILayout.Popup("Waves", selectedIndex, PopupList());

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Move up"))
        {
            SerializedProperty waves = serializedSpawner.FindProperty("waves");
            int swapIndex;
            if (selectedIndex == 0)
            {
                swapIndex = waves.arraySize - 1;
            }
            else
            {
                swapIndex = selectedIndex - 1;
            }
            waves.MoveArrayElement(selectedIndex, swapIndex);
            selectedIndex = swapIndex;
            serializedSpawner.ApplyModifiedProperties();
        }
        else if (GUILayout.Button("Move down"))
        {
            SerializedProperty waves = serializedSpawner.FindProperty("waves");
            int swapIndex;
            if (selectedIndex == waves.arraySize - 1)
            {
                swapIndex = 0;
            }
            else
            {
                swapIndex = selectedIndex + 1;
            }
            waves.MoveArrayElement(selectedIndex, swapIndex);
            selectedIndex = swapIndex;
            serializedSpawner.ApplyModifiedProperties();
        }
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("New Wave")) //creates and saves a SO asset in memory, and passes it to the wave list
        {
            newWave = ScriptableObject.CreateInstance<WaveSO>();
            newWave.values = new List<SpawnValues> { new SpawnValues { count = 1, delay = 0, interval = 1f } };
            SerializedProperty waves = serializedSpawner.FindProperty("waves");
            string waveName = AssetDatabase.GenerateUniqueAssetPath("Assets/SO instances/central/wave/newWave.asset");
            AssetDatabase.CreateAsset(newWave, waveName);
            AssetDatabase.SaveAssets();
            waves.InsertArrayElementAtIndex(waves.arraySize);
            waves.GetArrayElementAtIndex(waves.arraySize - 1).objectReferenceValue = newWave;
            newWave = null;
            serializedSpawner.ApplyModifiedProperties();
            selectedIndex = waves.arraySize - 1;
            GUIUtility.ExitGUI();
        }
        else if (GUILayout.Button("Delete wave")) //deletes the currently selected SO from the popup.
        {
            SerializedProperty waves = serializedSpawner.FindProperty("waves");
            if (selectedIndex >= 0 && selectedIndex < waves.arraySize)
            {
                SerializedProperty waveToDelete = waves.GetArrayElementAtIndex(selectedIndex);
                string path = null;
                if (waveToDelete.objectReferenceValue != null)
                {
                    path = AssetDatabase.GetAssetPath(waveToDelete.objectReferenceValue);
                    waveToDelete.objectReferenceValue = null;
                }
                waves.DeleteArrayElementAtIndex(selectedIndex);
                serializedSpawner.ApplyModifiedProperties();
                if (path != null) { AssetDatabase.DeleteAsset(path); }
                selectedIndex -= 1;
            }
            GUIUtility.ExitGUI();
        }
        GUILayout.EndHorizontal();
        int size = serializedSpawner.FindProperty("waves").arraySize;
        if (size == 0)
        {
            return;
        }
        selectedIndex = Mathf.Clamp(selectedIndex, 0, size - 1);
        wave = spawner.GetWaveFromIndex(selectedIndex);

        if (wave == null)
            serializedWave = null;
        else if (serializedWave == null || serializedWave.targetObject != wave)
            serializedWave = new SerializedObject(wave);

        if (serializedWave == null)
            return;
        serializedWave.Update();
        SerializedProperty serializedValues = serializedWave.FindProperty("values");
        SerializedProperty serializedReward = serializedWave.FindProperty("waveReward");
        SerializedProperty serializedTime = serializedWave.FindProperty("time");

        if (serializedValues.arraySize < 1)
        {
            if (GUILayout.Button("create values"))
            {
                serializedValues.InsertArrayElementAtIndex(0);
                SerializedProperty newValues = serializedValues.GetArrayElementAtIndex(0);
                newValues.FindPropertyRelative("enemyType").objectReferenceValue = null;
                newValues.FindPropertyRelative("count").intValue = 1;
                newValues.FindPropertyRelative("delay").intValue = 0;
                newValues.FindPropertyRelative("interval").floatValue = 1f;
                wave.waveReward = 0;
            }
        }
        for (int i = 0; i < serializedValues.arraySize; i++)
        {
            EditorGUILayout.Space(6);
            Rect line = EditorGUILayout.GetControlRect(false, 1);
            EditorGUI.DrawRect(line, new Color(0.5f, 0.5f, 0.5f, 0.6f));
            EditorGUILayout.Space(2);
            GUILayout.BeginHorizontal();
            bool deleteClicked = GUILayout.Button("delete values");
            bool insertClicked = GUILayout.Button("insert values below");
            GUILayout.EndHorizontal();
            if (deleteClicked)
            {
                serializedValues.DeleteArrayElementAtIndex(i);
                break;
            }
            if (insertClicked)
            {
                serializedValues.GetArrayElementAtIndex(i).DuplicateCommand();
                break;
            }

            SerializedProperty element = serializedValues.GetArrayElementAtIndex(i);
            SerializedProperty end = element.GetEndProperty(true);
            SerializedProperty child = element.Copy();

            bool enterChildren = true;
            while (child.Next(enterChildren) && !SerializedProperty.EqualContents(child, end))
            {
                EditorGUILayout.PropertyField(child, true);
                enterChildren = false;
            }
        }
        EditorGUILayout.Space(6);
        Rect line2 = EditorGUILayout.GetControlRect(false, 1);
        EditorGUI.DrawRect(line2, new Color(0.5f, 0.5f, 0.5f, 0.6f));
        EditorGUILayout.Space(2);
        EditorGUILayout.PropertyField(serializedReward, true);
        EditorGUILayout.PropertyField(serializedTime, true);
        serializedWave.ApplyModifiedProperties();

        EditorGUILayout.EndScrollView();
    }

    private string[] PopupList() // sorts the names of the list on instantiation for the popup.
    {
        SerializedProperty wavesArray = serializedSpawner.FindProperty("waves");
        if (wavesArray.arraySize == 0)
        {
            return new string[1] {"Empty"};
        }
        string[] waveNames = new string[wavesArray.arraySize];
        for (int i = 0; i < waveNames.Length; i++)
        {
            if (wavesArray.GetArrayElementAtIndex(i).objectReferenceValue != null)
                waveNames[i] = "wave " + (i+1);
            else
                waveNames[i] = "wave " + (i+1) + " (broken)";
        }
        return waveNames;
    }
}
