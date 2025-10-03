using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace JuiceBits
{
    public class ParticleInstantiateUI
    {
        private ParticleInstantiateModule _module;
        private VisualTreeAsset _userInterface;

        public ParticleInstantiateUI(ParticleInstantiateModule module, VisualTreeAsset userInterface)
        {
            _module = module;
            _userInterface = userInterface;
        }

        // Creates the whole interface for the module
        public VisualElement CreateInterface(System.Action removeModule = null)
        {
            VisualElement root = _userInterface.CloneTree();

            // Searches for the root with "Name" and assigns it to the variable
            Foldout foldout = root.Q<Foldout>("MainFoldout");
            Label delayInfo = root.Q<Label>("DelayInfo");
            Label moduleName = root.Q<Label>("ModuleName");
            TextField name = root.Q<TextField>("Name");
            TextField stopID = root.Q<TextField>("ID");
            IntegerField defaultCapacity = root.Q<IntegerField>("DefaultCapacity");
            IntegerField maxCapacity = root.Q<IntegerField>("MaxCapacity");
            IntegerField repetitions = root.Q<IntegerField>("Repetitions");
            FloatField cooldownTime = root.Q<FloatField>("CooldownTime");
            FloatField startDelay = root.Q<FloatField>("StartDelay");
            FloatField probability = root.Q<FloatField>("Probability");
            FloatField repetitionDelay = root.Q<FloatField>("RepetitionDelay");
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
            ObjectField target = root.Q<ObjectField>("Target");
            ObjectField particlePrefab = root.Q<ObjectField>("ParticlePrefab");
            BaseBoolField useCooldown = root.Q<BaseBoolField>("UseCooldown");
            BaseBoolField setAsParent = root.Q<BaseBoolField>("SetAsParent");
            BaseBoolField usePooling = root.Q<BaseBoolField>("UsePooling");
            BaseBoolField useRandomizer = root.Q<BaseBoolField>("Randomizer");
            ListView randomColor = root.Q<ListView>("RandomColor");
            ListView randomMaterial = root.Q<ListView>("RandomMaterial");
            Button removeButton = root.Q<Button>("RemoveButton");

            // Assigns the values of the modules to the fields
            foldout.value = _module.IsMainOpen;
            name.value = _module.Name;
            moduleName.text = _module.Name;
            stopID.value = _module.ID;
            target.value = _module.Target;
            particlePrefab.value = _module.ParticlePrefab;
            particlePrefab.objectType = typeof(ParticleSystem);
            offset.value = _module.Offset;
            useCooldown.value = _module.UseCooldown;
            cooldownTime.value = _module.CooldownTime;
            setAsParent.value = _module.SetAsParent;
            usePooling.value = _module.UsePooling;
            defaultCapacity.value = _module.DefaultCapacity;
            maxCapacity.value = _module.MaxCapacity;
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
            target.tooltip = "The Target of the Particle System";
            delayInfo.tooltip = "Information about the Delay of the Module";
            particlePrefab.tooltip = "The Particle Prefab to use";
            offset.tooltip = "The Offset of the Particle System at the Location";
            useCooldown.tooltip = "Activates the use of Cooldown for Particles";
            cooldownTime.tooltip = "Time of the Particle before it can be used again";
            setAsParent.tooltip = "Sets the Target as Parent of the Particle System";
            usePooling.tooltip = "Activates the Pooling of Particle. Pooling deactivates the Particle and activates them again when needed";
            defaultCapacity.tooltip = "The default amount of Particles in the Pool";
            maxCapacity.tooltip = "The maximal amount of Particles in the Pool";
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

            // Creates list content
            randomColor.itemsSource = _module.Randomizer.RandomColor;
            randomColor.fixedItemHeight = 20;
            randomColor.makeItem = () => new ColorField { style = { flexGrow = 1 } };
            randomColor.bindItem = (element, i) =>
            {
                var colorField = (ColorField)element;
                colorField.value = _module.Randomizer.RandomColor[i];
                colorField.RegisterValueChangedCallback(color =>
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
                var materialField = (ObjectField)element;
                materialField.value = _module.Randomizer.RandomMaterial[i];
                materialField.RegisterValueChangedCallback(material =>
                {
                    _module.Randomizer.RandomMaterial[i] = material.newValue as Material;
                });
            };

            randomMaterial.selectionType = SelectionType.None;

            name.RegisterValueChangedCallback(name =>
            {
                _module.Name = name.newValue;
                moduleName.text = _module.Name;
            });
            foldout.RegisterValueChangedCallback(foldout => _module.IsMainOpen = foldout.newValue);
            stopID.RegisterValueChangedCallback(stopID => _module.ID = stopID.newValue);
            target.RegisterValueChangedCallback(target => _module.Target = (GameObject)target.newValue);
            particlePrefab.RegisterValueChangedCallback(particlePrefab => _module.ParticlePrefab = (ParticleSystem)particlePrefab.newValue);
            offset.RegisterValueChangedCallback(offset => _module.Offset = offset.newValue);
            useCooldown.RegisterValueChangedCallback(useCooldown => _module.UseCooldown = useCooldown.newValue);
            cooldownTime.RegisterValueChangedCallback(cooldownTime => _module.CooldownTime = cooldownTime.newValue);
            setAsParent.RegisterValueChangedCallback(setAsParent => _module.SetAsParent = setAsParent.newValue);
            usePooling.RegisterValueChangedCallback(usePooling => _module.UsePooling = usePooling.newValue);
            defaultCapacity.RegisterValueChangedCallback(defaultCapacity => _module.DefaultCapacity = defaultCapacity.newValue);
            maxCapacity.RegisterValueChangedCallback(maxCapacity => _module.MaxCapacity = maxCapacity.newValue);

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
            startDelay.RegisterValueChangedCallback(startDelay => _module.StartDelay = startDelay.newValue);
            probability.RegisterValueChangedCallback(probability => _module.Probability = probability.newValue);
            repetitions.RegisterValueChangedCallback(repetitions => _module.Repetitions = repetitions.newValue);
            repetitionDelay.RegisterValueChangedCallback(repetitionDelay => _module.RepeatDelay = repetitionDelay.newValue);
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