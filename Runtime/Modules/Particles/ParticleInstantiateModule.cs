using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

namespace JuiceBits
{
    [System.Serializable]
    public class ParticleInstantiateModule : ModuleBase
    {
        [SerializeField]
        public string Name = "Particle Instantiate";
        public GameObject Target;
        public int DefaultCapacity = 20;
        public int MaxCapacity = 50;
        public float CooldownTime;
        public Vector3 Offset;
        public bool SetAsParent;
        public bool UsePooling;
        public bool UseCooldown;
        public ParticleSystem ParticlePrefab;
        public ParticleRandomizer Randomizer = new();

        private bool _hasCooldown;
        private ParticleSystem.MainModule _mainModule;
        private ObjectPool<ParticleSystem> _ParticlePool;
        private ParticleSystemRenderer _particleRenderer;
        private Coroutine _repeatingCoroutine;

        public override void Initialize(GameObject targetObject)
        {
            _mainModule = ParticlePrefab.main;
            _particleRenderer = ParticlePrefab.GetComponent<ParticleSystemRenderer>();

            // Fixed values are the values of the particle prefab
            Randomizer.FixedMaterial = _particleRenderer.sharedMaterial;
            Randomizer.FixedColor = _mainModule.startColor.color;
            Randomizer.FixedDurationn = _mainModule.duration;
            Randomizer.FixedLifetime = _mainModule.startLifetime.constant;
            Randomizer.FixedRotation = _mainModule.startRotation.constant;
            Randomizer.FixedSize = _mainModule.startSize.constant;
            Randomizer.FixedEmission = ParticlePrefab.emission.rateOverTime.constant;

            // Object pool
            _ParticlePool = new ObjectPool<ParticleSystem>(CreateParticlePrefab, ActionOnGet, ActionOnRelease, ActionOnDestroy, collectionCheck: true, DefaultCapacity, MaxCapacity);
        }

        // Executes the particle instantiate logic
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
            else if (!_hasCooldown)
            {
                CreateParticle();

                if (UseCooldown)
                {
                    CoroutineManager.Instance.StartCoroutine(StartCooldown());
                }
            }
        }

        public override void Update()
        {

        }

        // Creates the particle
        private void CreateParticle()
        {
            ParticleSystem particleInstantiate;

            // Create pooled particle system
            if (UsePooling && _ParticlePool.CountActive < MaxCapacity)
            {
                particleInstantiate = _ParticlePool.Get();
                CoroutineManager.Instance.StartCoroutine(HandleLifeCycle(particleInstantiate, isPooled: true));
            }
            // Creates no pooled particle system
            else
            {
                particleInstantiate = Instantiate(ParticlePrefab);
                particleInstantiate.gameObject.SetActive(true);

                // Set target as parent of particle system
                if (SetAsParent)
                {
                    particleInstantiate.transform.SetParent(Target.transform);
                    particleInstantiate.transform.position = Target.transform.position + Offset;
                }
                else
                {
                    particleInstantiate.transform.SetParent(null);
                    particleInstantiate.transform.position = Target.transform.position + Offset;
                }
                // Apply the randomized values
                ApplyRandomizer(particleInstantiate);
                particleInstantiate.Clear();
                particleInstantiate.Play();

                CoroutineManager.Instance.StartCoroutine(HandleLifeCycle(particleInstantiate, isPooled: false));
            }
        }

        // Single particle
        private ParticleSystem CreateParticlePrefab()
        {
            ParticleSystem particle = Instantiate(ParticlePrefab);
            var renderer = particle.GetComponent<ParticleSystemRenderer>();

            return particle;
        }

        private void ActionOnGet(ParticleSystem particle)
        {
            particle.gameObject.SetActive(true);

            if (SetAsParent)
            {
                // Sets the target as parent
                particle.transform.SetParent(Target.transform);
                particle.transform.position = Target.transform.position + Offset;
            }
            else
            {
                // Removes the child object from the parent
                particle.transform.SetParent(null);
                particle.transform.position += Offset;
            }

            ApplyRandomizer(particle);
            particle.Clear(true);
            particle.Play();
        }

        // When the particle system is done playing, deactivate it
        private void ActionOnRelease(ParticleSystem particle)
        {
            particle.gameObject.SetActive(false);
        }

        // Destroy particle system
        private void ActionOnDestroy(ParticleSystem particle)
        {
            Destroy(particle.gameObject);
        }

        // Gets an random value with the particle randomizer
        private void ApplyRandomizer(ParticleSystem particle)
        {
            ParticleSystemRenderer renderer = particle.GetComponent<ParticleSystemRenderer>();
            SetEmission(particle);

            var main = particle.main;
            main.startSize = Randomizer.GetSize();
            main.startColor = Randomizer.GetColor();
            main.startLifetime = Randomizer.GetLifetime();
            main.startRotation = Randomizer.GetRotation();
            renderer.material = Randomizer.GetMaterial();

            // Unity does not allow the duration change of running particle system
            if (!particle.isPlaying)
            {
                main.duration = Randomizer.GetDuration();
            }
        }

        // If the particle is in an object pool deactivate it, else destroy it after his duration
        private IEnumerator HandleLifeCycle(ParticleSystem particle, bool isPooled)
        {
            if (particle.main.loop) yield break;

            yield return new WaitForSeconds(particle.main.duration);

            if (isPooled)
            {
                _ParticlePool.Release(particle);
            }
            else
            {
                Destroy(particle.gameObject);
            }
        }

        // Sets the emission rate of the particle system to the random emission rate
        private void SetEmission(ParticleSystem particle)
        {
            ParticleSystem.EmissionModule emission = particle.emission;
            emission.rateOverTime = Randomizer.GetEmission();
        }

        // Has the particle system a cooldown prepare it
        private IEnumerator StartCooldown()
        {
            _hasCooldown = true;

            yield return new WaitForSeconds(CooldownTime);

            _hasCooldown = false;
        }

        // Eneables the repetion of the particle systems
        private IEnumerator RepeatModule()
        {
            for (int i = 1; i <= Repetitions; i++)
            {
                _skipStartDelay = i > 1;

                if (!_skipStartDelay && !IsSequential)
                {
                    yield return new WaitForSeconds(StartDelay);
                }

                CreateParticle();

                yield return new WaitForSeconds(RepeatDelay);
            }

            _isRepeating = false;
        }
    }
}