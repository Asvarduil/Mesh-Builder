using System;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleJSON
{
    public static class JSONExtensions
    {
        #region Extensions

        public static JSONClass ExportAsJson(this Vector3 vector)
        {
            JSONClass result = new JSONClass();

            result["x"] = new JSONData(vector.x);
            result["y"] = new JSONData(vector.y);
            result["z"] = new JSONData(vector.z);

            return result;
        }

        public static JSONClass ExportAsJson(this Vector2 vector)
        {
            JSONClass result = new JSONClass();

            result["x"] = new JSONData(vector.x);
            result["y"] = new JSONData(vector.y);

            return result;
        }

        public static JSONClass ExportAsJson(this Color color)
        {
            JSONClass result = new JSONClass();

            result["r"] = new JSONData(color.r);
            result["g"] = new JSONData(color.g);
            result["b"] = new JSONData(color.b);
            result["a"] = new JSONData(color.a);

            return result;
        }

        public static Vector3 ImportVector3(this JSONNode jsonObject)
        {
            if (jsonObject == null)
                throw new DataException("Cannot import Vector3, as the JSONNode is null.");

            Vector3 result = new Vector3
            {
                x = jsonObject["x"].AsFloat,
                y = jsonObject["y"].AsFloat,
                z = jsonObject["z"].AsFloat
            };

            return result;
        }

        public static Vector2 ImportVector2(this JSONNode jsonObject)
        {
            if (jsonObject == null)
                throw new DataException("Cannot import Vector2, as the JSONNode is null.");

            Vector2 result = new Vector2
            {
                x = jsonObject["x"].AsFloat,
                y = jsonObject["y"].AsFloat
            };

            return result;
        }

        public static Color ImportColor(this JSONNode jsonObject)
        {
            if (jsonObject == null)
                throw new DataException("Cannot import Color, as the JSONNode is null.");

            Color color = new Color
            {
                r = jsonObject["r"].AsFloat,
                g = jsonObject["g"].AsFloat,
                b = jsonObject["b"].AsFloat,
                a = jsonObject["a"].AsFloat
            };

            return color;
        }

        public static JSONArray FoldPrimitiveList<T>(this List<T> list)
        {
            JSONArray array = new JSONArray();

            for(int i = 0; i < list.Count; i++)
            {
                JSONData item = new JSONData(list[i].ToString());
                array.Add(item);
            }

            return array;
        }

        public static JSONArray FoldList<T>(this List<T> list, SimpleJsonMapper<T> mapper)
            where T : new()
        {
            JSONArray array = new JSONArray();

            for(int i = 0; i < list.Count; i++)
            {
                JSONClass item = mapper.ExportState(list[i]);
                array.Add(item);
            }

            return array;
        }

        public static List<string> UnfoldStringJsonArray(this JSONArray array)
        {
            var result = new List<string>();

            foreach(JSONNode child in array.Childs)
            {
                result.Add(child.Value);
            }

            return result;
        }

        public static List<T> MapArrayWithMapper<T>(this JSONArray array, SimpleJsonMapper<T> mapper)
            where T : new()
        {
            List<T> result = new List<T>();

            foreach (JSONNode child in array.Childs)
            {
                T newItem = mapper.MapSingle(child.AsObject);
                result.Add(newItem);
            }

            return result;
        }

        public static T ToEnum<T>(this JSONNode node)
        {
            return (T)Enum.Parse(typeof(T), node);
        }

        #endregion Extensions
    }
}