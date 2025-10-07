using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace JuiceBits
{
    public class FlashModule : ModuleBase
    {
        [SerializeField]
        public string Name = "Flash";
        public float FlashDuration = 0.1f;
        public int SortingOrder = 999;
        public Color FlashSettings = Color.white;

        private Image _flashImage;
        private Coroutine _coroutine;
        private Coroutine _repeatingCoroutine;

        // Creates the flash canvas
        public override void Initialize(GameObject targetObject)
        {
            GameObject flashCanvas = new GameObject("FlashCanvas");
            Canvas canvas = flashCanvas.AddComponent<Canvas>();
            CanvasScaler flashScaler = flashCanvas.AddComponent<CanvasScaler>();
            GraphicRaycaster graphicRaycaster = flashCanvas.AddComponent<GraphicRaycaster>();
            _flashImage = flashCanvas.AddComponent<Image>();

            canvas.sortingOrder = SortingOrder;
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            flashScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            _flashImage.color = Color.clear;
        }

        // Stops the flash and resets the values for a next flash
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
            _flashImage.color = Color.clear;
        }

        // Executes the logic of the flash module
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
                _coroutine = CoroutineManager.Instance.StartCoroutine(PlayFlash());
            }
        }

        // Enables repetition of the flash
        private IEnumerator RepeatModule()
        {
            {
                for (int i = 1; i <= Repetitions; i++)
                {
                    _skipStartDelay = i > 1;

                    yield return CoroutineManager.Instance.StartCoroutine(PlayFlash());

                    yield return new WaitForSeconds(RepeatDelay);
                }
            }

            _isRepeating = false;

            // Finish check of the repetition
            Finished();
        }

        // Flash Logic
        private IEnumerator PlayFlash()
        {
            if (!_skipStartDelay && !IsSequential)
            {
                yield return new WaitForSeconds(StartDelay);
            }

            _flashImage.color = new Color(FlashSettings.r, FlashSettings.g, FlashSettings.b, FlashSettings.a);

            // Runs via duration time, or it has a running until stopped marker and has to be stopped manually
            if (!_runningUntilStopped)
            {
                yield return new WaitForSeconds(FlashDuration);

                _flashImage.color = Color.clear;
            }

            // Finish check of the not repeating flash
            if (!_isRepeating)
            {
                Finished();
            }
        }
    }
}