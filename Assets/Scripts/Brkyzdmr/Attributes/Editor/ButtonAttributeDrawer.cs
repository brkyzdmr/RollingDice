using System;
using System.Collections.Generic;
using System.Reflection;
using Brkyzdmr.Helpers;
using UnityEditor;
using UnityEngine;

namespace Brkyzdmr.Attributes.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(UnityEngine.Object), true, isFallback = true)]
    public class ButtonEditor : UnityEditor.Editor
    {
        private InternalData _internalData;

        private void OnEnable()
        {
            _internalData ??= new InternalData(target.GetType());
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EditorGUILayout.Space();
            for (var i = 0; i < _internalData.ValidMethods.Count; i++)
            {
                var button = _internalData.Buttons[i];
                var method = _internalData.ValidMethods[i];

                if (button.hideInPlayMode && Application.isPlaying ||
                    button.hideInEditMode && !Application.isPlaying)
                    continue;

                var buttonName = !string.IsNullOrEmpty(button.name) ? button.name : method.Name.SplitPascalCase();

                if (GUILayout.Button(buttonName))
                {
                    foreach (var t in targets)
                    {
                        method.Invoke(t, null);
                        EditorUtility.SetDirty(t);
                    }
                }
            }
        }

        private class InternalData
        {
            public readonly List<MethodInfo> ValidMethods = new();
            public readonly List<ButtonAttribute> Buttons = new();
            public Type ObjType;

            public InternalData(Type objType) => Initialize(objType);

            private void Initialize(Type objType)
            {
                ObjType = objType;

                foreach (var method in objType.GetMethods(BindingFlags.Instance | BindingFlags.Public |
                                                          BindingFlags.NonPublic))
                {
                    var buttonAttribute = method.GetCustomAttribute<ButtonAttribute>();
                    if (buttonAttribute != null && method.ReturnType == typeof(void) &&
                        method.GetParameters().Length == 0)
                    {
                        ValidMethods.Add(method);
                        Buttons.Add(buttonAttribute);
                    }
                }
            }
        }
    }
}