using System;
using UnityEngine;

namespace Brkyzdmr.Attributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class ButtonAttribute : PropertyAttribute
    {
        public string name { get; set; }
        public bool hideInPlayMode { get; set; }
        public bool hideInEditMode { get; set; }

        public ButtonAttribute() : this("")
        {
        }

        public ButtonAttribute(string name = "", bool hideInPlayMode = false, bool hideInEditMode = false)
        {
            this.name = name;
            this.hideInPlayMode = hideInPlayMode;
            this.hideInEditMode = hideInEditMode;
        }
    }
}
