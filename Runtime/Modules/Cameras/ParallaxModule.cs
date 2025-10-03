using System.Collections.Generic;
using UnityEngine;

namespace JuiceBits
{
    public class ParallaxModule : ModuleBase
    {
        public List<Parallax> ParallaxList = new List<Parallax>();

        // Parallax class with the fields and values 
        [System.Serializable]
        public class Parallax
        {
            [SerializeField]
            public bool IsOpen;
            [SerializeField]
            public string Name = "Parallax";
            public Transform Layer;
            public Vector2 LayerSpeed;
            [HideInInspector]
            public float LayerWidth;
            [HideInInspector]
            public float LayerHeight;
            [HideInInspector]
            public Transform LayerDuplicateWidth;
            [HideInInspector]
            public Transform LayerDuplicateHeight;
        }

        public override void Initialize(GameObject targetObject)
        {
            // Looking for the min and max of the bounds
            foreach (Parallax parallax in ParallaxList)
            {
                float minWidth = float.MaxValue;
                float maxWidth = float.MinValue;
                float minHeight = float.MaxValue;
                float maxHeight = float.MinValue;

                SpriteRenderer[] spriteRenderer = parallax.Layer.GetComponentsInChildren<SpriteRenderer>();

                foreach (SpriteRenderer renderer in spriteRenderer)
                {
                    Bounds b = renderer.bounds;
                    minWidth = Mathf.Min(minWidth, b.min.x);
                    maxWidth = Mathf.Max(maxWidth, b.max.x);
                    minHeight = Mathf.Min(minHeight, b.min.y);
                    maxHeight = Mathf.Max(maxHeight, b.max.y);
                }
                
                // Calculating the Width and Height of the Parallax
                parallax.LayerWidth = maxWidth - minWidth;
                parallax.LayerHeight = maxHeight - minHeight;
            }
        }

        public override void Update()
        {

        }

        public override void Play()
        {
            // Declare movement
            foreach (Parallax parallax in ParallaxList)
            {
                Vector3 movement = new Vector3(parallax.LayerSpeed.x, parallax.LayerSpeed.y, 0f) * Time.deltaTime;
                parallax.Layer.position += movement;

                // X-Axis - When the parallax has X-Speed, create an duplicate on the X-Axis
                if (parallax.LayerSpeed.x != 0f)
                {
                    if (parallax.LayerDuplicateWidth == null)
                    {
                        parallax.LayerDuplicateWidth = Instantiate(parallax.Layer, parallax.Layer.position + new Vector3(parallax.LayerWidth, 0f, 0f), Quaternion.identity, parallax.Layer.parent);
                    }

                    parallax.LayerDuplicateWidth.position += movement;

                    // Looks if the parallax layer has moved off screen and moves it behind the duplicate
                    if (parallax.Layer.position.x <= -parallax.LayerWidth)
                    {
                        parallax.Layer.position = parallax.LayerDuplicateWidth.position + new Vector3(parallax.LayerWidth, 0f, 0f);
                    }

                    if (parallax.LayerDuplicateWidth.position.x <= -parallax.LayerWidth)
                    {
                        parallax.LayerDuplicateWidth.position = parallax.Layer.position + new Vector3(parallax.LayerWidth, 0f, 0f);
                    }
                }

                // Y-Axis - When the parallax has Y-Speed, create a duplicate on the Y-Axis
                if (parallax.LayerSpeed.y != 0f)
                {
                    if (parallax.LayerDuplicateHeight == null)
                    {
                        parallax.LayerDuplicateHeight = Instantiate(parallax.Layer, parallax.Layer.position + new Vector3(0f, parallax.LayerHeight, 0f), Quaternion.identity, parallax.Layer.parent);
                    }

                    parallax.LayerDuplicateHeight.position += movement;

                    // Looks if the parallax layer has moved off screen and moves it behind the duplicate
                    if (parallax.Layer.position.y <= -parallax.LayerHeight)
                    {
                        parallax.Layer.position = parallax.LayerDuplicateHeight.position + new Vector3(0f, parallax.LayerHeight, 0f);
                    }

                    if (parallax.LayerDuplicateHeight.position.y <= -parallax.LayerHeight)
                    {
                        parallax.LayerDuplicateHeight.position = parallax.Layer.position + new Vector3(0f, parallax.LayerHeight, 0f);
                    }
                }
            }
        }
    }
}