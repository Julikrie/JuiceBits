using UnityEngine;

namespace JuiceBits
{
    public class ParallaxManager : MonoBehaviour
    {
        public ModuleHandler ParallaxEffects;

        // Starts the Parallax  
        void Update()
        {
            ParallaxEffects?.PlayModules();
        }
    }
}