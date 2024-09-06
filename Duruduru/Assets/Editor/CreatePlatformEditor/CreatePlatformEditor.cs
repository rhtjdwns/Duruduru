using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(CreatePlatform))]
public class CreatePlatformEditor : Editor
{
    public VisualTreeAsset TreeAsset;
    private CreatePlatform _createPlatform;

    [MenuItem("Tools/Create Platform")]
    public static void Create()
    {
        
        GameObject prefab = Resources.Load<GameObject>("Prefabs/Buff/Platform/TempPlatform");
        Instantiate(prefab);
    }

    public override VisualElement CreateInspectorGUI()
    {
        if (!TreeAsset)
            return base.CreateInspectorGUI();

        _createPlatform = (CreatePlatform)target;

        VisualElement root = new VisualElement();
        TreeAsset.CloneTree(root);

        // Add your UI content here
        var inputMScript = root.Q<ObjectField>("unity-input-m_Script");
        inputMScript.AddToClassList("unity-disabled");
        inputMScript.Q(null, "unity-object-field__selector")?.SetEnabled(false);
        // root.Q<Label>("title").text = "Custom Property Drawer";

        var createBtn = root.Q<Button>("Button_Create");
        var deleteBtn = root.Q<Button>("Button_Delete");

        createBtn.clickable.clicked += () =>
        {
            _createPlatform.Create();
        };
        deleteBtn.clickable.clicked += () =>
        {
            _createPlatform.Delete();
        };

        var autoToggle = root.Q<Toggle>("Auto");
        var autoGroupBox = root.Q<VisualElement>("AutoGroupBox");
        var notAutoGroupBox = root.Q<VisualElement>("NotAutoGroupBox");

        autoToggle.RegisterValueChangedCallback((value) => 
        {
            if (value.newValue)
            {
                autoGroupBox.style.visibility = Visibility.Visible;
                notAutoGroupBox.style.visibility = Visibility.Hidden;
            }
            else
            {
                autoGroupBox.style.visibility = Visibility.Hidden;
                notAutoGroupBox.style.visibility = Visibility.Visible;
            }
        });

       

        return root;
    }
}