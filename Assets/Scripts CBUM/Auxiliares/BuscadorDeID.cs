﻿using UnityEngine;
using System.Reflection;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class BuscadorDeID
{
    public static void Validate(ref string ID,MonoBehaviour m)
    {
    #if UNITY_EDITOR
        Event e = Event.current;
        bool foi = false;
        if (e != null)
        {
            foi = e.commandName == "Duplicate" || e.commandName == "Paste";
            //Debug.Log(e.commandName);
        }

        if ((ID == "0" ||ID=="" || foi) && m.gameObject.scene.name != null)
        {

            ID = m.GetInstanceID() + "_" + m.gameObject.scene.name;
            SetUniqueIdProperty(m, ID, "ID");
        }
    #endif
    }

    public static string GetUniqueID(GameObject G, string id)
    {
#if UNITY_EDITOR
        PropertyInfo inspectorModeInfo =
    typeof(SerializedObject).GetProperty("inspectorMode", BindingFlags.NonPublic | BindingFlags.Instance);

        SerializedObject serializedObject = new SerializedObject(G.transform);
        inspectorModeInfo.SetValue(serializedObject, InspectorMode.Debug, null);

        SerializedProperty localIdProp =
            serializedObject.FindProperty("m_LocalIdentfierInFile");   //note the misspelling!

        int localId = localIdProp.intValue;

        return localId.ToString();
#endif
        return id;
    }

    public static void SetUniqueIdProperty(Object o, string id, string nomeProperty)
    {
#if UNITY_EDITOR
        
        PropertyInfo inspectorModeInfo =
    typeof(SerializedObject).GetProperty("inspectorMode", BindingFlags.NonPublic | BindingFlags.Instance);

        SerializedObject serializedObject = new SerializedObject(o);
        inspectorModeInfo.SetValue(serializedObject, InspectorMode.Debug, null);

        SerializedProperty localIdProp =
            serializedObject.FindProperty(nomeProperty);

        Debug.Log(localIdProp.stringValue+" : "+((MonoBehaviour)o).gameObject);
        localIdProp.stringValue = id;
#endif
    }


}