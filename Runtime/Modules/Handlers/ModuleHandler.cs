using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JuiceBits
{
    public class ModuleHandler : MonoBehaviour
    {
        [SerializeField]
        private List<ModuleBase> modules = new();
        public List<ModuleBase> Modules => modules;
        [SerializeField]
        public bool IsSequential;

        private void Awake()
        {
            foreach (var module in Modules)
            {
                module.Initialize(gameObject);
            }
        }

        private void Update()
        {
            foreach (var module in Modules)
            {
                module.Update();
            }
        }
        public void PlayModules()
        {
            // Stops modules from playing when the ModuleHandler is deactivated
            if (!enabled)
            {
                return;
            }

            if (IsSequential)
            {
                PlaySequence();
            }
            else
            {
                PlayAll();
            }
        }

        private void PlayAll()
        {
            foreach (var module in Modules)
            {
                module.Play();
            }
        }

        private void PlaySequence()
        {
            StartCoroutine(PlayAsSequence());
        }

        // Modules with start delay are anchor modules and every module after them with no start delay starts simultaneously with the anchor module in front of them
        private IEnumerator PlayAsSequence()
        {
            for (int i = 0; i < Modules.Count; i++)
            {
                ModuleBase module = Modules[i];
                bool finished = false;

                module.OnFinished += () => finished = true;

                yield return new WaitForSeconds(module.StartDelay);

                module.Play();

                if (i + 1 >= Modules.Count || Modules[i + 1].StartDelay > 0)
                {
                    yield return new WaitUntil(() => finished);
                }
            }
        }

        public void StopModules()
        {
            foreach (var module in Modules)
            {
                module.Stop();
            }
        }

        public void StopModuleByID(string ID)
        {
            foreach (var module in Modules)
            {
                if (ID == module.ID)
                {
                    module.Stop();
                }
            }
        }
    }
}