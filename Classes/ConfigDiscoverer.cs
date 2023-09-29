using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EMCL.Classes
{
    public static class ConfigDiscoverer
    {
        public static List<object?> GetAllFieldValues<T>(T obj) where T : class
        {
            FieldInfo[] fields = typeof(T).GetFields();
            List<object?> values = fields.Select(f => f.GetValue(obj)).ToList();
            if (values != null)
            {
                return values;
            }
            else
            {
                return new List<object?>();
            }
        }

        public static List<string> GetAllFieldKeys<T>(T obj) where T : class
        {
            List<string> fieldNames = new List<string>();
            List<string> values = typeof(T).GetFields().Select(f => f.Name).ToList();
            if (values != null)
            {
                return values;
            }
            else
            {
                return new List<string>();
            }
        }

        public static List<ValueTuple<string, object?>> GetAllField<T>(T obj) where T : class
        {
            List<ValueTuple<string, object?>> returns = new List<ValueTuple<string, object?>>();
            List<string> keys = GetAllFieldKeys(obj);
            List<object?> values = GetAllFieldValues(obj);
            if (keys.Count == values.Count)
            {
                for (int i = 0; i < keys.Count; i++)
                {
                    returns.Add(new ValueTuple<string, object?>(keys[i], values[i]));
                }
            }
            return returns;
        }

        public static void SetFieldValue(object obj, string fieldName, object fieldValue)
        {
            Type type = obj.GetType();
            FieldInfo? fieldInfo = type.GetField(fieldName);

            if (fieldInfo != null)
            {
                fieldInfo.SetValue(obj, fieldValue);
            }
        }

        public static Type? GetFieldType(object obj, string fieldName)
        {
            Type type = obj.GetType();
            FieldInfo? fieldInfo = type.GetField(fieldName);

            if (fieldInfo != null)
            {
                return fieldInfo.FieldType;
            }

            return null;
        }

        public static object? ConvertObject(object value, Type targetType)
        {
            if (value == null)
            {
                return null;
            }

            if (targetType == typeof(object))
            {
                return value;
            }

            return Convert.ChangeType(value, targetType);
        }

        public static object? ConvertFieldValue(object obj, string fieldName, object fieldValue)
        {
            Type? fieldType = GetFieldType(obj, fieldName);

            if (fieldType == null)
            {
                return null;
            }

            if (fieldValue is string strValue && !string.IsNullOrEmpty(strValue))
            {
                if (fieldType == typeof(int))
                {
                    if (int.TryParse(strValue, out int value))
                    {
                        return value;
                    }
                }
                else if (fieldType == typeof(float))
                {
                    if (float.TryParse(strValue, out float value))
                    {
                        return value;
                    }
                }
                else if (fieldType == typeof(double))
                {
                    if (double.TryParse(strValue, out double value))
                    {
                        return value;
                    }
                }
                else if (fieldType == typeof(bool))
                {
                    if (bool.TryParse(strValue, out bool value))
                    {
                        return value;
                    }
                }
                else if (fieldType == typeof(long))
                {
                    if (long.TryParse(strValue, out long value))
                    {
                        return value;
                    }
                }
                else if (fieldType == typeof(string))
                {
                    if (strValue is string)
                    {
                        return (string)strValue;
                    }
                }
            }

            return null;
        }
    }
}
