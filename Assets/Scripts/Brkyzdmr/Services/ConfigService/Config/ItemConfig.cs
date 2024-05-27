using System;
using UnityEngine;

namespace Brkyzdmr.Services.ConfigService
{
    [Serializable]
    public class ItemConfig
    {
        public string id;
        public string name;
        public string iconPath;
        public Color color;
        public string prefabPath;
    }
}