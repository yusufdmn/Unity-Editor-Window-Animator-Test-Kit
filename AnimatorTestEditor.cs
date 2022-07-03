using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;

public class AnimatorTestEditor : EditorWindow
{

    [MenuItem("GameObject/AnimatorTestEditor",false,10)]
    static void Init()
    {
        if(Selection.count == 1)
        {
            EditorWindow window = GetWindow<AnimatorTestEditor>("Animator Test");
            window.Show();
        }
    }

    [MenuItem("GameObject/AnimatorTestEditor", true,10)]
    static bool Test()
    {
        if(Selection.count == 1)
        {                                     // It can be open only when selected gameobject has animator and  in PLAY MODE
            if (Selection.activeGameObject.GetComponent<Animator>() != null  && Application.isPlaying)
                return true;
        }

        return false;
    }



    GameObject _gameObject;
    AnimatorControllerParameter[] parameters;
    Animator animator;

    ArrayList parametersInt;
    ArrayList parametersFloat;
    ArrayList parametersBool;
    ArrayList parametersTrigger;

    GUIStyle style = new GUIStyle();
    Vector2 scrollPos;
    

    private void Awake()        // Get the parameters of the animator and keep them in different lists to display them in order later (Int, float, bool, trigger)
    {
        parameters = null;

        parametersInt = new ArrayList();
        parametersFloat = new ArrayList();
        parametersBool = new ArrayList();
        parametersTrigger = new ArrayList();

        _gameObject = Selection.activeGameObject;


        if(_gameObject.GetComponent<Animator>() != null)
        {
            animator = _gameObject.GetComponent<Animator>();
            parameters = animator.parameters;

            foreach (AnimatorControllerParameter parameter in parameters)
            {
                if (parameter.type == AnimatorControllerParameterType.Trigger)
                    parametersTrigger.Add(parameter);

                else if (parameter.type == AnimatorControllerParameterType.Bool)
                    parametersBool.Add(parameter);

                else if (parameter.type == AnimatorControllerParameterType.Float)
                    parametersFloat.Add(parameter);

                else if (parameter.type == AnimatorControllerParameterType.Int)
                    parametersInt.Add(parameter);

            }
        }

    }

    private void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        
        SetLabelStyle();

        if (!Application.isPlaying) {       // Close the window when PLAY MODE is off
            this.Close();
            return;
        }

        if (Selection.activeGameObject != _gameObject)  // Change Parameters if another object is selected 
            Awake();

        if (_gameObject != null)        // Create the required GUI in order
        {
            string titleIInt = "Int Parameters";
            AddTitleLabel(titleIInt, parametersInt,1);

            foreach (AnimatorControllerParameter parameterInt in parametersInt)     // Create int fields
            {
                CreateIntField(parameterInt.name);
            }

            string titleFloat = "Float Parameters";
            AddTitleLabel(titleFloat, parametersFloat,3);

            foreach (AnimatorControllerParameter parameterFloat in parametersFloat)     // Create float fields
            {
                CreateFloatSlider(parameterFloat.name);
            }

            string titleBool = "Bool Parameters";
            AddTitleLabel(titleBool, parametersBool, 3);

            foreach (AnimatorControllerParameter parameterBool in parametersBool)        // Create bool fields
            {
                CreateBoolToggle(parameterBool.name);
            }

            string titleTrigger = "Trigger Parameters";
            AddTitleLabel(titleTrigger, parametersTrigger, 3);

            foreach (AnimatorControllerParameter parameterTrigger in parametersTrigger)      // Create trigger fields
            {
                CreateTriggerButton(parameterTrigger.name);
            }

        }

        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }

    void SetLabelStyle()
    {
        style.alignment = TextAnchor.MiddleCenter;
        style.fontStyle = FontStyle.Bold;
        style.normal.textColor = Color.red;
        GUI.skin.label = style;
    }

    void AddTitleLabel(string title, ArrayList array, float space)
    {
        GUILayout.Space(space);
        if (array != null && array.Count > 0)
        {
            GUILayout.Label(title);
        }
        GUILayout.Space(1);
    }


    /*      "Create Required GUI" Functions        */


    void CreateFloatSlider(string name)
    {
        float parameter = animator.GetFloat(name);
        parameter = EditorGUILayout.Slider(name, parameter, 0, 10);
        SetFloatAnim(name, parameter);
    }

    void CreateIntField(string name)
    {
        int parameter = animator.GetInteger(name);
        parameter = EditorGUILayout.IntField(name, parameter);
        SetIntAnim(name, parameter);
    }


    void CreateBoolToggle(string name)
    {
        bool parameter = animator.GetBool(name);
        parameter = EditorGUILayout.Toggle(name, parameter);
        
        if (parameter == true)
        {
            SetBoolAnim(name, true);
        }
        else
        {
            SetBoolAnim(name, false);
        }
    }


    void CreateTriggerButton(string name)
    {
        if (GUILayout.Button(name))
        {
            SetTriggerAnim(name);
        }
    }


    /*      "Set Animation" Functions        */


    void SetTriggerAnim(string animName)
    {
        animator.SetTrigger(animName);
    }

    void SetBoolAnim(string animName, bool parameter)
    {
        animator.SetBool(animName, parameter);
    }

    void SetIntAnim(string name, int parameter)
    {
        animator.SetInteger(name, parameter);
    }

    void SetFloatAnim(string name, float parameter)
    {
        animator.SetFloat(name, parameter);
    }
}
