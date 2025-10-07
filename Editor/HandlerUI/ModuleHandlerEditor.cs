using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace JuiceBits
{
    // Editor for module handler
    [CustomEditor(typeof(ModuleHandler))]
    public class ModuleHandlerEditor : Editor
    {
        // VisualTreeAsset of the UI-Documents
        private VisualTreeAsset VisualTreeHandler;
        private VisualTreeAsset VisualTreeShake;
        private VisualTreeAsset VisualTreeFade;
        private VisualTreeAsset VisualTreeFlash;
        private VisualTreeAsset VisualTreeZoom;
        private VisualTreeAsset VisualTreeOffset;
        private VisualTreeAsset VisualTreeTimeStop;
        private VisualTreeAsset VisualTreeTimeScale;
        private VisualTreeAsset VisualTreeAfterImage2D;
        private VisualTreeAsset VisualTreeAfterImage3D;
        private VisualTreeAsset VisualTreeHitFeedback;
        private VisualTreeAsset VisualTreeParticleScene;
        private VisualTreeAsset VisualTreeParticleInstantiate;
        private VisualTreeAsset VisualTreeParallax;

        // Adds a dropdown entry with the "Custom Name" and module
        private Dictionary<string, Func<ModuleBase>> _dropdownEntries = new()
    {
        {"Camera Shake", () => CreateInstance<ShakeModule>()},
        {"Camera Fade", () => CreateInstance<FadeModule>()},
        {"Camera Flash", () => CreateInstance<FlashModule>()},
        {"Camera Zoom", () => CreateInstance<ZoomModule>()},
        {"Camera Offset", () => CreateInstance<OffsetModule>()},
        {"Camera Parallax", () => CreateInstance<ParallaxModule>()},
        {"Camera TimeStop", () => CreateInstance<TimeStopModule>()},
        {"Camera TimeScale", () => CreateInstance<TimeScaleModule>()},
        {"Hit Feedback", () => CreateInstance<HitFeedbackModule>()},
        {"Afterimage 2D", () => CreateInstance<AfterImage2DModule>()},
        {"Afterimage 3D", () => CreateInstance<AfterImage3DModule>()},
        {"Particle Scene", () => CreateInstance<ParticleSceneModule>()},
        {"Particle Instantiate", () => CreateInstance<ParticleInstantiateModule>()}
    };

        private void OnEnable()
        {
            // Loads all Visual Trees to work both in Edit and Play mode - Visual Trees need to be in the resources folder
            VisualTreeHandler = Resources.Load<VisualTreeAsset>("UXML/Handler/ModuleHandlerVisualTree");
            VisualTreeShake = Resources.Load<VisualTreeAsset>("UXML/Camera/CameraShakeVisualTree");
            VisualTreeFade = Resources.Load<VisualTreeAsset>("UXML/Camera/CameraFadeVisualTree");
            VisualTreeFlash = Resources.Load<VisualTreeAsset>("UXML/Camera/CameraFlashVisualTree");
            VisualTreeZoom = Resources.Load<VisualTreeAsset>("UXML/Camera/CameraZoomVisualTree");
            VisualTreeOffset = Resources.Load<VisualTreeAsset>("UXML/Camera/CameraOffsetVisualTree");
            VisualTreeTimeStop = Resources.Load<VisualTreeAsset>("UXML/Camera/CameraTimeStopVisualTree");
            VisualTreeTimeScale = Resources.Load<VisualTreeAsset>("UXML/Camera/CameraTimeScaleVisualTree");
            VisualTreeAfterImage2D = Resources.Load<VisualTreeAsset>("UXML/Camera/AfterImage2DVisualTree");
            VisualTreeAfterImage3D = Resources.Load<VisualTreeAsset>("UXML/Camera/AfterImage3DVisualTree");
            VisualTreeHitFeedback = Resources.Load<VisualTreeAsset>("UXML/Camera/HitFeedbackVisualTree");
            VisualTreeParticleScene = Resources.Load<VisualTreeAsset>("UXML/Particle/ParticleSceneVisualTree");
            VisualTreeParticleInstantiate = Resources.Load<VisualTreeAsset>("UXML/Particle/ParticleInstantiateVisualTree");
            VisualTreeParallax = Resources.Load<VisualTreeAsset>("UXML/Camera/ParallaxVisualTree");
        }

        public override VisualElement CreateInspectorGUI()
        {
            ModuleHandler moduleHandler = (ModuleHandler)target;
            var root = VisualTreeHandler.CloneTree();

            DropdownField dropdown = root.Q<DropdownField>("Dropdown");
            ListView listView = root.Q<ListView>("ModuleList");
            BaseBoolField isSequential = root.Q<BaseBoolField>("Sequential");

            dropdown.choices = _dropdownEntries.Keys.ToList();
            dropdown.value = "Please Select Effect";
            isSequential.value = moduleHandler.IsSequential;

            listView.itemsSource = moduleHandler.Modules;
            listView.reorderable = true;
            listView.showAddRemoveFooter = false;
            listView.selectionType = SelectionType.None;

            listView.makeItem = () => new VisualElement();

            listView.bindItem = (element, i) =>
            {
                element.Clear();
                var module = moduleHandler.Modules[i];

                // Method to remove the modules with a remove button
                void RemoveModule()
                {
                    Undo.RecordObject(moduleHandler, "Remove Module");
                    moduleHandler.Modules.RemoveAt(i);
                    EditorUtility.SetDirty(moduleHandler);
                    listView.Rebuild();
                }

                // Adds the correct UI of the dropdown list
                if (module is ShakeModule shakeModule)
                    element.Add(new ShakeUI(shakeModule, VisualTreeShake).CreateInterface(RemoveModule));

                else if (module is FadeModule fadeModule)
                    element.Add(new FadeUI(fadeModule, VisualTreeFade).CreateInterface(RemoveModule));

                else if (module is FlashModule flashModule)
                    element.Add(new FlashUI(flashModule, VisualTreeFlash).CreateInterface(RemoveModule));

                else if (module is ZoomModule zoomModule)
                    element.Add(new ZoomUI(zoomModule, VisualTreeZoom).CreateInterface(RemoveModule));

                else if (module is OffsetModule offsetModule)
                    element.Add(new OffsetUI(offsetModule, VisualTreeOffset).CreateInterface(RemoveModule));

                else if (module is ParallaxModule parallaxModule)
                    element.Add(new ParallaxUI(parallaxModule, VisualTreeParallax).CreateInterface(RemoveModule));

                else if (module is TimeStopModule timeStopModule)
                    element.Add(new TimeStopUI(timeStopModule, VisualTreeTimeStop).CreateInterface(RemoveModule));

                else if (module is TimeScaleModule timeScaleModule)
                    element.Add(new TimeScaleUI(timeScaleModule, VisualTreeTimeScale).CreateInterface(RemoveModule));

                else if (module is AfterImage2DModule afterImage2DModule)
                    element.Add(new AfterImage2DUI(afterImage2DModule, VisualTreeAfterImage2D).CreateInterface(RemoveModule));

                else if (module is AfterImage3DModule afterImage3DModule)
                    element.Add(new AfterImage3DUI(afterImage3DModule, VisualTreeAfterImage3D).CreateInterface(RemoveModule));

                else if (module is HitFeedbackModule hitFeedbackModule)
                    element.Add(new HitFeedbackUI(hitFeedbackModule, VisualTreeHitFeedback).CreateInterface(RemoveModule));

                else if (module is ParticleSceneModule particleSceneModule)
                    element.Add(new ParticleSceneUI(particleSceneModule, VisualTreeParticleScene).CreateInterface(RemoveModule));

                else if (module is ParticleInstantiateModule particleInstantiateModule)
                    element.Add(new ParticleInstantiateUI(particleInstantiateModule, VisualTreeParticleInstantiate).CreateInterface(RemoveModule));
            };

            // Changes to sequential when the checkbox is ticked
            isSequential.RegisterValueChangedCallback(isSequential =>
            {
                moduleHandler.IsSequential = isSequential.newValue;

                foreach (var module in moduleHandler.Modules)
                {
                    module.IsSequential = isSequential.newValue;
                }
            });

            dropdown.RegisterValueChangedCallback(module =>
            {
                if (_dropdownEntries.TryGetValue(module.newValue, out var moduleEntry))
                {
                    ModuleBase newModule = moduleEntry();
                    moduleHandler.Modules.Add(newModule);

                    if (Application.isPlaying)
                        newModule.Initialize(moduleHandler.gameObject);

                    if (!Application.isPlaying)
                    {
                        Undo.RecordObject(moduleHandler, "Add Module");
                        EditorUtility.SetDirty(moduleHandler);
                    }

                    listView.Rebuild();
                    dropdown.SetValueWithoutNotify("Please Select Effect");
                }
            });

            return root;
        }
    }
}