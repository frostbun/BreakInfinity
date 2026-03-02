#nullable enable
namespace BreakInfinity.Editor
{
    using UnityEditor;
    using UnityEngine;

    [CustomPropertyDrawer(typeof(BigDouble))]
    internal sealed class BigDoubleDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            const float GAP_WIDTH = 10;
            var mantissaRect = new Rect(position.x, position.y, (position.width - GAP_WIDTH) * 0.66f, position.height);
            var gapRect = new Rect(mantissaRect.x + mantissaRect.width, position.y, GAP_WIDTH, position.height);
            var exponentRect = new Rect(gapRect.x + gapRect.width, position.y, (position.width - GAP_WIDTH) * 0.33f, position.height);

            var mantissaProperty = property.FindPropertyRelative("<Mantissa>k__BackingField");
            var exponentProperty = property.FindPropertyRelative("<Exponent>k__BackingField");

            EditorGUI.BeginChangeCheck();
            var mantissa = EditorGUI.DoubleField(mantissaRect, mantissaProperty.doubleValue, new GUIStyle(EditorStyles.numberField)
            {
                alignment = TextAnchor.MiddleRight,
            });
            EditorGUI.LabelField(gapRect, "e");
            var exponent = EditorGUI.LongField(exponentRect, exponentProperty.longValue);

            if (EditorGUI.EndChangeCheck())
            {
                var normalized = BigDouble.Normalize(mantissa, exponent);
                mantissaProperty.doubleValue = normalized.Mantissa;
                exponentProperty.longValue = normalized.Exponent;
            }

            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }
    }
}