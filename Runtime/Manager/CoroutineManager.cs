using System.Collections;
using UnityEngine;

namespace JuiceBits
{
    // The Coroutine Manager executes the coroutines of the modules
    // because they do not inherit from MonoBehaviour and can't execute them on their own
    // Singleton to be usable everywhere, without reference
    public class CoroutineManager : MonoBehaviour
    {
        private static CoroutineManager _instance;

        public static CoroutineManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject coroutineManager = new GameObject("CoroutineManager");
                    _instance = coroutineManager.AddComponent<CoroutineManager>();
                    DontDestroyOnLoad(coroutineManager);
                }

                return _instance;
            }
        }

        public void RunCoroutine(IEnumerator coroutine)
        {
            StartCoroutine(coroutine);
        }

        public Coroutine RunTrackedCoroutine(IEnumerator coroutine)
        {
            return StartCoroutine(coroutine);
        }

        public void StoppingCoroutine(IEnumerator coroutine)
        {
            StopCoroutine(coroutine);
        }
    }
}