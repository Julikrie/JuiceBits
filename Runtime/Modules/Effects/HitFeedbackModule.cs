using System.Collections;
using UnityEngine;

namespace JuiceBits
{
    public class HitFeedbackModule : ModuleBase
    {
        [SerializeField]
        public string Name = "Hit Feedback";
        public GameObject Target;
        public float Duration = 0.1f;
        private SpriteRenderer[] _spriteRenderers;
        private MeshRenderer[] _meshRenderers;
        public Color FlashFeedbackColor = new Color(1f, 1f, 1f, 1f);
        private Color[] _originalSpriteColors;
        private Color[] _originalMeshColors;
        private Material[] _originalSpriteMaterials;
        private Material[] _originalMeshMaterials;
        private Material _hitFlashMaterial;
        private Coroutine _coroutine;
        private Coroutine _repeatingCoroutine;

        public override void Initialize(GameObject targetObject)
        {
            // 2D Sprites
            _spriteRenderers = Target.GetComponentsInChildren<SpriteRenderer>();

            // Savest the original material and color of every Sprite
            _originalSpriteColors = new Color[_spriteRenderers.Length];
            _originalSpriteMaterials = new Material[_spriteRenderers.Length];

            for (int i = 0; i < _spriteRenderers.Length; i++)
            {
                _originalSpriteMaterials[i] = _spriteRenderers[i].material;
                _originalSpriteColors[i] = _spriteRenderers[i].material.color;
            }

            // Do not create when no sprite renderer exists
            if (_spriteRenderers.Length > 0)
            {
                _hitFlashMaterial = new Material(Shader.Find("Sprites/Default"));
            }

            // 3D Meshes
            _meshRenderers = Target.GetComponentsInChildren<MeshRenderer>();

            // Save original material and color of every mesh
            _originalMeshColors = new Color[_meshRenderers.Length];
            _originalMeshMaterials = new Material[_meshRenderers.Length];
            for (int i = 0; i < _meshRenderers.Length; i++)
            {
                _originalMeshMaterials[i] = _meshRenderers[i].material;
                _originalMeshColors[i] = _meshRenderers[i].material.color;
            }
        }

        // Executes the hit feedback
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
                _coroutine = CoroutineManager.Instance.StartCoroutine(PlayHitFeedback());
            }
        }

        // Stops the hit feedback and resets the values for the next use
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

            // Stops the 2D feedback
            for (int i = 0; i < _spriteRenderers.Length; i++)
            {
                _spriteRenderers[i].material = _originalSpriteMaterials[i];
                _spriteRenderers[i].material.color = _originalSpriteColors[i];
            }

            // Stops the 3D feedback
            for (int i = 0; i < _meshRenderers.Length; i++)
            {
                _meshRenderers[i].material = _originalMeshMaterials[i];
                _meshRenderers[i].material.color = _originalMeshColors[i];
            }

            _isRepeating = false;
            _skipStartDelay = false;
        }

        // Hit feedback logic
        private IEnumerator PlayHitFeedback()
        {
            if (!_skipStartDelay && !IsSequential)
            {
                yield return new WaitForSeconds(StartDelay);
            }

            // Flash color 2D
            for (int i = 0; i < _spriteRenderers.Length; i++)
            {
                _spriteRenderers[i].material.color = FlashFeedbackColor;
            }

            // Flash color 3D
            for (int i = 0; i < _meshRenderers.Length; i++)
            {
                _meshRenderers[i].material.color = FlashFeedbackColor;
            }

            yield return new WaitForSeconds(Duration);

            // Return to original color in 2D
            for (int i = 0; i < _spriteRenderers.Length; i++)
            {
                _spriteRenderers[i].material = _originalSpriteMaterials[i];
                _spriteRenderers[i].material.color = _originalSpriteColors[i];
            }

            // Return to original color in 3D
            for (int i = 0; i < _meshRenderers.Length; i++)
            {
                _meshRenderers[i].material = _originalMeshMaterials[i];
                _meshRenderers[i].material.color = _originalMeshColors[i];
            }

            // Checks if the hit feedback is finished and not in the repetition mode
            if (!_isRepeating)
            {
                Finished();
            }
        }

        // Enables the repetition of hit feedback
        private IEnumerator RepeatModule()
        {
            for (int i = 1; i <= Repetitions; i++)
            {
                _skipStartDelay = i > 1;

                yield return CoroutineManager.Instance.StartCoroutine(PlayHitFeedback());

                yield return new WaitForSeconds(RepeatDelay);
            }

            _isRepeating = false;

            // Checks if the repetition of the module is finished
            Finished();
        }
    }
}