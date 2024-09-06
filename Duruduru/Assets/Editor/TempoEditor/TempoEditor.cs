using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class TempoEditor : EditorWindow
{
    private VisualElement _mainVisualElement;
    
    private Dictionary<Button, TempoAttackData> _tempoList = new Dictionary<Button, TempoAttackData>();

    private TempoAttackData _currentData;

    [MenuItem("Tools/Tempo Setting")]
    public static void ShowExample()
    {
        TempoEditor wnd = GetWindow<TempoEditor>();
        //�ҷ��ɴϴ�.
        wnd.titleContent = new GUIContent("Tempo Setting");
        Vector2 size = new Vector2(400f, 300f);
        wnd.minSize = size;
        wnd.maxSize = size;
        //������ ����
    }

    private void CreateGUI()
    {
        VisualElement root = rootVisualElement; // �ֻ��� ȭ���� ���� root�� �����մϴ�

        VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>
            ("Assets/Editor/TempoEditor/TempoEditor.uxml");
        root.Add(visualTree.Instantiate()); // uxml�� �ҷ��ɴϴ�.

        _mainVisualElement = root.Q<ScrollView>("MainVisualElement");
        _mainVisualElement.style.visibility = Visibility.Hidden;     

        AddData("MainTempo1");
        AddData("MainTempo2");
        AddData("MainTempo3");
        AddData("MainTempo4");
        AddData("PointTempo1");
        AddData("PointTempo2");

        InitData();
    }

    private void AddData(string name)
    {

        string[] datas = Directory.GetFiles("Assets/Resources/ScriptableObjects/TempoAttackDatas", $"{name}.asset",
            SearchOption.AllDirectories);

        foreach (string data in datas)
        {
            
            string cleanedPath = data.Replace("\\", "/"); // \\�� / �� �ٲ��ݴϴ�. Assets\\Data �� �ƴ� Assets/Data �� ���⶧����
            _tempoList.Add(rootVisualElement.Q<Button>(name), (TempoAttackData)AssetDatabase.LoadAssetAtPath(cleanedPath,
                typeof(TempoAttackData)));

        }
    }

    private void InitData()
    {
        var slowSlider = rootVisualElement.Q<MinMaxSlider>("SlowRangth");

        foreach (var t in _tempoList)
        {
            
            t.Key.clickable.clicked += () =>
            {
                _currentData = t.Value;
                UpdateData();
                
            };

          
        }
    }
 
    private void UpdateData()
    {

        rootVisualElement.Q<Label>("TempoTitle").text = _currentData.name;

        SerializedObject so = new SerializedObject(_currentData);
        _mainVisualElement.Bind(so);
        //���ε����ݴϴ�.
        if (_currentData.type == Define.TempoType.MAIN)
        {
            rootVisualElement.Q<FloatField>("PerfectStamina").label = "���¹̳�";
            rootVisualElement.Q<FloatField>("GoodStamina").style.visibility = Visibility.Hidden;

            rootVisualElement.Q<FloatField>("PerfectDamage").label = "������";
            rootVisualElement.Q<FloatField>("GoodDamage").style.visibility = Visibility.Hidden;
        }
        else
        {
            rootVisualElement.Q<FloatField>("PerfectStamina").label = "Perfect ���¹̳�";
            rootVisualElement.Q<FloatField>("GoodStamina").style.visibility = Visibility.Visible;

            rootVisualElement.Q<FloatField>("PerfectDamage").label = "Perfect ������";
            rootVisualElement.Q<FloatField>("GoodDamage").style.visibility = Visibility.Visible;
        }

        _mainVisualElement.style.visibility = Visibility.Visible;


    }
  
}
