using UnityEngine;
using System.Collections;
using System;

namespace com.egamesstudios.cell {
    /// <summary>
    /// Wrapper class beacuse Unity's JSON serializer sucks
    /// </summary>
    [Serializable]
    public class DialogueWrapper
    {
        public string[] dialogue = new string[] { "Line1", "Line2" };
        public string this[int index]    // Indexer declaration  
        {
            get { return dialogue[index]; }
        }
        
    }
    /// <summary>
    /// Addition to unity's JSON to handle serialization of arrays
    /// </summary>
    public static class JsonHelper
    {
        public static T[] FromJson<T>(string json)
        {
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
            return wrapper.Items;
        }

        public static string ToJson<T>(T[] array)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = array;
            return JsonUtility.ToJson(wrapper);
        }

        public static string ToJson<T>(T[] array, bool prettyPrint)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = array;
            return JsonUtility.ToJson(wrapper, prettyPrint);
        }

        [Serializable]
        private class Wrapper<T>
        {
            public T[] Items;
        }
    }
}
