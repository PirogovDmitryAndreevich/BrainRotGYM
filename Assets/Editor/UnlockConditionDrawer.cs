using UnityEditor;
using UnityEngine;
using System;
using System.Linq;

[CustomPropertyDrawer(typeof(UnlockCondition), true)]
public class UnlockConditionDrawer : PropertyDrawer
{
    private static Type[] _conditionTypes;

    static UnlockConditionDrawer()
    {
        // Находим все классы, наследующие UnlockCondition
        _conditionTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => t.IsSubclassOf(typeof(UnlockCondition)) && !t.IsAbstract)
            .ToArray();
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.managedReferenceValue == null)
        {
            DrawTypeSelector(position, property);
            return;
        }

        var type = property.managedReferenceValue.GetType();
        var typeName = type.Name;

        var foldoutRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, typeName, true);

        if (property.isExpanded)
        {
            EditorGUI.indentLevel++;
            var bodyRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + 2, position.width, EditorGUIUtility.singleLineHeight);
            var iterator = property.Copy();
            var depth = iterator.depth;

            iterator.NextVisible(true);
            while (iterator.depth > depth)
            {
                EditorGUI.PropertyField(bodyRect, iterator, true);
                bodyRect.y += EditorGUI.GetPropertyHeight(iterator, true) + 2;
                if (!iterator.NextVisible(false))
                    break;
            }

            // кнопка смены типа
            var buttonRect = new Rect(bodyRect.x, bodyRect.y + 2, bodyRect.width, EditorGUIUtility.singleLineHeight);
            if (GUI.Button(buttonRect, "Изменить тип условия"))
            {
                property.managedReferenceValue = null;
            }

            EditorGUI.indentLevel--;
        }
    }

    private void DrawTypeSelector(Rect position, SerializedProperty property)
    {
        var names = _conditionTypes.Select(t => t.Name).ToArray();
        var index = EditorGUI.Popup(position, "Тип условия", -1, names);

        if (index >= 0)
        {
            var selectedType = _conditionTypes[index];
            property.managedReferenceValue = Activator.CreateInstance(selectedType);
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (property.managedReferenceValue == null)
            return EditorGUIUtility.singleLineHeight * 1.2f;

        if (!property.isExpanded)
            return EditorGUIUtility.singleLineHeight * 1.2f;

        float height = EditorGUIUtility.singleLineHeight * 2f;
        var iterator = property.Copy();
        var depth = iterator.depth;

        iterator.NextVisible(true);
        while (iterator.depth > depth)
        {
            height += EditorGUI.GetPropertyHeight(iterator, true) + 2;
            if (!iterator.NextVisible(false))
                break;
        }

        return height;
    }
}
