using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PositionBorders))]
public class PositionBordersEditor : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        PositionBorders positionBorders = (PositionBorders)target;

        if (GUILayout.Button("Position Object")) {
            positionBorders.PositionObject();
        }

        if (GUILayout.Button("Destroy All")) {
            positionBorders.DestroyAll();
        }
    }
}
