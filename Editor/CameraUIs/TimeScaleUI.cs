using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace JuiceBits
{
    public class TimeScaleUI
    {
        private TimeScaleModule _module;
        private VisualTreeAsset _userInterface;

        public TimeScaleUI(TimeScaleModule module, VisualTreeAsset userInterface)
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
            TextField name = root.Q<TextField>("TimeScaleName");
            TextField stopID = root.Q<TextField>("ID");
            IntegerField repetitions = root.Q<IntegerField>("Repetitions");
            FloatField duration = root.Q<FloatField>("TimeScaleDuration");
            FloatField startScale = root.Q<FloatField>("StartScale");
            FloatField endScale = root.Q<FloatField>("EndScale");
            FloatField startDelay = root.Q<FloatField>("StartDelay");
            FloatField probability = root.Q<FloatField>("Probability");
            FloatField repetitionDelay = root.Q<FloatField>("RepetitionDelay");
            FloatField timeScale = root.Q<FloatField>("TimeScale");
            BaseBoolField useCurve = root.Q<BaseBoolField>("UseCurve");
            BaseBoolField timeScaleInstantly = root.Q<BaseBoolField>("TimeScaleInstantly");
            BaseBoolField runningUntilStopped = root.Q<BaseBoolField>("RunningUntilStopped");
            CurveField animationCurve = root.Q<CurveField>("AnimationCurve");
            EnumField easePresets = root.Q<EnumField>("EaseTypes");
            Button removeButton = root.Q<Button>("RemoveButton");

            // Assigns the values of the module to the variables
            mainFoldout.value = _module.IsMainOpen;
            extrasFoldout.value = _module.IsExtrasOpen;
            name.value = _module.Name;
            moduleName.text = _module.Name;
            stopID.value = _module.ID;
            easePresets.value = _module.EaseTypes;
            animationCurve.value = _module.AnimationCurve;
            duration.value = _module.Duration;
            startScale.value = _module.StartScale;
            endScale.value = _module.EndScale;
            timeScale.value = _module.TimeScale;
            useCurve.value = _module.UseCurve;
            timeScaleInstantly.value = _module.TimeScaleInstantly;
            runningUntilStopped.value = _module._runningUntilStopped;
            startDelay.value = _module.StartDelay;
            probability.value = _module.Probability;
            repetitions.value = _module.Repetitions;
            repetitionDelay.value = _module.RepeatDelay;
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
            easePresets.tooltip = "Curve Presets to choose from";
            delayInfo.tooltip = "Information about the Delay of the Module";
            duration.tooltip = "The Duration of the Time Scale";
            startScale.tooltip = "The Time Scale at the beginning of the effect";
            endScale.tooltip = "The Time Scale at the end of the effect";
            timeScaleInstantly.tooltip = "Instantly jumps to the chosen Scale";

            easePresets.Init(_module.EaseTypes);

            // Saves the foldout open/closed state
            mainFoldout.RegisterValueChangedCallback(mainFoldout => _module.IsMainOpen = mainFoldout.newValue);
            extrasFoldout.RegisterValueChangedCallback(extrasFoldout => _module.IsExtrasOpen = extrasFoldout.newValue);

            // Registers value changes and saves them
            name.RegisterValueChangedCallback(name =>
            {
                _module.Name = name.newValue;
                moduleName.text = _module.Name;
            });
            stopID.RegisterValueChangedCallback(stopID => _module.ID = stopID.newValue);
            easePresets.RegisterValueChangedCallback(easePresets => _module.EaseTypes = (EaseTypes)easePresets.newValue);
            animationCurve.RegisterValueChangedCallback(animationCurve => _module.AnimationCurve = animationCurve.newValue);
            duration.RegisterValueChangedCallback(duration => _module.Duration = duration.newValue);
            startScale.RegisterValueChangedCallback(startScale => _module.StartScale = startScale.newValue);
            endScale.RegisterValueChangedCallback(endScale => _module.EndScale = endScale.newValue);
            timeScale.RegisterValueChangedCallback(timeScale => _module.TimeScale = timeScale.newValue);
            startDelay.RegisterValueChangedCallback(startDelay => _module.StartDelay = startDelay.newValue);
            probability.RegisterValueChangedCallback(probability => _module.Probability = probability.newValue);
            repetitions.RegisterValueChangedCallback(repetitions => _module.Repetitions = repetitions.newValue);
            repetitionDelay.RegisterValueChangedCallback(repetitionDelay => _module.RepeatDelay = repetitionDelay.newValue);

            animationCurve.style.display = _module.UseCurve ? DisplayStyle.Flex : DisplayStyle.None;
            startScale.style.display = _module.TimeScaleInstantly || _module._runningUntilStopped ? DisplayStyle.None : DisplayStyle.Flex;
            endScale.style.display = _module.TimeScaleInstantly || _module._runningUntilStopped ? DisplayStyle.None : DisplayStyle.Flex;
            timeScale.style.display = _module.TimeScaleInstantly || _module._runningUntilStopped ? DisplayStyle.Flex : DisplayStyle.None;

            repetitionDelay.SetEnabled(!_module._runningUntilStopped);
            repetitions.SetEnabled(!_module._runningUntilStopped);
            duration.SetEnabled(!_module._runningUntilStopped);
            timeScaleInstantly.SetEnabled(!_module._runningUntilStopped);
            useCurve.SetEnabled(!_module._runningUntilStopped);
            easePresets.SetEnabled(!_module._runningUntilStopped);
            animationCurve.SetEnabled(!_module._runningUntilStopped);

            useCurve.RegisterValueChangedCallback(useCurve =>
            {
                _module.UseCurve = useCurve.newValue;
                animationCurve.style.display = useCurve.newValue ? DisplayStyle.Flex : DisplayStyle.None;
                easePresets.style.display = useCurve.newValue ? DisplayStyle.None : DisplayStyle.Flex;
            });

            runningUntilStopped.RegisterValueChangedCallback(untilStopped =>
            {
                _module._runningUntilStopped = untilStopped.newValue;

                // The fields will be greyed out
                repetitionDelay.SetEnabled(!untilStopped.newValue);
                repetitions.SetEnabled(!untilStopped.newValue);
                duration.SetEnabled(!untilStopped.newValue);
                timeScaleInstantly.SetEnabled(!untilStopped.newValue);
                useCurve.SetEnabled(!untilStopped.newValue);
                easePresets.SetEnabled(!untilStopped.newValue);
                animationCurve.SetEnabled(!untilStopped.newValue);

                // Disables/Enables the fields
                timeScale.style.display = untilStopped.newValue ? DisplayStyle.Flex : DisplayStyle.None;
                startScale.style.display = untilStopped.newValue ? DisplayStyle.None : DisplayStyle.Flex;
                endScale.style.display = untilStopped.newValue ? DisplayStyle.None : DisplayStyle.Flex;

                if (untilStopped.newValue)
                {
                    repetitionDelay.value = 0f;
                    _module.RepeatDelay = 0f;

                    repetitions.value = 0;
                    _module.Repetitions = 0;

                    duration.value = 0f;
                    _module.Duration = 0f;

                    timeScaleInstantly.value = false;
                    _module.TimeScaleInstantly = false;
                }
            });

            timeScaleInstantly.RegisterValueChangedCallback(timeScaleInstantly =>
            {
                _module.TimeScaleInstantly = timeScaleInstantly.newValue;

                // The fields will be greyed out
                useCurve.SetEnabled(!timeScaleInstantly.newValue);
                easePresets.SetEnabled(!timeScaleInstantly.newValue);
                animationCurve.SetEnabled(!timeScaleInstantly.newValue);

                // Disables/Enables the fields
                startScale.style.display = timeScaleInstantly.newValue ? DisplayStyle.None : DisplayStyle.Flex;
                endScale.style.display = timeScaleInstantly.newValue ? DisplayStyle.None : DisplayStyle.Flex;
                timeScale.style.display = timeScaleInstantly.newValue ? DisplayStyle.Flex : DisplayStyle.None;
            });

            startDelay.RegisterValueChangedCallback(startDelay =>
            {
                _module.StartDelay = startDelay.newValue;
                delayInfo.text = "Delay: " + startDelay.newValue.ToString("F2") + " seconds";
            });

            removeButton.clicked += () => removeModule?.Invoke();

            return root;
        }
    }
}