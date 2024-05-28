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
        public string color;
        public string prefabPath;

        public ItemConfig(string id, string name)
        {
            this.id = id;
            this.name = name;
        }

        public Color GetColor()
        {
            string[] rgb = color.Split(',');
            if (rgb.Length != 3)
            {
                throw new FormatException("Color string is not in the correct format.");
            }
            
            if (int.TryParse(rgb[0], out int r) &&
                int.TryParse(rgb[1], out int g) &&
                int.TryParse(rgb[2], out int b))
            {
                return new Color(r / 255f, g / 255f, b / 255f);
            }
            else
            {
                throw new FormatException("Color string contains non-numeric values.");
            }
        }
    }
}