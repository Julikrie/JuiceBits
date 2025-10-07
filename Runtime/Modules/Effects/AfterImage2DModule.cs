using System.Collections;
using UnityEngine;

namespace JuiceBits
{
    public class AfterImage2DModule : ModuleBase
    {
        [SerializeField]
        public string Name = "Afterimage 2D";
        public GameObject Target;
        public int AfterImageSortingOrder;
        public int MaxAfterImages = 5;
        private int _currentAfterImages;
        public float SpawnRate = 0.1f;
        public float DespawnTime = 1f;
        public Vector2 SpawnDistance;
        public Vector2 ScaleToSize;
        public Color AfterImageColor = Color.white;
        public bool UseCurve;
        public bool FadeOverTime = false;
        public bool IsFollowMode;
        public bool ScaleOverTime = false;
        public AnimationCurve FadeCurve;
        public EaseTypes EaseTypes = EaseTypes.Linear;

        private Vector2 _spawnPosition;
        private Vector2 _movementDirection;
        private Rigidbody2D _targetRb;
        private SpriteRenderer _targetRenderer;
        private Coroutine _coroutine;
        private Coroutine _followCoroutine;

        public override void Initialize(GameObject targetObject)
        {
            _targetRb = Target.GetComponent<Rigidbody2D>();
            _targetRenderer = Target.GetComponent<SpriteRenderer>();
        }

        // Sets the default value of the animation curve to linear
        private void Reset()
        {
            FadeCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
        }

        // Executes the afterimage 2D
        public override void Play()
        {
            if (_coroutine != null)
            {
                CoroutineManager.Instance.StopCoroutine(_coroutine);
            }

            _coroutine = CoroutineManager.Instance.StartCoroutine(CreateAfterImage2D());
        }

        // Stops the afterimage 2D and resets his values to use it again
        public override void Stop()
        {
            if (_coroutine != null && _runningUntilStopped)
            {
                CoroutineManager.Instance.StopCoroutine(_coroutine);
                _coroutine = null;
            }

            if (_followCoroutine != null)
            {
                IsFollowMode = false;
                CoroutineManager.Instance.StopCoroutine(_followCoroutine);
                _followCoroutine = null;
            }
        }

        // Afterimage 2D logic
        private IEnumerator CreateAfterImage2D()
        {
            // Afterimages in the scene
            int spawnedAfterImages = 0;

            if (!IsSequential)
            {
                yield return new WaitForSeconds(StartDelay);
            }

            while (spawnedAfterImages < MaxAfterImages || _runningUntilStopped)
            {
                {
                    // Looks in which direction the target moves
                    _movementDirection = _targetRb.linearVelocity.normalized;
                    _spawnPosition = _targetRb.position - _movementDirection * SpawnDistance;

                    // Creates afterimage
                    GameObject afterImage = new GameObject("AfterImage");
                    afterImage.transform.position = _spawnPosition;

                    SpriteRenderer afterImageRenderer = afterImage.AddComponent<SpriteRenderer>();

                    // Sets the afterimage sprite similar to the target Sprite
                    afterImageRenderer.sprite = _targetRenderer.sprite;
                    afterImageRenderer.flipX = _targetRenderer.flipX;
                    afterImageRenderer.flipY = _targetRenderer.flipY;
                    afterImageRenderer.color = AfterImageColor;
                    afterImageRenderer.sortingLayerID = _targetRenderer.sortingLayerID;
                    afterImageRenderer.sortingOrder = AfterImageSortingOrder;

                    spawnedAfterImages++;
                    _currentAfterImages++;

                    // Activates fade and/or scale over time
                    if (FadeOverTime || ScaleOverTime)
                    {
                        CoroutineManager.Instance.StartCoroutine(FadeAndScale(afterImage, afterImageRenderer));
                    }
                    else
                    {
                        Destroy(afterImage, DespawnTime);
                        CoroutineManager.Instance.StartCoroutine(DecreaseCounter(DespawnTime));
                    }
                }

                yield return new WaitForSeconds(SpawnRate);
            }

            // Checks if the afterimage 2D is finished
            Finished();
        }

        // Decreases the counter of the afterimages 2D after they despawn
        private IEnumerator DecreaseCounter(float despawnTime)
        {
            yield return new WaitForSeconds(despawnTime);

            _currentAfterImages--;
        }

        // Fade and scale Logic
        private IEnumerator FadeAndScale(GameObject afterImage, SpriteRenderer afterImageRenderer)
        {
            float elapsedTime = 0f;
            Vector3 startScale = afterImage.transform.localScale;
            float startAlpha = AfterImageColor.a;

            // Fade and scale over Time
            while (elapsedTime < DespawnTime)
            {
                elapsedTime += Time.deltaTime;
                float time = elapsedTime / DespawnTime;
                float easedTime = UseCurve ? FadeCurve.Evaluate(time) : EaseManger.Easings(EaseTypes, time);
                Color color = AfterImageColor;

                if (FadeOverTime)
                {
                    color.a = Mathf.Lerp(startAlpha, 0f, easedTime);
                }

                afterImageRenderer.color = color;

                if (ScaleOverTime)
                {
                    afterImage.transform.localScale = Vector3.Lerp(startScale, ScaleToSize, easedTime);
                }

                yield return null;
            }

            // Destroys the afterimage 2D, when the fade and scale is done
            Destroy(afterImage);
            _currentAfterImages--;
        }
    }
}