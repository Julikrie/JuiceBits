using System;
using UnityEngine;

namespace JuiceBits
{
    [Serializable]
    public abstract class ModuleBase : ScriptableObject
    {
        public string ID;
        public int Repetitions;
        public float RepeatDelay;
        public float StartDelay;
        public float Probability;
        public event Action OnFinished;
        public bool IsSequential = false;
        public bool IsMainOpen = true;
        public bool IsExtrasOpen = false;
        public bool _skipStartDelay = false;
        public bool _isRepeating = false;
        public bool _runningUntilStopped = false;

        protected GameObject _targetObject;

        // Awake method of my modules
        public virtual void Initialize(GameObject targetObject)
        {
            _targetObject = targetObject;
        }

        // Update method of my modules
        public virtual void Update()
        {

        }

        // Executes the logic of the modules
        public abstract void Play();

        // Stops the modules
        public virtual void Stop()
        {

        }

        // Broadcasts the finish event
        public void Finished()
        {
            OnFinished?.Invoke();
        }
    }
}