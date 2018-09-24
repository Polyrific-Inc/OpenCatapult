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
        public static string ToCliString<T>(this T obj, string openingLine = "", string[] obfuscatedFields = null)
        {
            if (obj is string stringValue)
                return stringValue;

            PropertyInfo[] propertyInfos = null;
            propertyInfos = obj.GetType().GetProperties();

            var sb = new StringBuilder(openingLine);
            sb.AppendLine();
            foreach (var item in propertyInfos)
            {
                var prop = item.GetValue(obj);

                if (prop == null)
                {
                    sb.AppendLine($"{item.Name}: NULL");
                }
                else if (prop is Dictionary<string, string> propDictionary)
                {
                    sb.AppendLine($"{item.Name}:");
                    foreach (var dictItem in propDictionary)
                    {
                        sb.AppendLine($" {dictItem.Key}: {GetDisplayValue(dictItem.Key, dictItem.Value, obfuscatedFields)}");
                    }
                }
                else if (prop is IEnumerable enumProp && !(prop is string))
                {
                    sb.AppendLine(enumProp.ToListCliString($"{item.Name}:"));
                }
                else
                {
                    sb.AppendLine($"{item.Name}: {GetDisplayValue(item.Name, prop.ToString(), obfuscatedFields)}");
                }
            }

            return sb.ToString();
        }

        public static string ToListCliString(this IEnumerable list, string openingLine = "", string[] obfuscatedFields = null)
        {
            var sb = new StringBuilder(openingLine);
            sb.AppendLine();

            bool empty = true;
            foreach (var listitem in list)
            {
                empty = false;
                sb.Append(listitem.ToCliString("", obfuscatedFields));
            }

            if (empty)
            {
                sb.Append("No item found");
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
