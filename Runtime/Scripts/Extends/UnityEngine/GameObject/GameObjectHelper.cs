using UnityEngine;

namespace TinaX
{
    public static class GameObjectHelper
    {
        public static GameObject FindOrCreateGameObject(string name)
        {
            var go = GameObject.Find(name);
            if (go == null)
            {
                go = new GameObject(name);
            }
            return go;
        }
    }
}
