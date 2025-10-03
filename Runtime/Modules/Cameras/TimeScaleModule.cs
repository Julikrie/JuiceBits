using System.Collections;
using UnityEngine;

namespace JuiceBits
{
    public class TimeScaleModule : ModuleBase
    {
        [SerializeField]
        public string Name = "Time Scale";
        public float Duration;
        public float TimeScale;
        public float StartScale;
        public float EndScale;
        public bool UseCurve = false;
        public bool TimeScaleInstantly = false;
        public AnimationCurve AnimationCurve;
        public EaseTypes EaseTypes = EaseTypes.Linear;

        private float _elapsedTime = 0f;
        private Coroutine _coroutine;
        private Coroutine _repeatingCoroutine;
        private Coroutine _easeCoroutine;

        public override void Initialize(GameObject targetObject)
        {

        }

        // Sets the default value of the animation curve to linear
        private void Reset()
        {
            AnimationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
        }

        public override void Update()
        {

        }

        // Executes the time scale logic
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
                _coroutine = CoroutineManager.Instance.StartCoroutine(PlayTimeScale());
            }
        }

        // Stops the time scale and resets the values for the next time scale
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

            if (_easeCoroutine != null)
            {
                CoroutineManager.Instance.StopCoroutine(_easeCoroutine);
                _easeCoroutine = null;
            }

            _isRepeating = false;
            _skipStartDelay = false;
            _elapsedTime = 0f;
            Time.timeScale = 1f;
        }

        // Time scale logic
        private IEnumerator PlayTimeScale()
        {
            if (!_skipStartDelay && !IsSequential)
            {
                yield return new WaitForSecondsRealtime(StartDelay);
            }

            _easeCoroutine = CoroutineManager.Instance.StartCoroutine(EaseTimeScale());
            yield return _easeCoroutine;

            _elapsedTime = 0f;
            Time.timeScale = 1f;

            // Checks if the time scale is finished
            if (!_isRepeating)
            {
                Finished();
            }
        }

        // Lerps the time scale with ease presets
        private IEnumerator EaseTimeScale()
        {
            _elapsedTime = 0f;
            float originalTimeScale = Time.timeScale;

            if (TimeScaleInstantly || _runningUntilStopped)
            {
                Time.timeScale = TimeScale;

                if (!_runningUntilStopped)
                {
                    yield return new WaitForSecondsRealtime(Duration);
                }
                else
                {
                    // When the time scale runs forever, do not use the duration
                    while (true)
                    {
                        yield return null;
                    }
                }

                Time.timeScale = originalTimeScale;
                yield break;
            }

            // Time scale lerp 
            while (_elapsedTime < Duration)
            {
                _elapsedTime += Time.unscaledDeltaTime;
                float time = Mathf.Clamp01(_elapsedTime / Duration);
                float easedTime = UseCurve ? AnimationCurve.Evaluate(time) : EaseManger.Easings(EaseTypes, time);

                Time.timeScale = Mathf.Lerp(StartScale, EndScale, easedTime);

                yield return null;
            }

            Time.timeScale = TimeScaleInstantly ? originalTimeScale : EndScale;
        }

        // Enables the repetitionn of time scale
        private IEnumerator RepeatModule()
        {
            for (int i = 1; i <= Repetitions; i++)
            {
                _skipStartDelay = i > 1;

                yield return CoroutineManager.Instance.StartCoroutine(PlayTimeScale());

                yield return new WaitForSecondsRealtime(RepeatDelay);
            }

            _isRepeating = false;

            // Checks if the time scale is finished
            Finished();
        }
    }
}