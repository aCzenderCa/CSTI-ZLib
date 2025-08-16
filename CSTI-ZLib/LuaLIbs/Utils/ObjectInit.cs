using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CSTI_ZLib.LuaLIbs.Utils
{
    public static class ObjectInit<T>
        where T : ScriptableObject
    {
        private static readonly T? EmptyPrefab = ScriptableObject.CreateInstance<T>();

        static ObjectInit()
        {
            InitAll(EmptyPrefab, typeof(T));
        }

        private static void InitAll(object? o, Type type, int depth = 16)
        {
            if (depth < 0) return;
            if (o == null) return;

            var fieldInfos = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            foreach (var fieldInfo in fieldInfos)
            {
                if (!fieldInfo.IsPublic && fieldInfo.GetCustomAttributes().All(attribute => attribute is not SerializeField)) continue;
                var value = fieldInfo.GetValue(o);
                if (value == null)
                {
                    var subInit = true;
                    if (fieldInfo.FieldType.IsSubclassOf(typeof(ScriptableObject)))
                    {
                        value = ScriptableObject.CreateInstance(fieldInfo.FieldType);
                    }
                    else if (fieldInfo.FieldType.IsSubclassOf(typeof(Object)))
                    {
                        value = null;
                        subInit = false;
                    }
                    else if (fieldInfo.FieldType.IsEnum)
                    {
                        value = fieldInfo.FieldType.GetEnumValues().GetValue(0);
                        subInit = false;
                    }
                    else if (fieldInfo.FieldType == typeof(string))
                    {
                        value = "";
                        subInit = false;
                    }
                    else if (fieldInfo.FieldType.IsArray)
                    {
                        value = Array.CreateInstance(fieldInfo.FieldType.GetElementType()!, 0);
                        subInit = false;
                    }
                    else if (fieldInfo.FieldType.IsGenericType && fieldInfo.FieldType.GetGenericTypeDefinition() == typeof(List<>))
                    {
                        value = AccessTools.CreateInstance(fieldInfo.FieldType);
                        subInit = false;
                    }
                    else
                    {
                        value = JsonUtility.FromJson("{}", fieldInfo.FieldType);
                    }

                    subInit &= fieldInfo.FieldType != type;
                    if (subInit) InitAll(value, fieldInfo.FieldType, depth - 1);
                    if (value != null) fieldInfo.SetValue(o, value);
                }
            }
        }

        public static T Empty => Object.Instantiate(EmptyPrefab)!;
    }
}