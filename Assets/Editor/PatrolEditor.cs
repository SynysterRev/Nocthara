using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(Enemy))]
    public class PatrolEditor : UnityEditor.Editor
    {
        private SerializedProperty _wayPoints;
        private List<Vector2> _newTargetPos = new List<Vector2>();
        private Enemy _enemy;

        private void OnEnable()
        {
            _wayPoints = serializedObject.FindProperty("WayPoints");
            _enemy = (Enemy)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();
            EditorGUILayout.PropertyField(_wayPoints);
            if (_newTargetPos.Count != _wayPoints.arraySize)
            {
                _newTargetPos.Clear();
                for (int i = 0; i < _wayPoints.arraySize; ++i)
                {
                    _newTargetPos.Add(_wayPoints.GetArrayElementAtIndex(i).vector2Value);
                }
            }

            // for (int i = 0; i < _wayPoints.arraySize; ++i)
            // {
            //     _wayPoints.GetArrayElementAtIndex(i).vector2Value = _newTargetPos[i];
            // }
            serializedObject.ApplyModifiedProperties();
        }

        private void OnSceneGUI()
        {
            _newTargetPos = _enemy.WayPoints;
            for (int i = 0; i < _newTargetPos.Count; ++i)
            {
                _newTargetPos[i] = Handles.PositionHandle(_newTargetPos[i], Quaternion.identity);
                Handles.color = Color.black;
                Handles.DrawWireDisc(_newTargetPos[i], Vector3.forward, 0.1f, 15.0f);
                Handles.Label(_newTargetPos[i], i.ToString());
            }
            Handles.color = Color.white;
            var position = _enemy.transform.position;
            Handles.DrawWireArc(position, Vector3.forward, Vector3.up, 360.0f, _enemy.ViewRange);
            
            Vector3 viewAngleA = _enemy.DirFromAngle (-_enemy.ViewAngle / 2);
            Vector3 viewAngleB = _enemy.DirFromAngle (_enemy.ViewAngle / 2);
            Handles.DrawLine (position, position + viewAngleA * _enemy.ViewRange);
            Handles.DrawLine (position, position + viewAngleB * _enemy.ViewRange);
            EditorUtility.SetDirty(_enemy);
        }
    }
}
