using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace JuiceBits
{
    public class FadeUI
    {
        private FadeModule _module;
        private VisualTreeAsset _userInterface;

        public FadeUI(FadeModule module, VisualTreeAsset userInterface)
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
            TextField name = root.Q<TextField>("FadeName");
            TextField stopID = root.Q<TextField>("ID");
            IntegerField sortingOrder = root.Q<IntegerField>("FadeOrder");
            IntegerField repetitions = root.Q<IntegerField>("Repetitions");
            FloatField duration = root.Q<FloatField>("FadeDuration");
            FloatField startDelay = root.Q<FloatField>("StartDelay");
            FloatField probability = root.Q<FloatField>("Probability");
            FloatField repetitionDelay = root.Q<FloatField>("RepetitionDelay");
            EnumField fadeType = root.Q<EnumField>("FadeType");
            EnumField easePresets = root.Q<EnumField>("FadeEasePresets");
            BaseBoolField useCurve = root.Q<BaseBoolField>("UseCurve");
            BaseBoolField runningUntilStopped = root.Q<BaseBoolField>("RunningUntilStopped");
            ColorField color = root.Q<ColorField>("FadeColor");
            CurveField animationCurve = root.Q<CurveField>("AnimationCurve");
            Button removeButton = root.Q<Button>("RemoveButton");

            // Assigns the values of the module to the variables
            mainFoldout.value = _module.IsMainOpen;
            extrasFoldout.value = _module.IsExtrasOpen;
            name.value = _module.Name;
            stopID.value = _module.ID;
            sortingOrder.value = _module.SortingOrder;
            color.value = _module.FadeColor;
            duration.value = _module.FadeDuration;
            fadeType.value = _module.FadeType;
            easePresets.value = _module.EaseTypes;
            useCurve.value = _module.UseCurve;
            animationCurve.value = _module.AnimationCurve;
            startDelay.value = _module.StartDelay;
            probability.value = _module.Probability;
            repetitions.value = _module.Repetitions;
            repetitionDelay.value = _module.RepeatDelay;
            runningUntilStopped.value = _module._runningUntilStopped;
            moduleName.text = _module.Name;
            delayInfo.text = "Delay: " + _module.StartDelay.ToString("F2") + " seconds";

            // Tooltips
            name.tooltip = "Name of the Module";
            stopID.tooltip = "Use a string to stop with StopModuleByID(string)";
            repetitions.tooltip = "How often the Module repeats itself";
            startDelay.tooltip = "Delay to the predecessor Module";
            probability.tooltip = "The chance to trigger the Module";
            repetitionDelay.tooltip = "Delay between the Repetitons";
            useCurve.tooltip = "Activates the Animation Curve Field";
            animationCurve.tooltip = "Create own Animation Curve";
            runningUntilStopped.tooltip = "Runs the Module until it is stopped with the StopID";
            color.tooltip = "The Color of the After Images";
            delayInfo.tooltip = "Information about the Delay of the Module";
            sortingOrder.tooltip = "The Sorting Order of the Fade";
            duration.tooltip = "The Duration of the Fade";
            fadeType.tooltip = "Choose between FadeIn and FadeOut";
            easePresets.tooltip = "Curve Presets to choose from";

            // Initializes values of the Enums
            fadeType.Init(_module.FadeType);
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
            sortingOrder.RegisterValueChangedCallback(sortingOrder => _module.SortingOrder = sortingOrder.newValue);
            color.RegisterValueChangedCallback(color => _module.FadeColor = color.newValue);
            duration.RegisterValueChangedCallback(duration => _module.FadeDuration = duration.newValue);
            fadeType.RegisterValueChangedCallback(fadeType => _module.FadeType = (FadeTypes)fadeType.newValue);
            easePresets.RegisterValueChangedCallback(easePreset => _module.EaseTypes = (EaseTypes)easePreset.newValue);
            useCurve.RegisterValueChangedCallback(useCurve =>
            {
                _module.UseCurve = useCurve.newValue;
                animationCurve.style.display = useCurve.newValue ? DisplayStyle.Flex : DisplayStyle.None;
                easePresets.style.display = useCurve.newValue ? DisplayStyle.None : DisplayStyle.Flex;
            });
            animationCurve.RegisterValueChangedCallback(animationCurve => _module.AnimationCurve = animationCurve.newValue);
            startDelay.RegisterValueChangedCallback(startDelay =>
            {
                _module.StartDelay = startDelay.newValue;
                delayInfo.text = "Delay: " + startDelay.newValue.ToString("F2") + " seconds";
            });
            probability.RegisterValueChangedCallback(probability => _module.Probability = probability.newValue);
            repetitions.RegisterValueChangedCallback(repetitions => _module.Repetitions = repetitions.newValue);
            repetitionDelay.RegisterValueChangedCallback(repetitionDelay => _module.RepeatDelay = repetitionDelay.newValue);
            runningUntilStopped.RegisterValueChangedCallback(untilStopped => _module._runningUntilStopped = untilStopped.newValue);

            removeButton.clicked += () => removeModule?.Invoke();

            return root;
        }
    }
}