using UnityEngine;

namespace Brkyzdmr.Helpers
{
    public static class JsonReader
    {
        /// <summary>
        /// Reads a JSON file from the Resources folder and deserializes it into an object of type T.
        /// </summary>
        /// <typeparam name="T">The type of object to deserialize the JSON into.</typeparam>
        /// <param name="path">The path to the JSON file inside the Resources folder.</param>
        /// <returns>The deserialized object of type T, or null if an error occurs during reading or deserialization.</returns>
        public static T ReadJsonFromResources<T>(string path) where T : class
        {
            try
            {
                string json = Resources.Load<TextAsset>(path).text;
                return JsonUtility.FromJson<T>(json);
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Error reading JSON config: " + ex.Message);
                return null;
            }
        }
    }
}