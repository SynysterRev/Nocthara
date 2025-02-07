using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(WayPoint))]
    public class WayPointEditor : UnityEditor.Editor
    {
        private SerializedProperty _wayPoints;
        private List<Vector2> _newTargetPos;
        private WayPoint _wayPoint;

        private void OnEnable()
        {
            _wayPoints = serializedObject.FindProperty("WayPoints");
            _wayPoint = (WayPoint)target;
            _newTargetPos = new List<Vector2>();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(_wayPoints);
            if (EditorGUI.EndChangeCheck())
            {
                if (_newTargetPos.Count != _wayPoints.arraySize)
                {
                    bool wasEmpty = _newTargetPos.Count == 0;
                    _newTargetPos.Clear();
                    for (int i = 0; i < _wayPoints.arraySize; ++i)
                    {
                        if (wasEmpty && i == 0)
                        {
                            _wayPoints.GetArrayElementAtIndex(i).vector2Value = _wayPoint.transform.position;
                        }

                        _newTargetPos.Add(_wayPoints.GetArrayElementAtIndex(i).vector2Value);
                    }
                }

                serializedObject.ApplyModifiedProperties();
            }
        }

        private void OnSceneGUI()
        {
            _newTargetPos = _wayPoint.WayPoints;
            for (int i = 0; i < _newTargetPos.Count; ++i)
            {
                _newTargetPos[i] = Handles.PositionHandle(_newTargetPos[i], Quaternion.identity);
                Handles.color = Color.black;
                Handles.DrawWireDisc(_newTargetPos[i], Vector3.forward, 0.1f, 15.0f);
                Handles.Label(_newTargetPos[i], i.ToString());
            }
        }
    }
}