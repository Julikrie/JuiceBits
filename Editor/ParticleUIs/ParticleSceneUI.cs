using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace JuiceBits
{
    public class ParticleSceneUI
    {
        private ParticleSceneModule _module;
        private VisualTreeAsset _userInterface;

        public ParticleSceneUI(ParticleSceneModule module, VisualTreeAsset userInterface)
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
            TextField name = root.Q<TextField>("Name");
            TextField stopID = root.Q<TextField>("ID");
            FloatField activateTime = root.Q<FloatField>("ActivateTime");
            FloatField deactivateTime = root.Q<FloatField>("DeactivateTime");
            FloatField startDelay = root.Q<FloatField>("StartDelay");
            FloatField minEmission = root.Q<FloatField>("MinEmissionRate");
            FloatField maxEmission = root.Q<FloatField>("MaxEmissionRate");
            FloatField minDuration = root.Q<FloatField>("MinDuration");
            FloatField maxDuration = root.Q<FloatField>("MaxDuration");
            FloatField minLifetime = root.Q<FloatField>("MinLifetime");
            FloatField maxLifetime = root.Q<FloatField>("MaxLifetime");
            FloatField minSize = root.Q<FloatField>("MinSize");
            FloatField maxSize = root.Q<FloatField>("MaxSize");
            FloatField minRotation = root.Q<FloatField>("MinRotation");
            FloatField maxRotation = root.Q<FloatField>("MaxRotation");
            Vector3Field offset = root.Q<Vector3Field>("Offset");
            BaseBoolField setAsParent = root.Q<BaseBoolField>("SetAsParent");
            BaseBoolField samePosition = root.Q<BaseBoolField>("SamePosition");
            BaseBoolField activateParticle = root.Q<BaseBoolField>("ActivateParticle");
            BaseBoolField deactivateParticle = root.Q<BaseBoolField>("DeactivateParticle");
            BaseBoolField useRandomizer = root.Q<BaseBoolField>("Randomizer");
            ObjectField target = root.Q<ObjectField>("Target");
            ListView particleList = root.Q<ListView>("ParticleList");
            ListView activateList = root.Q<ListView>("ActivateAtIndexList");
            ListView deactivateList = root.Q<ListView>("DeactivateAtIndexList");
            ListView randomColor = root.Q<ListView>("RandomColor");
            ListView randomMaterial = root.Q<ListView>("RandomMaterial");
            Button removeButton = root.Q<Button>("RemoveButton");

            // Connects the values of the modules to the variables
            mainFoldout.value = _module.IsMainOpen;
            extrasFoldout.value = _module.IsExtrasOpen;
            name.value = _module.Name;
            moduleName.text = _module.Name;
            stopID.value = _module.ID;
            target.value = _module.Target;
            offset.value = _module.ParticleOffset;
            setAsParent.value = _module.SetAsParent;
            samePosition.value = _module.SamePosition;
            activateParticle.value = _module.ActivateParticle;
            deactivateParticle.value = _module.DeactivateParticle;
            activateTime.value = _module.ActivateTime;
            deactivateTime.value = _module.DeactivateTime;
            useRandomizer.value = _module.Randomizer.UseRandom;
            minEmission.value = _module.Randomizer.MinEmission;
            maxEmission.value = _module.Randomizer.MaxEmission;
            minDuration.value = _module.Randomizer.MinDuration;
            maxDuration.value = _module.Randomizer.MaxDuration;
            minLifetime.value = _module.Randomizer.MinLifetime;
            maxLifetime.value = _module.Randomizer.MaxLifetime;
            minSize.value = _module.Randomizer.MinSize;
            maxSize.value = _module.Randomizer.MaxSize;
            minRotation.value = _module.Randomizer.MinRotation;
            maxRotation.value = _module.Randomizer.MaxRotation;
            startDelay.value = _module.StartDelay;
            delayInfo.text = "Delay: " + _module.StartDelay.ToString("F2") + " seconds";

            // Tooltips
            activateParticle.tooltip = "Particles can now be activated in the Scene";
            deactivateParticle.tooltip = "Particles can now be deactivated in the Scene";
            activateTime.tooltip = "The time before the Particle System activates";
            deactivateTime.tooltip = "The time before the Particle System deactivates";
            name.tooltip = "Name of the Module";
            stopID.tooltip = "Use a string to stop with StopModuleByID(string)";
            startDelay.tooltip = "Delay to the predecessor Module";
            target.tooltip = "The Target of the Particle System";
            delayInfo.tooltip = "Information about the Delay of the Module";
            offset.tooltip = "The Offset of the Particle System at the Location";
            setAsParent.tooltip = "Sets the Target as Parent of the Particle System";
            samePosition.tooltip = "Sets the Position of the Particle System to the Target Position";
            useRandomizer.tooltip = "Allows the the use of the random Values below";
            minEmission.tooltip = "The minimal Emission Rate of the Particle System";
            maxEmission.tooltip = "The maximal Emission Rate of the Particle System";
            minDuration.tooltip = "The minmal Duration of the Particle System";
            maxDuration.tooltip = "The maximal Duration of the Particle System";
            minLifetime.tooltip = "The minmal Lifetime of the Particle System";
            maxLifetime.tooltip = "The maximal Lifetime of the Particle System";
            minSize.tooltip = "The maximal Size of the Particle in the Particle System";
            maxSize.tooltip = "The minmal Size of the Particle in the Particle System";
            minRotation.tooltip = "The minimal Rotation of the Particle in the Particle System";
            maxRotation.tooltip = "The maximal Rotation of the Particle in the Particle System";
            randomColor.tooltip = "The Particle System chooses a random Color";
            randomMaterial.tooltip = "The Particle System chooses a random Material";

            // Creating the list content
            particleList.itemsSource = _module.Particle;
            particleList.fixedItemHeight = 20;
            particleList.makeItem = () => new ObjectField { objectType = typeof(ParticleSystem) };
            particleList.bindItem = (element, i) =>
            {
                var field = (ObjectField)element;
                field.value = _module.Particle[i];
                field.RegisterValueChangedCallback(particle =>
                {
                    _module.Particle[i] = particle.newValue as ParticleSystem;
                });
            };

            particleList.selectionType = SelectionType.None;

            activateList.itemsSource = _module.ActivateAtIndex;
            activateList.fixedItemHeight = 20;
            activateList.makeItem = () => new IntegerField { style = { flexGrow = 1 } };
            activateList.bindItem = (element, i) =>
            {
                var field = (IntegerField)element;
                field.value = _module.ActivateAtIndex[i];
                field.RegisterValueChangedCallback(index =>
                {
                    _module.ActivateAtIndex[i] = index.newValue;
                });
            };

            activateList.selectionType = SelectionType.None;

            deactivateList.itemsSource = _module.DeactivateAtIndex;
            deactivateList.fixedItemHeight = 20;
            deactivateList.makeItem = () => new IntegerField { style = { flexGrow = 1 } };
            deactivateList.bindItem = (element, i) =>
            {
                var field = (IntegerField)element;
                field.value = _module.DeactivateAtIndex[i];
                field.RegisterValueChangedCallback(index =>
                {
                    _module.DeactivateAtIndex[i] = index.newValue;
                });
            };

            deactivateList.selectionType = SelectionType.None;

            randomColor.itemsSource = _module.Randomizer.RandomColor;
            randomColor.fixedItemHeight = 20;
            randomColor.makeItem = () => new ColorField { style = { flexGrow = 1 } };
            randomColor.bindItem = (element, i) =>
            {
                var field = (ColorField)element;
                field.value = _module.Randomizer.RandomColor[i];
                field.RegisterValueChangedCallback(color =>
                {
                    _module.Randomizer.RandomColor[i] = color.newValue;
                });
            };
            randomColor.selectionType = SelectionType.None;

            randomMaterial.itemsSource = _module.Randomizer.RandomMaterial;
            randomMaterial.fixedItemHeight = 20;
            randomMaterial.makeItem = () => new ObjectField { objectType = typeof(Material), style = { flexGrow = 1 } };
            randomMaterial.bindItem = (element, i) =>
            {
                var field = (ObjectField)element;
                field.value = _module.Randomizer.RandomMaterial[i];
                field.RegisterValueChangedCallback(material =>
                {
                    _module.Randomizer.RandomMaterial[i] = material.newValue as Material;
                });
            };
            randomMaterial.selectionType = SelectionType.None;

            // Registers value changes and saves them
            name.RegisterValueChangedCallback(name =>
            {
                _module.Name = name.newValue;
                moduleName.text = _module.Name;
            });
            useRandomizer.RegisterValueChangedCallback(useRandomizer => _module.Randomizer.UseRandom = useRandomizer.newValue);
            minEmission.RegisterValueChangedCallback(minEmissionRate => _module.Randomizer.MinEmission = minEmissionRate.newValue);
            maxEmission.RegisterValueChangedCallback(maxEmissionRate => _module.Randomizer.MaxEmission = maxEmissionRate.newValue);
            minDuration.RegisterValueChangedCallback(minDuration => _module.Randomizer.MinDuration = minDuration.newValue);
            maxDuration.RegisterValueChangedCallback(maxDuration => _module.Randomizer.MaxDuration = maxDuration.newValue);
            minLifetime.RegisterValueChangedCallback(minLifetime => _module.Randomizer.MinLifetime = minLifetime.newValue);
            maxLifetime.RegisterValueChangedCallback(maxLifetime => _module.Randomizer.MaxLifetime = maxLifetime.newValue);
            minSize.RegisterValueChangedCallback(minSize => _module.Randomizer.MinSize = minSize.newValue);
            maxSize.RegisterValueChangedCallback(maxSize => _module.Randomizer.MaxSize = maxSize.newValue);
            minRotation.RegisterValueChangedCallback(minRotation => _module.Randomizer.MinRotation = minRotation.newValue);
            maxRotation.RegisterValueChangedCallback(maxRotation => _module.Randomizer.MaxRotation = maxRotation.newValue);
            mainFoldout.RegisterValueChangedCallback(mainFoldout => _module.IsMainOpen = mainFoldout.newValue);
            extrasFoldout.RegisterValueChangedCallback(extrasFoldout => _module.IsExtrasOpen = extrasFoldout.newValue);
            stopID.RegisterValueChangedCallback(stopID => _module.ID = stopID.newValue);
            target.RegisterValueChangedCallback(target => _module.Target = (GameObject)target.newValue);
            offset.RegisterValueChangedCallback(offset => _module.ParticleOffset = offset.newValue);
            setAsParent.RegisterValueChangedCallback(setAsParent => _module.SetAsParent = setAsParent.newValue);
            samePosition.RegisterValueChangedCallback(samePosition => _module.SamePosition = samePosition.newValue);
            activateParticle.RegisterValueChangedCallback(activateParticle => _module.ActivateParticle = activateParticle.newValue);
            deactivateParticle.RegisterValueChangedCallback(deactivateParticle => _module.DeactivateParticle = deactivateParticle.newValue);
            activateTime.RegisterValueChangedCallback(activateTime => _module.ActivateTime = activateTime.newValue);
            deactivateTime.RegisterValueChangedCallback(deactivateTime => _module.DeactivateTime = deactivateTime.newValue);
            startDelay.RegisterValueChangedCallback(startDelay => _module.StartDelay = startDelay.newValue);
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