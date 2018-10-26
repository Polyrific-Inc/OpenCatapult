// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;

namespace Polyrific.Catapult.Cli.Extensions
{
    public static class CatapultCliExtensions
    {
        public static string ToCliString<T>(this T obj, string openingLine = "", string[] obfuscatedFields = null, int indentation = 1)
        {
            string indentationString = String.Concat(Enumerable.Repeat("  ", indentation));

            if (obj is string stringValue)
                return $"{indentationString}{stringValue}";

            PropertyInfo[] propertyInfos = null;
            propertyInfos = obj.GetType().GetProperties();

            var sb = new StringBuilder(openingLine);
            sb.AppendLine();
            foreach (var item in propertyInfos)
            {
                var prop = item.GetValue(obj);

                if (prop == null)
                {
                    sb.AppendLine($"{indentationString}{item.Name}: NULL");
                }
                else if (prop is Dictionary<string, string> propDictionary)
                {
                    sb.AppendLine($"{indentationString}{item.Name}:");
                    foreach (var dictItem in propDictionary)
                    {
                        sb.AppendLine($"{indentationString}  {dictItem.Key}: {GetDisplayValue(dictItem.Key, dictItem.Value, obfuscatedFields)}");
                    }
                }
                else if (prop is IEnumerable enumProp && !(prop is string))
                {
                    sb.AppendLine(enumProp.ToListCliString($"{indentationString}{item.Name}:", obfuscatedFields, indentation));
                }
                else
                {
                    sb.AppendLine($"{indentationString}{item.Name}: {GetDisplayValue(item.Name, prop.ToString(), obfuscatedFields)}");
                }
            }

            return sb.ToString();
        }

        public static string ToListCliString(this IEnumerable list, string openingLine = "", string[] obfuscatedFields = null, int indentation = 0)
        {
            var sb = new StringBuilder(openingLine);
            sb.AppendLine();

            bool empty = true;
            foreach (var listitem in list)
            {
                empty = false;
                sb.Append(listitem.ToCliString("", obfuscatedFields, indentation + 1));
            }
            
            if (empty)
            {
                string indentationString = String.Concat(Enumerable.Repeat("  ", indentation));
                sb.Append($"{indentationString}No item found");
            }

            return sb.ToString();
        }

        public static string ToJson(this (string,string)[] option)
        {
            var dict = option.ToDictionary(o => o.Item1, o => o.Item2);
            return JsonConvert.SerializeObject(dict);
        }

        public static void Merge(this Dictionary<string, string> target, Dictionary<string, string> source)
        {
            foreach ((string key, string value) in source)
            {
                if (!target.TryGetValue(key, out var currentValue))
                {
                    target.Add(key, value);
                }
                else
                {
                    target[key] = value;
                }
            }
        }

        private static string GetDisplayValue(string name, string value, string[] obfuscatedFields)
        {
            return !string.IsNullOrEmpty(value) && (obfuscatedFields?.Contains(name) ?? false) ? "****" : value;
        }
    }
}
