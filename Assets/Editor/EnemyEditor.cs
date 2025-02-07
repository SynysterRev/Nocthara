using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(Enemy))]
    public class EnemyEditor : UnityEditor.Editor
    {
        private Enemy _enemy;

        private void OnEnable()
        {
            _enemy = (Enemy)target;
        }

        private void OnSceneGUI()
        {
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
