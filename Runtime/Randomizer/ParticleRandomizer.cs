using System.Collections.Generic;
using UnityEngine;

namespace JuiceBits
{
    [System.Serializable]
    public class ParticleRandomizer
    {
        public float FixedEmission;
        public float MinEmission;
        public float MaxEmission;
        public float FixedDurationn;
        public float MinDuration;
        public float MaxDuration;
        public float FixedLifetime;
        public float MinLifetime;
        public float MaxLifetime;
        public float FixedSize;
        public float MinSize;
        public float MaxSize;
        public float FixedRotation;
        public float MinRotation;
        public float MaxRotation;
        public bool UseRandom = false;
        public Color FixedColor;
        public Material FixedMaterial;
        public List<Color> RandomColor = new();
        public List<Material> RandomMaterial = new();

        // Gets random color of min and max
        public Color GetColor()
        {
            if (!UseRandom)
            {
                return FixedColor;
            }
            else
            {
                if (RandomColor.Count == 0) return FixedColor;

                int index = Random.Range(0, RandomColor.Count);
                Color color = RandomColor[index];
                color.a = 1f;

                return color;
            }
        }

        // Gets random material of min and max
        public Material GetMaterial()
        {
            if (!UseRandom)
            {
                return FixedMaterial;
            }
            else
            {
                if (RandomMaterial.Count == 0) return FixedMaterial;

                int index = Random.Range(0, RandomMaterial.Count);

                return RandomMaterial[index];
            }
        }

        // Gets random size of min and max
        public float GetSize()
        {
            // The basic value "0" gets used, with this the MaxValue has to be changed by the user and the MinValue can still be 0
            if (UseRandom && MaxSize > 0)
            {
                return Random.Range(MinSize, MaxSize);
            }
            else
            {
                return FixedSize;
            }
        }

        // Gets random lifetime of min and max
        public float GetLifetime()
        {
            if (UseRandom && MaxLifetime > 0)
            {
                return Random.Range(MinLifetime, MaxLifetime);
            }
            else
            {
                return FixedLifetime;
            }
        }

        // Gets random duration of min and max
        public float GetDuration()
        {
            if (UseRandom & MaxDuration > 0f)
            {
                return Random.Range(MinDuration, MaxDuration);
            }
            else
            {
                return FixedDurationn;
            }
        }

        // Gets random rotation of min and max
        public float GetRotation()
        {
            if (UseRandom && MaxRotation > 0f)
            {
                return Random.Range(MinRotation, MaxRotation);
            }
            else
            {
                return FixedRotation;
            }
        }

        // Gets random emisison rate of min and max
        public float GetEmission()
        {
            if (UseRandom && MaxEmission > 0f)
            {
                return Random.Range(MinEmission, MaxEmission);
            }
            else
            {
                return FixedEmission;
            }
        }
    }
}