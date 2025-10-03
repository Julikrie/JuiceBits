using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace JuiceBits
{
    public class HitFeedbackUI
    {
        private HitFeedbackModule _module;
        private VisualTreeAsset _userInterface;

        public HitFeedbackUI(HitFeedbackModule module, VisualTreeAsset userInterface)
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
            TextField name = root.Q<TextField>("HitFeedbackName");
            TextField stopID = root.Q<TextField>("ID");
            IntegerField repetitions = root.Q<IntegerField>("Repetitions");
            ColorField color = root.Q<ColorField>("FlashFeedbackColor");
            FloatField duration = root.Q<FloatField>("Duration");
            FloatField startDelay = root.Q<FloatField>("StartDelay");
            FloatField probability = root.Q<FloatField>("Probability");
            FloatField repetitionDelay = root.Q<FloatField>("RepetitionDelay");
            ObjectField target = root.Q<ObjectField>("Target");
            Button removeButton = root.Q<Button>("RemoveButton");

            // Assigns the values of the module to the variables
            mainFoldout.value = _module.IsMainOpen;
            extrasFoldout.value = _module.IsExtrasOpen;
            name.value = _module.Name;
            stopID.value = _module.ID;
            target.value = _module.Target;
            color.value = _module.FlashFeedbackColor;
            duration.value = _module.Duration;
            startDelay.value = _module.StartDelay;
            probability.value = _module.Probability;
            repetitions.value = _module.Repetitions;
            repetitionDelay.value = _module.RepeatDelay;
            moduleName.text = _module.Name;
            delayInfo.text = "Delay: " + _module.StartDelay.ToString("F2") + " seconds";

            // Tooltips
            name.tooltip = "Name of the Module";
            stopID.tooltip = "Use a string to stop with StopModuleByID(string)";
            repetitions.tooltip = "How often the Module repeats itself";
            startDelay.tooltip = "Delay to the predecessor Module";
            probability.tooltip = "The chance to trigger the Module";
            repetitionDelay.tooltip = "Delay between the Repetitons";
            color.tooltip = "The Color of the Hit Feedback";
            delayInfo.tooltip = "Information about the Delay of the Module";
            duration.tooltip = "The Duration of the Fade";
            target.tooltip = "The Target of the Hit Feedback";

            // Registers value changes and saves them
            mainFoldout.RegisterValueChangedCallback(mainFoldout => _module.IsMainOpen = mainFoldout.newValue);
            extrasFoldout.RegisterValueChangedCallback(extrasFoldout => _module.IsExtrasOpen = extrasFoldout.newValue);
            name.RegisterValueChangedCallback(name =>
            {
                _module.Name = name.newValue;
                moduleName.text = _module.Name;
            });
            stopID.RegisterValueChangedCallback(stopID => _module.ID = stopID.newValue);
            target.RegisterValueChangedCallback(target => _module.Target = (GameObject)target.newValue);
            color.RegisterValueChangedCallback(color => _module.FlashFeedbackColor = color.newValue);
            duration.RegisterValueChangedCallback(duration => _module.Duration = duration.newValue);
            startDelay.RegisterValueChangedCallback(startDelay =>
            {
                _module.StartDelay = startDelay.newValue;
                delayInfo.text = "Delay: " + startDelay.newValue.ToString("F2") + " seconds";
            });
            probability.RegisterValueChangedCallback(probability => _module.Probability = probability.newValue);
            repetitions.RegisterValueChangedCallback(repetitions => _module.Repetitions = repetitions.newValue);
            repetitionDelay.RegisterValueChangedCallback(repetitionDelay => _module.RepeatDelay = repetitionDelay.newValue);

            removeButton.clicked += () => removeModule?.Invoke();

            return root;
        }
    }
}