using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomPropertyDrawer(typeof(MaxStringLengthAttribute))]
    public class MaxStringLengthDrawer : PropertyDrawer
    {
        private GUIStyle _textAreaStyle;
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            MaxStringLengthAttribute maxLengthAttribute = (MaxStringLengthAttribute)attribute;

            if (property.propertyType == SerializedPropertyType.String)
            {
                EditorGUI.BeginProperty(position, label, property);
                
                if (_textAreaStyle == null)
                {
                    _textAreaStyle = new GUIStyle(EditorStyles.textArea)
                    {
                        wordWrap = true // Permet le retour automatique à la ligne
                    };
                }

                // Dessiner le label
                position = EditorGUI.PrefixLabel(position, label);

                // Définir la hauteur dynamique du TextArea
                position.height = EditorGUIUtility.singleLineHeight * 4; // Simule [TextArea(1,3)]
                // position.width = EditorGUIUtility.fieldWidth * 5;

                // Affichage du TextArea
                EditorGUI.BeginChangeCheck();
                string newValue = EditorGUI.TextArea(position, property.stringValue, _textAreaStyle);

                // Empêcher l'utilisateur d'ajouter plus de caractères que la limite
                if (newValue.Length > maxLengthAttribute.MaxLength)
                {
                    newValue = newValue.Substring(0, maxLengthAttribute.MaxLength);
                    GUI.FocusControl(null); // Retire le focus pour stopper l'input immédiatement
                }

                if (EditorGUI.EndChangeCheck())
                {
                    property.stringValue = newValue;
                }

                EditorGUI.EndProperty();
            }
            else
            {
                EditorGUI.LabelField(position, label.text, "Use MaxStringLength with string.");
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 4; // Simule la hauteur d'un TextArea
        }
    }
}