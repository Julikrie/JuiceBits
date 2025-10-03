using System;
using Unity.Cinemachine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace JuiceBits
{
    public class ZoomUI
    {
        private ZoomModule _module;
        private VisualTreeAsset _userInterface;

        public ZoomUI(ZoomModule module, VisualTreeAsset userInterface)
        {
            _module = module;
            _userInterface = userInterface;
        }

        // Creates the whole interface for the module
        public VisualElement CreateInterface(Action removeModule = null)
        {
            // Everything beneath the root gets drawn as UI
            VisualElement root = _userInterface.CloneTree();

            // Searches for the root with "Name" and assigns it to the variable
            Foldout mainFoldout = root.Q<Foldout>("MainFoldout");
            Foldout extrasFoldout = root.Q<Foldout>("Extras");
            Foldout foldout = root.Q<Foldout>("MainFoldout");
            Label moduleName = root.Q<Label>("ModuleName");
            Label delayInfo = root.Q<Label>("DelayInfo");
            TextField name = root.Q<TextField>("ZoomName");
            TextField stopID = root.Q<TextField>("ID");
            IntegerField repetitions = root.Q<IntegerField>("Repetitions");
            FloatField targetValue = root.Q<FloatField>("TargetValue");
            FloatField zoomBuffer = root.Q<FloatField>("ZoomBuffer");
            FloatField zoomOutDuration = root.Q<FloatField>("ZoomOutDuration");
            FloatField zoomInDuration = root.Q<FloatField>("ZoomInDuration");
            FloatField startDelay = root.Q<FloatField>("StartDelay");
            FloatField probability = root.Q<FloatField>("Probability");
            FloatField repetitionDelay = root.Q<FloatField>("RepetitionDelay");
            BaseBoolField runningUntilStopped = root.Q<BaseBoolField>("RunningUntilStopped");
            BaseBoolField useCurve = root.Q<BaseBoolField>("UseDampingCurve");
            EnumField easePresets = root.Q<EnumField>("EasePresets");
            CurveField animationCurve = root.Q<CurveField>("ZoomDampingCurve");
            ObjectField camera = root.Q<ObjectField>("Camera");
            Button removeButton = root.Q<Button>("RemoveButton");

            // Assigns the values of the module to the variables
            mainFoldout.value = _module.IsMainOpen;
            extrasFoldout.value = _module.IsExtrasOpen;
            foldout.value = _module.IsMainOpen;
            name.value = _module.Name;
            moduleName.text = _module.Name;
            startDelay.value = _module.StartDelay;
            probability.value = _module.Probability;
            repetitions.value = _module.Repetitions;
            repetitionDelay.value = _module.RepeatDelay;
            easePresets.value = _module.EaseTypes;
            targetValue.value = _module.TargetValue;
            useCurve.value = _module.UseCurve;
            animationCurve.value = _module.AnimationCurve;
            zoomBuffer.value = _module.ZoomBuffer;
            zoomOutDuration.value = _module.ZoomOutDuration;
            zoomInDuration.value = _module.ZoomInDuration;
            stopID.value = _module.ID;
            camera.value = _module.CinemachineCamera;
            runningUntilStopped.value = _module._runningUntilStopped;
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
            easePresets.Init(_module.EaseTypes);
            animationCurve.style.display = _module.UseCurve ? DisplayStyle.Flex : DisplayStyle.None;
            zoomInDuration.tooltip = "The Time it takes to Zoom In";
            zoomOutDuration.tooltip = "The Time it takes to Zoom Out";
            zoomBuffer.tooltip = "Time before the Zoom returns to the original values";
            targetValue.tooltip = "The destination of the Zoom";

            // Turns off/on field display when UseCurve checkbox is ticked
            animationCurve.style.display = _module.UseCurve ? DisplayStyle.Flex : DisplayStyle.None;
            easePresets.style.display = _module.UseCurve ? DisplayStyle.None : DisplayStyle.Flex;

            // Registers value changes and saves them
            mainFoldout.RegisterValueChangedCallback(mainFoldout => _module.IsMainOpen = mainFoldout.newValue);
            extrasFoldout.RegisterValueChangedCallback(extrasFoldout => _module.IsExtrasOpen = extrasFoldout.newValue);
            camera.RegisterValueChangedCallback(camera => _module.CinemachineCamera = (CinemachineCamera)camera.newValue);
            startDelay.RegisterValueChangedCallback(startDelay =>
            {
                _module.StartDelay = startDelay.newValue; 
                delayInfo.text = "Delay: " + startDelay.newValue.ToString("F2") + " seconds";
            });
            probability.RegisterValueChangedCallback(probability => _module.Probability = probability.newValue);
            repetitions.RegisterValueChangedCallback(repetitions => _module.Repetitions = repetitions.newValue);
            repetitionDelay.RegisterValueChangedCallback(repetitionDelay => _module.RepeatDelay = repetitionDelay.newValue);
            name.RegisterValueChangedCallback(name =>
            {
                _module.Name = name.newValue;
                moduleName.text = _module.Name;
            });
            easePresets.RegisterValueChangedCallback(easePresets => _module.EaseTypes = (EaseTypes)easePresets.newValue);
            targetValue.RegisterValueChangedCallback(targetValue => _module.TargetValue = targetValue.newValue);
            useCurve.RegisterValueChangedCallback(useCurve =>
            {
                _module.UseCurve = useCurve.newValue;
                animationCurve.style.display = useCurve.newValue ? DisplayStyle.Flex : DisplayStyle.None;
                easePresets.style.display = useCurve.newValue ? DisplayStyle.None : DisplayStyle.Flex;
            });
            animationCurve.RegisterValueChangedCallback(animationCurve => _module.AnimationCurve = animationCurve.newValue);
            zoomBuffer.RegisterValueChangedCallback(zoomBuffer => _module.ZoomBuffer = zoomBuffer.newValue);
            zoomOutDuration.RegisterValueChangedCallback(zoomOutDuration => _module.ZoomOutDuration = zoomOutDuration.newValue);
            zoomInDuration.RegisterValueChangedCallback(zoomInDuration => _module.ZoomInDuration = zoomInDuration.newValue);
            stopID.RegisterValueChangedCallback(stopID => _module.ID = stopID.newValue);
            runningUntilStopped.RegisterValueChangedCallback(untilStopped => _module._runningUntilStopped = untilStopped.newValue);

            removeButton.clicked += () => removeModule?.Invoke();

            return root;
        }
    }
}