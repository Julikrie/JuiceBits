using UnityEngine.UIElements;

namespace JuiceBits
{
    public class TimeStopUI
    {
        private TimeStopModule _module;
        private VisualTreeAsset _userInterface;

        public TimeStopUI(TimeStopModule module, VisualTreeAsset userInterface)
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
            Label delayInfo = root.Q<Label>("DelayInfo");
            Label moduleName = root.Q<Label>("ModuleName");
            TextField name = root.Q<TextField>("TimeStopName");
            TextField stopID = root.Q<TextField>("ID");
            IntegerField repetitions = root.Q<IntegerField>("Repetitions");
            FloatField duration = root.Q<FloatField>("Duration");
            FloatField startDelay = root.Q<FloatField>("StartDelay");
            FloatField repetitionDelay = root.Q<FloatField>("RepetitionDelay");
            FloatField probability = root.Q<FloatField>("Probability");
            BaseBoolField runningUntilStopped = root.Q<BaseBoolField>("RunningUntilStopped");
            Button removeButton = root.Q<Button>("RemoveButton");

            // Assigns the values of the module to the variables
            mainFoldout.value = _module.IsMainOpen;
            extrasFoldout.value = _module.IsExtrasOpen;
            name.value = _module.Name;
            moduleName.text = _module.Name;
            stopID.value = _module.ID;
            duration.value = _module.Duration;
            startDelay.value = _module.StartDelay;
            probability.value = _module.Probability;
            repetitions.value = _module.Repetitions;
            repetitionDelay.value = _module.RepeatDelay;
            runningUntilStopped.value = _module._runningUntilStopped;
            delayInfo.text = "Delay: " + _module.StartDelay.ToString("F2") + " seconds";

            // Tooltips
            name.tooltip = "Name of the Module";
            stopID.tooltip = "Use a string to stop with StopModuleByID(string)";
            repetitions.tooltip = "How often the Module repeats itself";
            startDelay.tooltip = "Delay to the predecessor Module";
            probability.tooltip = "The chance to trigger the Module";
            repetitionDelay.tooltip = "Delay between the Repetitons";
            runningUntilStopped.tooltip = "Runs the Module until it is stopped with the StopID";
            delayInfo.tooltip = "Information about the Delay of the Module";
            duration.tooltip = "The Duration of the Time Stop";
            
            // The fields will be greyed out
            duration.SetEnabled(!_module._runningUntilStopped);
            repetitionDelay.SetEnabled(!_module._runningUntilStopped);
            repetitions.SetEnabled(!_module._runningUntilStopped);

            // Registers value changes and saves them
            mainFoldout.RegisterValueChangedCallback(mainFoldout => _module.IsMainOpen = mainFoldout.newValue);
            extrasFoldout.RegisterValueChangedCallback(extrasFoldout => _module.IsExtrasOpen = extrasFoldout.newValue);
            name.RegisterValueChangedCallback(name =>
            {
                _module.Name = name.newValue;
                moduleName.text = _module.Name;
            });
            stopID.RegisterValueChangedCallback(stopID => _module.ID = stopID.newValue);
            startDelay.RegisterValueChangedCallback(startDelay =>
            {
                _module.StartDelay = startDelay.newValue;
                delayInfo.text = "Delay: " + startDelay.newValue.ToString("F2") + " seconds";
            });
            probability.RegisterValueChangedCallback(probability => _module.Probability = probability.newValue);
            repetitions.RegisterValueChangedCallback(repetitions => _module.Repetitions = repetitions.newValue);
            repetitionDelay.RegisterValueChangedCallback(repetitionDelay => _module.RepeatDelay = repetitionDelay.newValue);
            duration.RegisterValueChangedCallback(duration => _module.Duration = duration.newValue);

            runningUntilStopped.RegisterValueChangedCallback(untilStoped =>
            {
                _module._runningUntilStopped = untilStoped.newValue;

                duration.SetEnabled(!untilStoped.newValue);
                repetitionDelay.SetEnabled(!untilStoped.newValue);
                repetitions.SetEnabled(!untilStoped.newValue);

                // Resets values of fields that are disabled
                if (untilStoped.newValue)
                {
                    duration.value = 0f;
                    _module.Duration = 0f;
                    repetitionDelay.value = 0f;
                    _module.RepeatDelay = 0f;
                    repetitions.value = 0;
                    _module.Repetitions = 0;
                }
            });

            removeButton.clicked += () => removeModule?.Invoke();

            return root;
        }
    }
}