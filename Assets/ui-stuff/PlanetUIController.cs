using UnityEngine;
using UnityEngine.UIElements;

public class PlanetUIController : MonoBehaviour
{
    private VisualElement _root;
    private PlanetController _planet;

    public void OnEnable() {
        _root = GetComponent<UIDocument>().rootVisualElement;
    }

    public void CustomSetup(PlanetController planet)
    {
        _planet = planet;
        if (planet.IsSettled())
        {
            _root.Q<Button>("BuyFarm").RegisterCallback<ClickEvent>(ev => _planet.CreateBuilding("Farm"));
            _root.Q<Button>("BuyReactor").RegisterCallback<ClickEvent>(ev => _planet.CreateBuilding("Reactor"));
            _root.Q<Button>("BuyAsteroid").RegisterCallback<ClickEvent>(ev => _planet.CreateBuilding("AsteroidMiner"));
            _root.Q<Button>("BuyMine").RegisterCallback<ClickEvent>(ev => _planet.CreateBuilding("Mine"));
            _root.Q<Button>("BuyFab").RegisterCallback<ClickEvent>(ev => _planet.CreateBuilding("FabricationPlant"));
            _root.Q<Button>("BuyTerraform").RegisterCallback<ClickEvent>(ev => _planet.CreateBuilding("Terraformer"));
            _root.Q<Button>("BuyHousing").RegisterCallback<ClickEvent>(ev => _planet.CreateBuilding("Housing"));
            _root.Q<Button>("BuySpaceStation").RegisterCallback<ClickEvent>(ev => _planet.CreateBuilding("SpaceStation"));
            _root.Q<Button>("Close").RegisterCallback<ClickEvent>(ev => GameManager.Instance.CloseCurrentUI());   
        }
    }

    public void SetUserInterfaceValues()
    {
        if (_planet != null && _planet.IsSettled())
        {
            int[] planetResources = _planet.inventoryController.GetResources();
            _root.Q<Label>("PlanetName").text = _planet.planetName;
            _root.Q<Label>("Habitability").text = $"Habitability: {_planet.GetHabitability().ToString()}";
            _root.Q<Label>("Population").text = $"Population: {_planet.GetPopulation().ToString()}";
            _root.Q<Label>("Food").text = $"Food: {planetResources[0].ToString()}";
            _root.Q<Label>("Energy").text = $"Energy: {planetResources[1].ToString()}";
            _root.Q<Label>("Minerals").text = $"Minerals: {planetResources[2].ToString()}";
            _root.Q<Label>("ConsumerGoods").text = $"Consumer Goods: {planetResources[3].ToString()}";

            var buildingCounter = _planet.GetBuildingCounter();
            _root.Q<Label>("Farm").text = $"Farms: {buildingCounter["Farm"]}";
            _root.Q<Label>("Reactor").text = $"Reactors: {buildingCounter["Reactor"]}";
            _root.Q<Label>("AsteroidMiner").text = $"Asteroid Miners: {buildingCounter["AsteroidMiner"]}";
            _root.Q<Label>("Mine").text = $"Mines: {buildingCounter["Mine"]}";
            _root.Q<Label>("FabricationPlant").text = $"Fabrication Plants: {buildingCounter["FabricationPlant"]}";
            _root.Q<Label>("Terraformer").text = $"Terraformers: {buildingCounter["Terraformer"]}";
            _root.Q<Label>("Housing").text = $"Housing: {buildingCounter["Housing"]}";
        }
        else if (_planet != null && !_planet.IsSettled())
        {
            _root.Q<Label>("PlanetName").text = _planet.planetName;
            _root.Q<Label>("Habitability").text = $"Habitability: {_planet.GetHabitability().ToString()}";
            _root.Q<Button>("Colonize").RegisterCallback<ClickEvent>(ev => _planet.SettlePlanet()); 
            _root.Q<Button>("Close").RegisterCallback<ClickEvent>(ev => GameManager.Instance.CloseCurrentUI()); 
        }
    }

    private void Update() {
        
    }
}
