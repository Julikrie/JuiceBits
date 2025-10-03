using Unity.Cinemachine;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace JuiceBits
{
    public class OffsetUI
    {
        private OffsetModule _module;
        private VisualTreeAsset _userInterface;

        public OffsetUI(OffsetModule module, VisualTreeAsset userInterface)
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
            TextField name = root.Q<TextField>("OffsetName");
            TextField stopID = root.Q<TextField>("ID");
            IntegerField repetitions = root.Q<IntegerField>("Repetitions");
            FloatField startDelay = root.Q<FloatField>("StartDelay");
            FloatField probability = root.Q<FloatField>("Probability");
            FloatField repetitionDelay = root.Q<FloatField>("RepetitionDelay");
            FloatField duration = root.Q<FloatField>("OffsetDuration");
            FloatField timeBeforeReturn = root.Q<FloatField>("TimeBeforeReturn");
            Vector3Field cameraOffset = root.Q<Vector3Field>("Offset");
            BaseBoolField stayAtOffset = root.Q<BaseBoolField>("StayAtOffset");
            BaseBoolField useCurve = root.Q<BaseBoolField>("UseCurve");
            BaseBoolField runningUntilStopped = root.Q<BaseBoolField>("RunningUntilStopped");
            ObjectField camera = root.Q<ObjectField>("Camera");
            CurveField animationCurve = root.Q<CurveField>("AnimationCurve");
            EnumField easePresets = root.Q<EnumField>("EasePresets");
            Button removeButton = root.Q<Button>("RemoveButton");

            // Assigns the values of the module to the variables
            mainFoldout.value = _module.IsMainOpen;
            extrasFoldout.value = _module.IsExtrasOpen;
            name.value = _module.Name;
            stopID.value = _module.ID;
            camera.value = _module.Camera;
            useCurve.value = _module.UseCurve;
            animationCurve.value = _module.AnimationCurve;
            easePresets.value = _module.EaseTypes;
            cameraOffset.value = _module.Offset;
            duration.value = _module.Duration;
            timeBeforeReturn.value = _module.TimeBeforeReturn;
            stayAtOffset.value = _module.StayAtOffset;
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
            easePresets.tooltip = "Curve Presets to choose from";
            delayInfo.tooltip = "Information about the Delay of the Module";
            stayAtOffset.tooltip = "Stays at the Offset forever";
            camera.tooltip = "The target Camera of the Offset";
            cameraOffset.tooltip = "Direction of the Offset";
            timeBeforeReturn.tooltip = "Time before the Offset returns back to the original values";

            // Initializes values of the Enum
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
            camera.RegisterValueChangedCallback(camera => _module.Camera = (CinemachineCamera)camera.newValue);
            useCurve.RegisterValueChangedCallback(useCurve =>
            {
                _module.UseCurve = useCurve.newValue;
                animationCurve.style.display = useCurve.newValue ? DisplayStyle.Flex : DisplayStyle.None;
                easePresets.style.display = useCurve.newValue ? DisplayStyle.None : DisplayStyle.Flex;
            });
            animationCurve.RegisterValueChangedCallback(animationCurve => _module.AnimationCurve = animationCurve.newValue);
            easePresets.RegisterValueChangedCallback(easePresets => _module.EaseTypes = (EaseTypes)easePresets.newValue);
            cameraOffset.RegisterValueChangedCallback(cameraOffset => _module.Offset = cameraOffset.newValue);
            duration.RegisterValueChangedCallback(duration => _module.Duration = duration.newValue);
            timeBeforeReturn.RegisterValueChangedCallback(timeBeforeReturn => _module.TimeBeforeReturn = timeBeforeReturn.newValue);
            stayAtOffset.RegisterValueChangedCallback(stayAtOffset => _module.StayAtOffset = stayAtOffset.newValue);
            startDelay.RegisterValueChangedCallback(startDelay =>
            {
                _module.StartDelay = startDelay.newValue;
                delayInfo.text = "Delay: " + startDelay.newValue.ToString("F2") + " seconds";
            });
            probability.RegisterValueChangedCallback(probability => _module.Probability = probability.newValue);
            repetitions.RegisterValueChangedCallback(repetitions => _module.Repetitions = repetitions.newValue);
            repetitionDelay.RegisterValueChangedCallback(repetitionDelay => _module.RepeatDelay = repetitionDelay.newValue);
            runningUntilStopped.RegisterValueChangedCallback(untilStopped =>
            {
                _module._runningUntilStopped = untilStopped.newValue;

                repetitionDelay.SetEnabled(!untilStopped.newValue);
                repetitions.SetEnabled(!untilStopped.newValue);

                if (untilStopped.newValue)
                {
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