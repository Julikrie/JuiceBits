using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JuiceBits
{
    public class ParticleSceneModule : ModuleBase
    {
        [SerializeField]
        public string Name = "Particle Scene";
        public float DeactivateTime;
        public float ActivateTime;
        public ParticleRandomizer Randomizer = new();
        public List<ParticleSystem> Particle = new();
        public List<int> ActivateAtIndex = new();
        public List<int> DeactivateAtIndex = new(); 
        public GameObject Target;
        public Vector3 ParticleOffset;
        public bool AtIndex;
        public bool InSequence;
        public bool SetAsParent;
        public bool SamePosition;
        public bool ActivateParticle;
        public bool DeactivateParticle;

        private ParticleSystem.MainModule _mainModule;
        private ParticleSystemRenderer _particleSystemRenderer;
        private ParticleSystem.EmissionModule _emissionModule;

        public override void Initialize(GameObject targetObject)
        {
            foreach (ParticleSystem particle in Particle)
            {
                _mainModule = particle.main;
                _particleSystemRenderer = particle.GetComponent<ParticleSystemRenderer>();

                // Sets the fixed values of the Randomizer
                Randomizer.FixedMaterial = _particleSystemRenderer.sharedMaterial;
                Randomizer.FixedDurationn = _mainModule.duration;
                Randomizer.FixedLifetime = _mainModule.startLifetime.constant;
                Randomizer.FixedRotation = _mainModule.startRotation.constant;
                Randomizer.FixedSize = _mainModule.startSize.constant;
                Randomizer.FixedEmission = particle.emission.rateOverTime.constant;

                particle.transform.position += ParticleOffset;
            }
        }

        // Executes the Particle Scene logic
        public override void Play()
        {
            CoroutineManager.Instance.StartCoroutine(PrepareParticle());
        }

        // Sets the nesting, emission rate and randomizer
        private IEnumerator PrepareParticle()
        {
            if (!_skipStartDelay && !IsSequential)
            {
                yield return new WaitForSeconds(StartDelay);
            }

            Nesting();

            foreach (ParticleSystem particle in Particle)
            {
                SetEmission();
                ApplyRandomizer();
            }

            yield return CoroutineManager.Instance.StartCoroutine(SetDeactive());
            yield return CoroutineManager.Instance.StartCoroutine(SetActive());

            // Checks if the particle system is finished
            Finished();
        }

        private IEnumerator SetActive()
        {
            if (ActivateParticle && AtIndex)
            {

                // Activates marked particle systems at once 
                if (!InSequence)
                {
                    yield return new WaitForSeconds(ActivateTime);
                }

                // Activates particle systems one after another at the index
                foreach (int i in ActivateAtIndex)
                {
                    if (InSequence)
                    {
                        yield return new WaitForSeconds(ActivateTime);
                    }

                    Particle[i].gameObject.SetActive(true);
                }
            }

            if (ActivateParticle && !AtIndex)
            {
                // Activates the particle systems at once
                if (!InSequence)
                {
                    yield return new WaitForSeconds(ActivateTime);
                }

                // Activates the particle systems one after one 
                foreach (ParticleSystem particle in Particle)
                {
                    if (InSequence)
                    {
                        yield return new WaitForSeconds(ActivateTime);
                    }

                    particle.gameObject.SetActive(true);
                }
            }
        }

        private IEnumerator SetDeactive()
        {
            if (DeactivateParticle && AtIndex)
            {
                // Deactivates marked particle systems at once 
                if (!InSequence)
                {
                    yield return new WaitForSeconds(DeactivateTime);
                }

                // Deactivates particle systems one ofter another at the index
                foreach (int i in DeactivateAtIndex)
                {
                    if (InSequence)
                    {
                        yield return new WaitForSeconds(DeactivateTime);
                    }

                    Particle[i].gameObject.SetActive(false);
                }
            }

            if (DeactivateParticle && !AtIndex)
            {
                // Deactivates the particle system at once
                if (!InSequence)
                {
                    yield return new WaitForSeconds(DeactivateTime);
                }

                // Deactivates the particle systems one after another
                foreach (ParticleSystem particle in Particle)
                {
                    if (InSequence)
                    {
                        yield return new WaitForSeconds(DeactivateTime);
                    }

                    particle.gameObject.SetActive(false);
                }
            }
        }

        // Sets the target as parent of the particle systems
        private void Nesting()
        {
            if (SetAsParent)
            {
                foreach (ParticleSystem particle in Particle)
                {
                    particle.transform.SetParent(Target.transform);
                    particle.transform.position += ParticleOffset;

                    if (SamePosition)
                    {
                        particle.transform.position = Target.transform.position + ParticleOffset;
                    }
                }
            }
        }

        // Sets the randomizer values
        private void ApplyRandomizer()
        {
            foreach (ParticleSystem particle in Particle)
            {
                var main = particle.main;
                var emission = particle.emission;
                var renderer = particle.GetComponent<ParticleSystemRenderer>();

                Randomizer.FixedColor = main.startColor.color;
                
                // Sets the Particle System Values to the Randomizer Values
                main.startSize = Randomizer.GetSize();
                main.startColor = Randomizer.GetColor();
                main.startLifetime = Randomizer.GetLifetime();
                main.startRotation = Randomizer.GetRotation();
                emission.rateOverTime = Randomizer.GetEmission();

                if (!particle.isPlaying)
                {
                    main.duration = Randomizer.GetDuration();
                }
            }
        }

        // Sets the emission rate of the particle systems to the randomized emission rate
        private void SetEmission()
        {
            foreach (ParticleSystem particle in Particle)
            {
                _emissionModule = particle.emission;
                _emissionModule.rateOverTime = Randomizer.GetEmission();
            }
        }
    }
}