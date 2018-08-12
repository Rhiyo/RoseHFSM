using UnityEngine;
using System.Collections;
using UnityEditor;

namespace RoseHFSM
{
    /// <summary>
    /// For adding NodeEditor button.
    /// </summary>
    [CustomEditor(typeof(Behaviour))]
    public class BehaviourEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            //ObjectBuilderScript myScript = (ObjectBuilderScript)target;
            if (GUILayout.Button("Node Editor"))
            {
                NodeEditor editor = EditorWindow.GetWindow<NodeEditor>();
                editor.UpdateNodes();
            }
        }
    }
}