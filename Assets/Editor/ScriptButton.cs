using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(LoadingDoors))]
public class ObjectBuilderEditor : Editor
{
   public override void OnInspectorGUI()
   {
      DrawDefaultInspector();

      LoadingDoors myScript = (LoadingDoors)target;
      if (GUILayout.Button("Open doors"))
      {
         myScript.OpenDoors();
      }
      if (GUILayout.Button("Close doors"))
      {
         myScript.CloseDoors();
      }
   }
}