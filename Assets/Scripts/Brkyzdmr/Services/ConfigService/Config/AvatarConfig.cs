using System;
using UnityEngine;

namespace Brkyzdmr.Services.ConfigService
{
    [Serializable]
    public class AvatarConfig
    {
        public string id;
        public string name;
        public Sprite icon;
        public int price;
        public string prefabPath;
    }
}