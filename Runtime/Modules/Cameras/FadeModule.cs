using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace JuiceBits
{
    public class FadeModule : ModuleBase
    {
        [SerializeField]
        public string Name = "Fade";
        public int SortingOrder = 999;
        public float FadeDuration = 0.2f;
        public bool UseCurve = false;
        public AnimationCurve AnimationCurve;
        public Color FadeColor = Color.white;
        public EaseTypes EaseTypes = EaseTypes.Linear;
        public FadeTypes FadeType = FadeTypes.FadeIn;

        private Image _fadeImage;
        private Coroutine _coroutine;
        private Coroutine _repeatCoroutine;

        // Creates the canvas for the fade in the scene
        public override void Initialize(GameObject targetObject)
        {
            GameObject fadeCanvas = new GameObject("FadeCanvas");
            Canvas canvas = fadeCanvas.AddComponent<Canvas>();
            _fadeImage = fadeCanvas.AddComponent<Image>();
            canvas.sortingOrder = SortingOrder;

            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            CanvasScaler fadeScaler = fadeCanvas.AddComponent<CanvasScaler>();
            GraphicRaycaster graphicRaycaster = fadeCanvas.AddComponent<GraphicRaycaster>();

            fadeScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;

            _fadeImage.color = Color.clear;
        }

        // Sets the default value of the animation curve to linear
        private void Reset()
        {
            AnimationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
        }

        // Executes the logic of the fade module
        public override void Play()
        {
            Stop();

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
                    _repeatCoroutine = CoroutineManager.Instance.StartCoroutine(RepeatModule());
                }
            }
            else
            {
                _coroutine = CoroutineManager.Instance.StartCoroutine(PlayFade());
            }
        }

        // Stops the fade coroutine and resets the values for the next fade
        public override void Stop()
        {
            if (_coroutine != null)
            {
                CoroutineManager.Instance.StopCoroutine(_coroutine);
                _coroutine = null;
            }

            if (_repeatCoroutine != null)
            {
                CoroutineManager.Instance.StopCoroutine(_repeatCoroutine);
                _repeatCoroutine = null;
            }

            _isRepeating = false;
            _skipStartDelay = false;
            _fadeImage.color = Color.clear;
        }

        // Repeats the module
        private IEnumerator RepeatModule()
        {
            {
                for (int i = 1; i <= Repetitions; i++)
                {
                    // Skips the delay at the start of the repetition
                    _skipStartDelay = i > 1;

                    yield return CoroutineManager.Instance.StartCoroutine(PlayFade());

                    yield return new WaitForSeconds(RepeatDelay);
                }
            }

            _isRepeating = false;

            // Event to check if the module is finished
            Finished();
        }

        // Fade Logic
        private IEnumerator PlayFade()
        {
            if (!_skipStartDelay && !IsSequential)
            {
                yield return new WaitForSeconds(StartDelay);
            }

            _fadeImage.color = FadeColor;
            float elapsedTime = 0f;

            while (elapsedTime < FadeDuration)
            {
                elapsedTime += Time.deltaTime;
                float time = elapsedTime / FadeDuration;
                float easedTime = UseCurve ? AnimationCurve.Evaluate(time) : EaseManger.Easings(EaseTypes, time);
                float alpha = Mathf.Lerp(1f, 0f, easedTime);
                alpha = FadeType == FadeTypes.FadeIn ? Mathf.Lerp(1f, 0f, easedTime) : Mathf.Lerp(0f, 1f, easedTime);

                _fadeImage.color = new Color(FadeColor.r, FadeColor.g, FadeColor.b, alpha);

                yield return null;
            }

            _fadeImage.color = FadeType == FadeTypes.FadeIn ? Color.clear : new Color(FadeColor.r, FadeColor.g, FadeColor.b, 1f);

            // Event to check if the module is finished
            if (!_isRepeating)
            {
                Finished();
            }
        }
    }
}