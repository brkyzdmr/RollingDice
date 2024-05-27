using System;
using UnityEngine;

namespace Brkyzdmr.Services.ViewService
{
    public class View : MonoBehaviour, IView
    {
        public string Id { get; set; }

        public virtual void OnSelect() { }

        public virtual void OnMatch() { }

        public virtual void OnDeselect() { }
        public virtual void OnDestroyed() { }
    }
}