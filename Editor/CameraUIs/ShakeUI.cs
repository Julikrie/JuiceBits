using Unity.Cinemachine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace JuiceBits
{
    public class ShakeUI
    {
        private ShakeModule _module;
        private VisualTreeAsset _userInterface;

        public ShakeUI(ShakeModule module, VisualTreeAsset userInterface)
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
            TextField name = root.Q<TextField>("ShakeName");
            TextField stopID = root.Q<TextField>("ID");
            ObjectField camera = root.Q<ObjectField>("Camera");
            IntegerField repetitions = root.Q<IntegerField>("Repetitions");
            FloatField duration = root.Q<FloatField>("ShakeDuration");
            FloatField startDelay = root.Q<FloatField>("StartDelay");
            FloatField repetitionDelay = root.Q<FloatField>("RepetitionDelay");
            FloatField probability = root.Q<FloatField>("Probability");
            Vector3Field direction = root.Q<Vector3Field>("ShakeDirection");
            CurveField animationCurve = root.Q<CurveField>("AnimationCurve");
            EnumField shakeType = root.Q<EnumField>("ShakePreset");
            Button removeButton = root.Q<Button>("RemoveButton");

            // Assigns the values of the module to the variables
            mainFoldout.value = _module.IsMainOpen;
            extrasFoldout.value = _module.IsExtrasOpen;
            name.value = _module.Name;
            moduleName.text = _module.Name;
            stopID.value = _module.ID;
            camera.value = _module.CinemachineCamera;
            shakeType.value = _module.ShakeType;
            duration.value = _module.ShakeDuration;
            direction.value = _module.ShakeDirection;
            animationCurve.value = _module.AnimationCurve;
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
            animationCurve.tooltip = "Create own Animation Curve";
            delayInfo.tooltip = "Information about the Delay of the Module";
            shakeType.tooltip = "Different Shake Presets to choose from";
            duration.tooltip = "Duration of the Shake";
            direction.tooltip = "The Shake Direction";

            // Turns off/on animation curve field when shake type is custom
            animationCurve.style.display = _module.ShakeType == ShakeType.Custom ? DisplayStyle.Flex : DisplayStyle.None;

            // Registers value changes and saves them
            mainFoldout.RegisterValueChangedCallback(mainFoldout => _module.IsMainOpen = mainFoldout.newValue);
            extrasFoldout.RegisterValueChangedCallback(extrasFoldout => _module.IsExtrasOpen = extrasFoldout.newValue);
            startDelay.RegisterValueChangedCallback(evt =>
            {
                _module.StartDelay = evt.newValue;
                delayInfo.text = "Delay: " + evt.newValue.ToString("F2") + " seconds";
            });
            name.RegisterValueChangedCallback(evt =>
            {
                _module.Name = evt.newValue;
                moduleName.text = _module.Name;
            });
            stopID.RegisterValueChangedCallback(stopID => _module.ID = stopID.newValue);
            camera.RegisterValueChangedCallback(camera => _module.CinemachineCamera = (CinemachineCamera)camera.newValue);
            shakeType.RegisterValueChangedCallback(shakeType =>
            {
                _module.ShakeType = (ShakeType)shakeType.newValue;
                animationCurve.style.display = _module.ShakeType == ShakeType.Custom ? DisplayStyle.Flex : DisplayStyle.None;
            });
            duration.RegisterValueChangedCallback(duration => _module.ShakeDuration = duration.newValue);
            direction.RegisterValueChangedCallback(direction => _module.ShakeDirection = direction.newValue);
            animationCurve.RegisterValueChangedCallback(animationCurve => _module.AnimationCurve = animationCurve.newValue);
            probability.RegisterValueChangedCallback(probability => _module.Probability = probability.newValue);
            repetitions.RegisterValueChangedCallback(repetitions => _module.Repetitions = repetitions.newValue);
            repetitionDelay.RegisterValueChangedCallback(repetitionDelay => _module.RepeatDelay = repetitionDelay.newValue);

            removeButton.clicked += () => removeModule?.Invoke();

            return root;
        }
    }
}