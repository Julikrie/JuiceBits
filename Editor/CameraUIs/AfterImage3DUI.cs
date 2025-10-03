using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace JuiceBits
{
    public class AfterImage3DUI
    {
        private AfterImage3DModule _module;
        private VisualTreeAsset _userInterface;

        public AfterImage3DUI(AfterImage3DModule module, VisualTreeAsset userInterface)
        {
            _module = module;
            _userInterface = userInterface;
        }

        // Creates the whole interface for the module
        public VisualElement CreateInterface(System.Action removeModule = null)
        {
            // Everything beneath the root gets drawn as UI
            VisualElement root = _userInterface.CloneTree();

            // Searches for the root with "Name" and assigns it to the variable
            Foldout mainFoldout = root.Q<Foldout>("MainFoldout");
            Foldout extrasFoldout = root.Q<Foldout>("Extras");
            Label moduleName = root.Q<Label>("ModuleName");
            Label delayInfo = root.Q<Label>("DelayInfo");
            TextField name = root.Q<TextField>("AfterImageName");
            TextField stopID = root.Q<TextField>("ID");
            IntegerField maxAfterImages = root.Q<IntegerField>("MaxAfterImages");
            FloatField startDelay = root.Q<FloatField>("StartDelay");
            FloatField spawnRate = root.Q<FloatField>("SpawnRate");
            FloatField despawnTime = root.Q<FloatField>("DespawnTime");
            FloatField spawnDistance = root.Q<FloatField>("SpawnDistance");
            Vector3Field scaleToSize = root.Q<Vector3Field>("ScaleToSize");
            BaseBoolField fadeOverTime = root.Q<BaseBoolField>("FadeOverTime");
            BaseBoolField scaleOverTime = root.Q<BaseBoolField>("ScaleOverTime");
            BaseBoolField untilStopped = root.Q<BaseBoolField>("RunningUntilStopped");
            BaseBoolField useCurve = root.Q<BaseBoolField>("UseCurve");
            EnumField easePresets = root.Q<EnumField>("EasePresets");
            CurveField animationCurve = root.Q<CurveField>("AnimationCurve");
            ObjectField target = root.Q<ObjectField>("Target");
            ColorField color = root.Q<ColorField>("Color");
            Button removeButton = root.Q<Button>("RemoveButton");

            // Assigns the values of the module to the variables
            mainFoldout.value = _module.IsMainOpen;
            extrasFoldout.value = _module.IsExtrasOpen;
            name.value = _module.Name;
            stopID.value = _module.ID;
            maxAfterImages.value = _module.MaxAfterImages;
            startDelay.value = _module.StartDelay;
            spawnRate.value = _module.SpawnRate;
            despawnTime.value = _module.DespawnTime;
            spawnDistance.value = _module.SpawnDistance;
            scaleToSize.value = _module.ScaleToSize;
            fadeOverTime.value = _module.FadeOverTime;
            scaleOverTime.value = _module.ScaleOverTime;
            untilStopped.value = _module._runningUntilStopped;
            useCurve.value = _module.UseCurve;
            easePresets.value = _module.EaseTypes;
            animationCurve.value = _module.AnimationCurve;
            target.value = _module.Target;
            color.value = _module.AfterImageColor;
            moduleName.text = _module.Name;
            delayInfo.text = "Delay: " + _module.StartDelay.ToString("F2") + " seconds";

            // Tooltips
            name.tooltip = "Name of the Module";
            stopID.tooltip = "Use a string to stop with StopModuleByID(string)";
            maxAfterImages.tooltip = "The amount of Afterimages that can be active in the Scene";
            startDelay.tooltip = "Delay to the predecessor Module";
            spawnRate.tooltip = "Set time to spawn the Afterimages";
            despawnTime.tooltip = "Set time to despawn the Afterimages";
            spawnDistance.tooltip = "Distance of the Afterimages to the Target";
            scaleToSize.tooltip = "Scales the Afterimage to the chosen Size";
            useCurve.tooltip = "Activates the Animation Curve Field";
            animationCurve.tooltip = "Create own Animation Curve";
            fadeOverTime.tooltip = "Activates the Fading of Afterimages";
            scaleOverTime.tooltip = "Activates the Scaling of Afterimages";
            untilStopped.tooltip = "Runs the Module until it is stopped with the StopID";
            easePresets.tooltip = "Curve Presets to choose from";
            target.tooltip = "The Target of the Afterimages spawns";
            color.tooltip = "The Color of the Afterimages";
            delayInfo.tooltip = "Information about the Delay of the Module";

            // Initializes values of the Enums
            easePresets.Init(_module.EaseTypes);
            // Turns Off/On field display when UseCurve checkbox is ticked
            animationCurve.style.display = _module.UseCurve ? DisplayStyle.Flex : DisplayStyle.None;
            easePresets.style.display = _module.UseCurve ? DisplayStyle.None : DisplayStyle.Flex;

            // Registers value changes and saves them
            mainFoldout.RegisterValueChangedCallback(mainFoldout => _module.IsMainOpen = mainFoldout.newValue);
            extrasFoldout.RegisterValueChangedCallback(extrasFoldout => _module.IsExtrasOpen = extrasFoldout.newValue);
            name.RegisterValueChangedCallback(name =>
            {
                _module.Name = name.newValue;
                moduleName.text = _module.Name;
            });
            stopID.RegisterValueChangedCallback(stopID => _module.ID = stopID.newValue);
            maxAfterImages.RegisterValueChangedCallback(maxAfterImages => _module.MaxAfterImages = maxAfterImages.newValue);
            startDelay.RegisterValueChangedCallback(evt =>
            {
                _module.StartDelay = evt.newValue;
                delayInfo.text = "Delay: " + evt.newValue.ToString("F2") + " seconds";
            });
            spawnRate.RegisterValueChangedCallback(spawnRate => _module.SpawnRate = spawnRate.newValue);
            despawnTime.RegisterValueChangedCallback(despawnTime => _module.DespawnTime = despawnTime.newValue);
            spawnDistance.RegisterValueChangedCallback(spawnDistance => _module.SpawnDistance = spawnDistance.newValue);
            scaleToSize.RegisterValueChangedCallback(scaleToSize => _module.ScaleToSize = scaleToSize.newValue);
            fadeOverTime.RegisterValueChangedCallback(fadeOverTime => _module.FadeOverTime = fadeOverTime.newValue);
            scaleOverTime.RegisterValueChangedCallback(scaleOverTime => _module.ScaleOverTime = scaleOverTime.newValue);
            untilStopped.RegisterValueChangedCallback(untilStopped => _module._runningUntilStopped = untilStopped.newValue);
            useCurve.RegisterValueChangedCallback(useCurve =>
            {
                _module.UseCurve = useCurve.newValue;
                animationCurve.style.display = useCurve.newValue ? DisplayStyle.Flex : DisplayStyle.None;
                easePresets.style.display = useCurve.newValue ? DisplayStyle.None : DisplayStyle.Flex;
            });
            easePresets.RegisterValueChangedCallback(easePresets => _module.EaseTypes = (EaseTypes)easePresets.newValue);
            animationCurve.RegisterValueChangedCallback(animationCurve => _module.AnimationCurve = animationCurve.newValue);
            target.RegisterValueChangedCallback(target => _module.Target = (GameObject)target.newValue);
            color.RegisterValueChangedCallback(color => _module.AfterImageColor = color.newValue);

            removeButton.clicked += () => removeModule?.Invoke();

            return root;
        }
    }
}