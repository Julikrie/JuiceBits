using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace JuiceBits
{
    public class AfterImage2DUI
    {
        private AfterImage2DModule _module;
        private VisualTreeAsset _userInterface;

        public AfterImage2DUI(AfterImage2DModule module, VisualTreeAsset userInterface)
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
            IntegerField afterImageSortingOrder = root.Q<IntegerField>("AfterImageSortingOrder");
            IntegerField repetitions = root.Q<IntegerField>("Repetitions");
            IntegerField maxAfterImages = root.Q<IntegerField>("MaxAfterImages");
            FloatField startDelay = root.Q<FloatField>("StartDelay");
            FloatField probability = root.Q<FloatField>("Probability");
            FloatField repetitionDelay = root.Q<FloatField>("RepetitionDelay");
            FloatField spawnRate = root.Q<FloatField>("SpawnRate");
            FloatField despawnTime = root.Q<FloatField>("DespawnTime");
            Vector2Field spawnDistance = root.Q<Vector2Field>("SpawnDistance");
            Vector2Field scaleToSize = root.Q<Vector2Field>("ScaleToSize");
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
            afterImageSortingOrder.value = _module.AfterImageSortingOrder;
            repetitions.value = _module.Repetitions;
            maxAfterImages.value = _module.MaxAfterImages;
            startDelay.value = _module.StartDelay;
            probability.value = _module.Probability;
            repetitionDelay.value = _module.RepeatDelay;
            spawnRate.value = _module.SpawnRate;
            despawnTime.value = _module.DespawnTime;
            spawnDistance.value = _module.SpawnDistance;
            scaleToSize.value = _module.ScaleToSize;
            fadeOverTime.value = _module.FadeOverTime;
            scaleOverTime.value = _module.ScaleOverTime;
            untilStopped.value = _module._runningUntilStopped;
            useCurve.value = _module.UseCurve;
            easePresets.value = _module.EaseTypes;
            animationCurve.value = _module.FadeCurve;
            target.value = _module.Target;
            color.value = _module.AfterImageColor;
            moduleName.text = _module.Name;
            delayInfo.text = "Delay: " + _module.StartDelay.ToString("F2") + " seconds";

            // Tooltips
            name.tooltip = "Name of the Module";
            stopID.tooltip = "Use a string to stop with StopModuleByID(string)";
            afterImageSortingOrder.tooltip = "The Sorting Order of the Afterimage";
            repetitions.tooltip = "How often the Module repeats itself";
            maxAfterImages.tooltip = "The amount of Afterimages that can be active in the Scene";
            startDelay.tooltip = "Delay to the predecessor Module";
            probability.tooltip = "The chance to trigger the Module";
            repetitionDelay.tooltip = "Delay between the Repetitons";
            spawnRate.tooltip = "Set time to spawn the Afterimages";
            despawnTime.tooltip = "Set time to despawn the Afterimages";
            spawnDistance.tooltip = "Distance of the Afterimages to the Target";
            scaleToSize.tooltip = "Scales the Afterimage to the chosen Size";
            useCurve.tooltip = "Activates the Animation Curve Field";
            animationCurve.tooltip = "Create own Fade Curve";
            fadeOverTime.tooltip = "Activates the Fading of Afterimages";
            scaleOverTime.tooltip = "Activates the Scaling of Afterimages";
            untilStopped.tooltip = "Runs the Module until it is stopped with the StopID";
            easePresets.tooltip = "Curve Presets to choose from";
            target.tooltip = "The Target of the Afterimages spawns";
            color.tooltip = "The Color of the Afterimages";
            delayInfo.tooltip = "Information about the Delay of the Module";

            // Initializes Values of the Enums
            easePresets.Init(_module.EaseTypes);
            // Turns off/on field display when UseCurve checkbox is ticked
            animationCurve.style.display = _module.UseCurve ? DisplayStyle.Flex : DisplayStyle.None;
            easePresets.style.display = _module.UseCurve ? DisplayStyle.None : DisplayStyle.Flex;

            // Registers value changes and saves them
            mainFoldout.RegisterValueChangedCallback(mainFoldout => _module.IsMainOpen = mainFoldout.newValue);
            extrasFoldout.RegisterValueChangedCallback(extrasFoldout => _module.IsExtrasOpen = extrasFoldout.newValue);
            name.RegisterValueChangedCallback(evt =>
            {
                _module.Name = evt.newValue;
                moduleName.text = _module.Name;
            });
            stopID.RegisterValueChangedCallback(stopID => _module.ID = stopID.newValue);
            afterImageSortingOrder.RegisterValueChangedCallback(sortingOrder => _module.AfterImageSortingOrder = sortingOrder.newValue);
            repetitions.RegisterValueChangedCallback(repetitions => _module.Repetitions = repetitions.newValue);
            maxAfterImages.RegisterValueChangedCallback(maxAfterImages => _module.MaxAfterImages = maxAfterImages.newValue);
            startDelay.RegisterValueChangedCallback(startDelay =>
            {
                _module.StartDelay = startDelay.newValue;
                delayInfo.text = "Delay: " + startDelay.newValue.ToString("F2") + " seconds";
            });
            probability.RegisterValueChangedCallback(probability => _module.Probability = probability.newValue);
            repetitionDelay.RegisterValueChangedCallback(repetitionDelay => _module.RepeatDelay = repetitionDelay.newValue);
            spawnRate.RegisterValueChangedCallback(spawnRate => _module.SpawnRate = spawnRate.newValue);
            despawnTime.RegisterValueChangedCallback(despawnTime => _module.DespawnTime = despawnTime.newValue);
            spawnDistance.RegisterValueChangedCallback(spawnDistance => _module.SpawnDistance = spawnDistance.newValue);
            scaleToSize.RegisterValueChangedCallback(scaleToSize => _module.ScaleToSize = scaleToSize.newValue);
            fadeOverTime.RegisterValueChangedCallback(fadeOverTime => _module.FadeOverTime = fadeOverTime.newValue);
            scaleOverTime.RegisterValueChangedCallback(scaleOverTime => _module.ScaleOverTime = scaleOverTime.newValue);
            untilStopped.RegisterValueChangedCallback(untilStopped => _module._runningUntilStopped = untilStopped.newValue);
            easePresets.RegisterValueChangedCallback(easePresets => _module.EaseTypes = (EaseTypes)easePresets.newValue);
            useCurve.RegisterValueChangedCallback(useCurve =>
            {
                _module.UseCurve = useCurve.newValue;
                animationCurve.style.display = useCurve.newValue ? DisplayStyle.Flex : DisplayStyle.None;
                easePresets.style.display = useCurve.newValue ? DisplayStyle.None : DisplayStyle.Flex;
            });
            animationCurve.RegisterValueChangedCallback(animationCurve => _module.FadeCurve = animationCurve.newValue);
            target.RegisterValueChangedCallback(target => _module.Target = (GameObject)target.newValue);
            color.RegisterValueChangedCallback(color => _module.AfterImageColor = color.newValue);

            removeButton.clicked += () => removeModule?.Invoke();

            return root;
        }
    }
}