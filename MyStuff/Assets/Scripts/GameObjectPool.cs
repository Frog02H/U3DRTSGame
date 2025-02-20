using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MyObjectPool
{
    public class GameObjectPool : PoolBase<GameObject>
    {
        public GameObjectPool(
            GameObject prefab,
            Func<GameObject> preloadFunc,
            Action<GameObject> getAction,
            Action<GameObject> returnAction,
            int preloadCount
        ) : base(() => Preload((prefab)), GetAction, ReturnAction, preloadCount)
        {

        }

        public static GameObject Preload(GameObject prefab) => Object.Instantiate(prefab);
        public static void GetAction(GameObject @object) => @object.SetActive(true);
        public static void ReturnAction(GameObject @object) => @object.SetActive(false);
    }
}