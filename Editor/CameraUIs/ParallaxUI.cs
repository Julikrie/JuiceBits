using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace JuiceBits
{
    public class ParallaxUI
    {
        private ParallaxModule _module;
        private VisualTreeAsset _userInterface;
        private VisualTreeAsset _parallaxContent;

        public ParallaxUI(ParallaxModule module, VisualTreeAsset userInterface)
        {
            _module = module;
            _userInterface = userInterface;
        }

        public VisualElement CreateInterface(System.Action removeModule = null)
        {
            _parallaxContent = Resources.Load<VisualTreeAsset>("UXML/Camera/ParallaxContentVisualTree");

            // Everything beneath the root gets drawn as UI
            VisualElement root = _userInterface.CloneTree();

            // Searches for the root with "Name" and assigns it to the variable
            Foldout mainFoldout = root.Q<Foldout>("MainFoldout");
            ListView parallaxList = root.Q<ListView>("ParallaxList");
            Button removeButton = root.Q<Button>("RemoveButton");

            // Creates the List for the parallax
            parallaxList.itemsSource = _module.ParallaxList;
            parallaxList.selectionType = SelectionType.Single;

            parallaxList.makeItem = () => _parallaxContent.CloneTree();

            parallaxList.bindItem = (parallaxContent, index) =>
            {
                var parallax = _module.ParallaxList[index];

                // Binds the fields to the list view
                Foldout parallaxFoldout = parallaxContent.Q<Foldout>("ParallaxFoldout");
                TextField name = parallaxContent.Q<TextField>("Name");
                ObjectField parallaxLayer = parallaxContent.Q<ObjectField>("ParallaxLayer");
                Vector2Field speed = parallaxContent.Q<Vector2Field>("Speed");
                Button removeModule = parallaxContent.Q<Button>("RemoveModule");

                // Assigns the values of the module to the variables
                parallaxFoldout.text = parallax.Name;
                name.value = parallax.Name;
                parallaxLayer.value = parallax.Layer;
                speed.value = parallax.LayerSpeed;

                // Tooltips
                name.tooltip = "Diplayed Name of the Parallax";
                parallaxLayer.tooltip = "The Target of the Parallax Effect";
                speed.tooltip = "The moving speed of the Parallax. Only choose X-Speed or Y-Speed, not both at the same time";

                // Saves the open/closed state of the folodut
                parallaxFoldout.value = parallax.IsOpen;
                mainFoldout.value = _module.IsMainOpen;
                parallaxFoldout.RegisterValueChangedCallback(parallaxFoldout => parallax.IsOpen = parallaxFoldout.newValue);
                mainFoldout.RegisterValueChangedCallback(mainFoldout => _module.IsMainOpen = mainFoldout.newValue);
                name.RegisterValueChangedCallback(name =>
                {
                    parallax.Name = name.newValue;
                    parallaxFoldout.text = parallax.Name;
                });
                parallaxLayer.RegisterValueChangedCallback(parallaxLayer => parallax.Layer = (Transform)parallaxLayer.newValue);
                speed.RegisterValueChangedCallback(speed => parallax.LayerSpeed = speed.newValue);

                // Remove button for list entries
                removeModule.clickable = new Clickable(() =>
                {
                    _module.ParallaxList.Remove(parallax);
                    parallaxList.RefreshItems();
                });
            };

            // Adds the fields to the list view
            parallaxList.onAdd = view =>
            {
                var itemsSourceCount = view.itemsSource.Count;

                _module.ParallaxList.Add(new ParallaxModule.Parallax
                {
                    Name = $"Parallax",
                    Layer = null,
                    LayerSpeed = Vector2.zero
                });
                view.RefreshItems();
                view.ScrollToItem(itemsSourceCount);
            };

            removeButton.clicked += () => removeModule?.Invoke();

            return root;
        }
    }
}