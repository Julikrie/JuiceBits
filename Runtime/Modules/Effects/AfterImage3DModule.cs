using System.Collections;
using UnityEngine;

namespace JuiceBits
{
    // In 3D the material has to be in fade mode
    public class AfterImage3DModule : ModuleBase
    {
        [SerializeField]
        public string Name = "Afterimage 3D";
        public GameObject Target;
        public AnimationCurve AnimationCurve;
        public Color AfterImageColor = Color.white;
        public int MaxAfterImages = 10;
        public float SpawnRate = 0.1f;
        public float DespawnTime = 1f;
        public float SpawnDistance;
        public Vector3 ScaleToSize;
        private Vector3 _movementDirection;
        private Vector3 _spawnPosition;
        public bool UseCurve;
        public bool IsFollowMode;
        public bool FadeOverTime;
        public bool ScaleOverTime;
        public EaseTypes EaseTypes = EaseTypes.Linear;

        private int _currentAfterImages = 0;
        private CharacterController _targetController;
        private MeshRenderer _targetMeshRenderer;
        private Rigidbody _targetRigidbody;
        private Coroutine _coroutine;

        public override void Initialize(GameObject targetObject)
        {
            // Character controller 
            if (_targetController != null)
            {
                _targetController = Target.GetComponent<CharacterController>();
            }

            // Rigidbody
            if (_targetRigidbody != null)
            {
                _targetRigidbody = Target.GetComponent<Rigidbody>();
            }

            _targetMeshRenderer = Target.GetComponent<MeshRenderer>();
        }

        // Sets the default value of the animation curve to linear
        private void Reset()
        {
            AnimationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
        }

        // Executes the afterimage 3D logic
        public override void Play()
        {
            if (_coroutine != null)
            {
                CoroutineManager.Instance.StopCoroutine(_coroutine);
            }

            _coroutine = CoroutineManager.Instance.StartCoroutine(CreateAfterImage3D());
        }

        // Stops the afterimage 3D 
        public override void Stop()
        {
            if (_coroutine != null)
            {
                CoroutineManager.Instance.StopCoroutine(_coroutine);
                _coroutine = null;
            }
        }

        // Afterimage 3D logic
        private IEnumerator CreateAfterImage3D()
        {
            int spawnedAfterImages = 0;
            bool _hasController = _targetController != null;
            bool _hasRigidbody = _targetRigidbody != null;

            MeshRenderer[] meshRenderers = Target.GetComponentsInChildren<MeshRenderer>();
            MeshFilter[] meshFilters = Target.GetComponentsInChildren<MeshFilter>();

            if (!IsSequential)
            {
                yield return new WaitForSeconds(StartDelay);
            }

            while (spawnedAfterImages < MaxAfterImages || _runningUntilStopped)
            {
                if (_currentAfterImages < MaxAfterImages)
                {
                    // Character controller check
                    if (_hasController)
                    {
                        _movementDirection = _targetController.velocity;
                    }
                    // Rigidbody check
                    else if (_hasRigidbody)
                    {
                        _movementDirection = _targetRigidbody.linearVelocity;
                    }
                    else
                    {
                        _movementDirection = Vector3.zero;
                    }

                    if (_movementDirection.sqrMagnitude < 0.01f)
                    {
                        _movementDirection = Target.transform.forward;
                    }

                    // Gets the movement direction
                    _movementDirection.Normalize();
                    _spawnPosition = Target.transform.position - _movementDirection * SpawnDistance;

                    // Create afterimage similar to target game object
                    GameObject afterImage = new GameObject("AfterImage");
                    afterImage.transform.position = _spawnPosition;
                    afterImage.transform.rotation = Target.transform.rotation;
                    afterImage.transform.localScale = Target.transform.localScale;

                    // Checks if the target has more than one mesh renderer or mesh filter -> game object has childs
                    if (meshFilters.Length > 1 || meshRenderers.Length > 1)
                    {
                        CreateForMultipleMeshes(meshRenderers, meshFilters, afterImage);
                    }
                    // One mesh renderer and mesh filter -> single game object
                    else
                    {
                        CreateForSingleMesh(afterImage);
                    }

                    spawnedAfterImages++;
                    _currentAfterImages++;

                    // Starts the fade and scale
                    if (FadeOverTime || ScaleOverTime)
                    {
                        CoroutineManager.Instance.StartCoroutine(FadeAndScale(afterImage));
                    }
                    else
                    {
                        Destroy(afterImage, DespawnTime);
                        CoroutineManager.Instance.StartCoroutine(DecreaseCounter(DespawnTime));
                    }
                }

                yield return new WaitForSeconds(SpawnRate);
            }

            // Checks if the afterimage 3D is finished
            Finished();
        }

        private void CreateForSingleMesh(GameObject afterImage)
        {
            // Adds mesh filter and mesh renderer to the afterimage for the same layout as the target
            MeshFilter afterImageMeshFilter = afterImage.AddComponent<MeshFilter>();
            afterImageMeshFilter.mesh = _targetMeshRenderer.GetComponent<MeshFilter>().sharedMesh;

            MeshRenderer afterImageMeshRenderer = afterImage.AddComponent<MeshRenderer>();
            Material afterImageMaterial = new Material(_targetMeshRenderer.sharedMaterial);
            afterImageMaterial.color = AfterImageColor;

            // Sets the material mode to fade and the standard values of Unity
            afterImageMaterial.SetFloat("_Mode", 2);
            afterImageMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            afterImageMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            afterImageMaterial.SetInt("_ZWrite", 0);
            afterImageMaterial.DisableKeyword("_ALPHATEST_ON");
            afterImageMaterial.EnableKeyword("_ALPHABLEND_ON");
            afterImageMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            afterImageMaterial.renderQueue = 3000;

            afterImageMeshRenderer.material = afterImageMaterial;
        }

        private void CreateForMultipleMeshes(MeshRenderer[] meshRenderers, MeshFilter[] meshFilters, GameObject afterImage)
        {
            // Looks at every child and creates an afterimage with the same properties as target
            for (int i = 0; i < meshFilters.Length; i++)
            {
                GameObject targetChild = new GameObject(meshFilters[i].gameObject.name);
                targetChild.transform.SetParent(afterImage.transform);
                targetChild.transform.localPosition = meshFilters[i].transform.localPosition;
                targetChild.transform.localRotation = meshFilters[i].transform.localRotation;
                targetChild.transform.localScale = meshFilters[i].transform.localScale;

                MeshFilter targetChildMeshFilter = targetChild.AddComponent<MeshFilter>();
                targetChildMeshFilter.mesh = meshFilters[i].sharedMesh;

                MeshRenderer targetChildMeshRenderer = targetChild.AddComponent<MeshRenderer>();
                Material targetChildMaterial = new Material(meshRenderers[i].sharedMaterial);
                targetChildMaterial.color = AfterImageColor;

                // Sets the material mode to fade and the standard values of Unity
                targetChildMaterial.SetFloat("_Mode", 2);
                targetChildMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                targetChildMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                targetChildMaterial.SetInt("_ZWrite", 0);
                targetChildMaterial.DisableKeyword("_ALPHATEST_ON");
                targetChildMaterial.EnableKeyword("_ALPHABLEND_ON");
                targetChildMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                targetChildMaterial.renderQueue = 3000;

                targetChildMeshRenderer.material = targetChildMaterial;
            }
        }

        // Decreases the current count of the afterimages after the despawn time
        private IEnumerator DecreaseCounter(float despawnTime)
        {
            yield return new WaitForSeconds(despawnTime);
            _currentAfterImages--;
        }

        // Fade and scale logic
        private IEnumerator FadeAndScale(GameObject afterImage)
        {
            float elapsedTime = 0f;
            Vector3 afterImageScale = afterImage.transform.localScale;

            MeshRenderer[] afterImageRenderers = afterImage.GetComponentsInChildren<MeshRenderer>();
            Color[] afterImageColor = new Color[afterImageRenderers.Length];

            // Loops through all mesh renderers and saves their current material color
            for (int i = 0; i < afterImageRenderers.Length; i++)
            {
                afterImageColor[i] = afterImageRenderers[i].material.color;
            }

            while (elapsedTime < DespawnTime)
            {
                elapsedTime += Time.deltaTime;
                float time = elapsedTime / DespawnTime;
                float easedTime = UseCurve ? AnimationCurve.Evaluate(time) : EaseManger.Easings(EaseTypes, time);

                if (FadeOverTime)
                {
                    for (int i = 0; i < afterImageRenderers.Length; i++)
                    {
                        Color colorAfterImage = afterImageColor[i];
                        colorAfterImage.a = Mathf.Lerp(afterImageColor[i].a, 0f, easedTime);
                        afterImageRenderers[i].material.color = colorAfterImage;
                    }
                }

                if (ScaleOverTime)
                {
                    afterImage.transform.localScale = Vector3.Lerp(afterImageScale, ScaleToSize, easedTime);
                }

                yield return null;
            }

            Destroy(afterImage);
            _currentAfterImages--;
        }
    }
}