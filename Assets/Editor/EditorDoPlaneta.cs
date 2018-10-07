using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Planeta))]
public class EditorDoPlaneta : Editor {

    Planeta planeta;
    Editor editorDeForma;
    Editor editorDeCor;

    public override void OnInspectorGUI()
    {
        using (var check = new EditorGUI.ChangeCheckScope())
        {
            base.OnInspectorGUI();
            if(check.changed)
            {
                planeta.GeraPlaneta();
            }
        }

        if(GUILayout.Button("Gera Planeta"))
        {
            planeta.GeraPlaneta();
        }
        DrawSettingsEditor(planeta.shapeSettings, planeta.OnShapeSettingsUpdated, ref planeta.shapeSettingsFoldout, ref editorDeForma);
        DrawSettingsEditor(planeta.colourSettings, planeta.OnColourSettingsUpdated, ref planeta.colourSettingsFoldout, ref editorDeCor);
    }

    void DrawSettingsEditor(Object settings, System.Action onSettingsUpdated, ref bool foldout, ref Editor editor)
    {
        if (settings != null)
        {
            foldout = EditorGUILayout.InspectorTitlebar(foldout, settings);
            using (var check = new EditorGUI.ChangeCheckScope())
            {

                if (foldout)
                {
                    CreateCachedEditor(settings, null, ref editor);
                    editor.OnInspectorGUI();
                    if (check.changed)
                    {
                        if (onSettingsUpdated != null)
                        {
                            onSettingsUpdated();
                        }
                    }
                }
            }
        }
    }

    private void OnEnable()
    {
        planeta = (Planeta)target;
    }

}
