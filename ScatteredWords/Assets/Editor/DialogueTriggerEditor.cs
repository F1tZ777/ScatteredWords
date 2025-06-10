using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(DialogueTrigger))]
public class DialogueTriggerEditor : Editor
{
    DialogueTrigger triggerScript;
    SerializedObject GetTarget;
    SerializedProperty ThisList, ThisGOIcon, ThisGOButtons;
    int ListSize;

    ReorderableList reorderableDialogueList;

    void OnEnable()
    {
        triggerScript = (DialogueTrigger)target;
        GetTarget = new SerializedObject(triggerScript);
        ThisList = GetTarget.FindProperty("dialogueSets");
        ThisGOIcon = GetTarget.FindProperty("dialogueIcon");
        ThisGOButtons = GetTarget.FindProperty("buttonsMinigame");

        // Setup reorderable list
        reorderableDialogueList = new ReorderableList(GetTarget, ThisList, true, true, true, true);

        reorderableDialogueList.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, "Ink JSON Dialogue Sets");
            
            // Prepare rect for int field on the right side (about 50px wide)
            Rect countRect = new Rect(rect.xMax - 55, rect.y + 2, 50, EditorGUIUtility.singleLineHeight);

            // Get current array size
            ListSize = ThisList.arraySize;

            // Draw int field aligned right
            int newSize = EditorGUI.IntField(countRect, ListSize);

            // Clamp size to 0 or positive only
            if (newSize < 0) newSize = 0;

            // If user changed the size, update the list
            if (newSize != ListSize)
            {
                while (newSize > ThisList.arraySize)
                    ThisList.InsertArrayElementAtIndex(ThisList.arraySize);

                while (newSize < ThisList.arraySize)
                    ThisList.DeleteArrayElementAtIndex(ThisList.arraySize - 1);
            }
        };

        reorderableDialogueList.elementHeightCallback = (int index) =>
        {
            var element = ThisList.GetArrayElementAtIndex(index);
            float lineHeight = EditorGUIUtility.singleLineHeight + 2;
            float height = lineHeight; // foldout height

            if (element.isExpanded)
            {
                // Count fields and add extra space if mergedID is shown
                SerializedProperty mergeBranchProp = element.FindPropertyRelative("mergeBranch");
                int visibleFields = 3; // inkJSON, showButton, mergeBranch

                if (mergeBranchProp.boolValue) visibleFields++;

                height += visibleFields * lineHeight + 10; // some padding
            }

            return height;
        };

        reorderableDialogueList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            var element = ThisList.GetArrayElementAtIndex(index);

            // Drag handle takes ~12 pixels on the left, so offset foldout start by about 15 pixels
            Rect foldoutRect = new Rect(rect.x + 15, rect.y, rect.width - 15, EditorGUIUtility.singleLineHeight);

            // Foldout
            element.isExpanded = EditorGUI.Foldout(
                foldoutRect,
                element.isExpanded,
                $"Element {index}",
                true);

            if (element.isExpanded)
            {
                EditorGUI.indentLevel++;

                float lineHeight = EditorGUIUtility.singleLineHeight + 2;
                Rect fieldRect = new Rect(rect.x, rect.y + lineHeight, rect.width, EditorGUIUtility.singleLineHeight);

                SerializedProperty inkJSONProp = element.FindPropertyRelative("inkJSON");
                SerializedProperty showButtonProp = element.FindPropertyRelative("showButton");
                SerializedProperty mergeBranchProp = element.FindPropertyRelative("mergeBranch");
                SerializedProperty mergedIDProp = element.FindPropertyRelative("mergedID");

                EditorGUI.PropertyField(fieldRect, inkJSONProp, new GUIContent("Ink JSON"));

                fieldRect.y += lineHeight;
                EditorGUI.PropertyField(fieldRect, showButtonProp);

                fieldRect.y += lineHeight;
                EditorGUI.PropertyField(fieldRect, mergeBranchProp);

                if (mergeBranchProp.boolValue)
                {
                    fieldRect.y += lineHeight;
                    EditorGUI.PropertyField(fieldRect, mergedIDProp);
                }

                EditorGUI.indentLevel--;
            }
        };
    }

    public override void OnInspectorGUI()
    {
        // Update list
        GetTarget.Update();

        EditorGUILayout.PropertyField(ThisGOIcon);
        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(ThisGOButtons);
        EditorGUILayout.Space();

        #region commented out Inspector Stuff
        //EditorGUILayout.LabelField("Ink JSON", EditorStyles.boldLabel);
        //ListSize = ThisList.arraySize;
        //EditorGUILayout.BeginHorizontal();

        //// Label takes normal label width (default is ~100 pixels)
        //EditorGUILayout.LabelField("Size", GUILayout.Width(EditorGUIUtility.labelWidth));

        //// Fill remaining space to right-align int field
        //GUILayout.FlexibleSpace();

        //// Int field limited to about 50 pixels (fits 3 digits)
        //ListSize = EditorGUILayout.IntField(ListSize, GUILayout.Width(50));

        //EditorGUILayout.EndHorizontal();

        //if (ListSize < 0) ListSize = 0;

        //EditorGUILayout.BeginHorizontal();

        //if (GUILayout.Button("Add New"))
        //{
        //    triggerScript.dialogueSets.Add(new InkDialogueSet());
        //}
        //if (GUILayout.Button("Remove Last"))
        //{
        //    ThisList.DeleteArrayElementAtIndex(ThisList.arraySize - 1);
        //    ListSize -= 1;
        //}

        //EditorGUILayout.EndHorizontal();

        //if (ListSize != ThisList.arraySize)
        //{
        //    while (ListSize > ThisList.arraySize)
        //    {
        //        ThisList.InsertArrayElementAtIndex(ThisList.arraySize);
        //    }
        //    while (ListSize < ThisList.arraySize)
        //    {
        //        ThisList.DeleteArrayElementAtIndex(ThisList.arraySize - 1);
        //    }
        //}

        //for (int i = 0; i < ThisList.arraySize; i++)
        //{
        //    //SerializedProperty ListRef = ThisList.GetArrayElementAtIndex(i);
        //    //SerializedProperty inkJSONProp = ListRef.FindPropertyRelative("inkJSON");
        //    //SerializedProperty showButtonProp = ListRef.FindPropertyRelative("showButton");
        //    //SerializedProperty mergeBranchProp = ListRef.FindPropertyRelative("mergeBranch");
        //    //SerializedProperty mergedIDProp = ListRef.FindPropertyRelative("mergedID");

        //    //if(DisplayField == 0)
        //    //{
        //    //    EditorGUILayout.PropertyField(inkJSONProp);
        //    //    EditorGUILayout.PropertyField(showButtonProp);
        //    //    EditorGUILayout.PropertyField(mergeBranchProp);
        //    //    if (mergeBranchProp.boolValue)
        //    //        EditorGUILayout.PropertyField(mergedIDProp);
        //    //}

        //    //if(GUILayout.Button("Remove This Item"))
        //    //{
        //    //    ThisList.DeleteArrayElementAtIndex(i);
        //    //}
        //    SerializedProperty elementProp = ThisList.GetArrayElementAtIndex(i);

        //    // Use foldout for each element — store foldout state in EditorPrefs or better in a dictionary, but for now use property.isExpanded
        //    elementProp.isExpanded = EditorGUILayout.Foldout(elementProp.isExpanded, $"Element {i}");

        //    if (elementProp.isExpanded)
        //    {
        //        EditorGUI.indentLevel++;

        //        SerializedProperty inkJSONProp = elementProp.FindPropertyRelative("inkJSON");
        //        SerializedProperty showButtonProp = elementProp.FindPropertyRelative("showButton");
        //        SerializedProperty mergeBranchProp = elementProp.FindPropertyRelative("mergeBranch");
        //        SerializedProperty mergedIDProp = elementProp.FindPropertyRelative("mergedID");

        //        EditorGUILayout.PropertyField(inkJSONProp);
        //        EditorGUILayout.PropertyField(showButtonProp);
        //        EditorGUILayout.PropertyField(mergeBranchProp);

        //        if (mergeBranchProp.boolValue)
        //            EditorGUILayout.PropertyField(mergedIDProp);

        //        if (GUILayout.Button("Remove This Item"))
        //        {
        //            ThisList.DeleteArrayElementAtIndex(i);
        //            // Avoid errors by applying and exiting early to update the array before continuing the loop
        //            GetTarget.ApplyModifiedProperties();
        //            return;
        //        }

        //        EditorGUI.indentLevel--;
        //    }
        //}
        #endregion

        reorderableDialogueList.DoLayoutList();

        GetTarget.ApplyModifiedProperties();
    }
}
