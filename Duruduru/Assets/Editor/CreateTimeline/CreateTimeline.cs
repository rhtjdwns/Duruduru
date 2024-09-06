using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class CreateTimeline : EditorWindow
{
    [MenuItem("Tools/Create Player Timeline")]
    public static void Create()
    {
        Transform spawnPoint = FindObjectOfType<TimelineManager>().transform;
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Resources/Prefabs/Timeline/TempTimeline.prefab");
        Instantiate(prefab, spawnPoint);
    }
}
