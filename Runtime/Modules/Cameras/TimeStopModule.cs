using System.Collections;
using UnityEngine;

namespace JuiceBits
{
    public class TimeStopModule : ModuleBase
    {
        [SerializeField]
        public string Name = "Time Stop";
        public float Duration;

        private Coroutine _coroutine;
        private Coroutine _repeatingCoroutine;

        // Executes the time stop
        public override void Play()
        {
            bool isPlaying = true;

            if (Probability > 0f)
            {
                float randomValue = Random.Range(0f, 100f);

                if (randomValue > Probability)
                {
                    isPlaying = false;
                }
            }

            if (!isPlaying)
                return;

            if (Repetitions > 0)
            {
                if (!_isRepeating)
                {
                    _isRepeating = true;
                    _repeatingCoroutine = CoroutineManager.Instance.StartCoroutine(RepeatModule());
                }
            }
            else
            {
                _coroutine = CoroutineManager.Instance.StartCoroutine(PlayTimeStop());
            }
        }

        // Stops the time stop and resets the values for the next use
        public override void Stop()
        {
            if (_coroutine != null)
            {
                CoroutineManager.Instance.StopCoroutine(_coroutine);
                _coroutine = null;
            }

            if (_repeatingCoroutine != null)
            {
                CoroutineManager.Instance.StopCoroutine(_repeatingCoroutine);
                _repeatingCoroutine = null;
            }

            _isRepeating = false;
            _skipStartDelay = false;
            Time.timeScale = 1f;
        }

        // Time stop logic
        private IEnumerator PlayTimeStop()
        {
            float originalTimeScale = Time.timeScale;

            if (!_skipStartDelay && !IsSequential)
            {
                yield return new WaitForSecondsRealtime(StartDelay);
            }

            Time.timeScale = 0f;

            if (!_runningUntilStopped)
            {
                yield return new WaitForSecondsRealtime(Duration);
            }
            else
            {
                // If the time stop is running until stopped the user has to stop the time stop
                while (true)
                {
                    yield return null;
                }
            }

            Time.timeScale = originalTimeScale;

            // Checks if the time scale is done and not in repetition mode
            if (!_isRepeating)
            {
                Finished();
            }
        }

        private IEnumerator RepeatModule()
        {
            for (int i = 1; i <= Repetitions; i++)
            {
                _skipStartDelay = i > 1;

                yield return CoroutineManager.Instance.StartCoroutine(PlayTimeStop());

                yield return new WaitForSecondsRealtime(RepeatDelay);
            }

            _isRepeating = false;

            // Checks if the repetition is finished
            Finished();
        }
    }
}