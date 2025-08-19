using System.Linq;
using UnityEngine;

namespace Project.Scripts.Utils
{
    public static class GameObjectExtension
    {
        public static bool IsInLayer(this GameObject gameObject, LayerMask layer)
        {
            return layer == (layer | 1 << gameObject.layer);
        }

        public static T GetInterface<T>(this GameObject gameObject)
        {
            var components = gameObject.GetComponents<Component>();

            foreach (var component in components)
            {
                if (component is T type)
                {
                    return type;
                }
            }

            return default;
        }
    }
}