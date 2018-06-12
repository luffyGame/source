// © 2015 Mario Lelas
#define EDITOR_UTILS_INFO
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
/// <summary>
/// control axis information
/// </summary>
public class InputAxis
{
    public enum AxisType
    {
        KeyOrMouseButton = 0,
        MouseMovement = 1,
        JoystickAxis = 2
    };

    public string name;
    public string descriptiveName;
    public string descriptiveNegativeName;
    public string negativeButton;
    public string positiveButton;
    public string altNegativeButton;
    public string altPositiveButton;

    public float gravity;
    public float dead;
    public float sensitivity;

    public bool snap = false;
    public bool invert = false;

    public AxisType type;

    public int axis;
    public int joyNum;
}

/// <summary>
/// editor utilities
/// </summary>
public class EditorUtils
{
    /// <summary>
    /// add tag
    /// </summary>
    /// <param name="tag">tag name</param>
    public static void AddTag(string tag)
    {
        UnityEngine.Object[] asset = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset");
        if ((asset != null) && (asset.Length > 0))
        {
            SerializedObject so = new SerializedObject(asset[0]);
            SerializedProperty tags = so.FindProperty("tags");

            for (int i = 0; i < tags.arraySize; ++i)
            {
                if (tags.GetArrayElementAtIndex(i).stringValue == tag)
                {
#if EDITOR_UTILS_INFO
                    Debug.Log("tag " + tag + " already defined");
#endif
                    return;     // Tag already present, nothing to do.
                }
            }


            int numTags = tags.arraySize;
            tags.InsertArrayElementAtIndex(numTags);
            tags.GetArrayElementAtIndex(numTags).stringValue = tag;
            so.ApplyModifiedProperties();
            so.Update();

            Debug.Log("tag ' " + tag + "' added at index " + (numTags) + ".");
        }
    }

    /// <summary>
    /// set layer at index
    /// </summary>
    /// <param name="layer"></param>
    /// <param name="index"></param>
    public static void AddLayer(string layer, int index)
    {
        UnityEngine.Object[] asset = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset");
        if ((asset != null) && (asset.Length > 0))
        {
            SerializedObject so = new SerializedObject(asset[0]);
            SerializedProperty layers = so.FindProperty("layers");

            if (layers.GetArrayElementAtIndex(index).stringValue == layer)
            {
#if EDITOR_UTILS_INFO
                Debug.Log("layer " + layer + " already defined");
#endif
                return;     // layer already present, nothing to do.
            }

            layers.InsertArrayElementAtIndex(index);
            layers.GetArrayElementAtIndex(index).stringValue = layer;
            so.ApplyModifiedProperties();
            so.Update();

            Debug.Log("layer ' " + layer + "' added at index " + index + ".");
        }
    }

    /// <summary>
    /// add input axis
    /// </summary>
    /// <param name="axis"></param>
    public static void AddAxis(InputAxis axis)
    {
        if (AxisDefined(axis.name))
        {
#if EDITOR_UTILS_INFO
            Debug.Log("axis '" + axis.name + "' already defined.");
#endif
            return;
        }

        SerializedObject serializedObject = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0]);
        SerializedProperty axesProperty = serializedObject.FindProperty("m_Axes");

        axesProperty.arraySize++;
        serializedObject.ApplyModifiedProperties();

        SerializedProperty axisProperty = axesProperty.GetArrayElementAtIndex(axesProperty.arraySize - 1);

        GetChildProperty(axisProperty, "m_Name").stringValue = axis.name;
        GetChildProperty(axisProperty, "descriptiveName").stringValue = axis.descriptiveName;
        GetChildProperty(axisProperty, "descriptiveNegativeName").stringValue = axis.descriptiveNegativeName;
        GetChildProperty(axisProperty, "negativeButton").stringValue = axis.negativeButton;
        GetChildProperty(axisProperty, "positiveButton").stringValue = axis.positiveButton;
        GetChildProperty(axisProperty, "altNegativeButton").stringValue = axis.altNegativeButton;
        GetChildProperty(axisProperty, "altPositiveButton").stringValue = axis.altPositiveButton;
        GetChildProperty(axisProperty, "gravity").floatValue = axis.gravity;
        GetChildProperty(axisProperty, "dead").floatValue = axis.dead;
        GetChildProperty(axisProperty, "sensitivity").floatValue = axis.sensitivity;
        GetChildProperty(axisProperty, "snap").boolValue = axis.snap;
        GetChildProperty(axisProperty, "invert").boolValue = axis.invert;
        GetChildProperty(axisProperty, "type").intValue = (int)axis.type;
        GetChildProperty(axisProperty, "axis").intValue = axis.axis - 1;
        GetChildProperty(axisProperty, "joyNum").intValue = axis.joyNum;

        serializedObject.ApplyModifiedProperties();

        Debug.Log("axis ' " + axis.name + "' added.");
    }

    /// <summary>
    /// get child property
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    private static SerializedProperty GetChildProperty(SerializedProperty parent, string name)
    {
        SerializedProperty child = parent.Copy();
        child.Next(true);
        do
        {
            if (child.name == name) return child;
        }
        while (child.Next(false));
        return null;
    }

    /// <summary>
    /// is axis already defined ?
    /// </summary>
    /// <param name="axisName"></param>
    /// <returns></returns>
    private static bool AxisDefined(string axisName)
    {
        SerializedObject serializedObject = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0]);
        SerializedProperty axesProperty = serializedObject.FindProperty("m_Axes");

        axesProperty.Next(true);
        axesProperty.Next(true);
        while (axesProperty.Next(false))
        {
            SerializedProperty axis = axesProperty.Copy();
            axis.Next(true);
            if (axis.stringValue == axisName) return true;
        }
        return false;
    }
}
#endif