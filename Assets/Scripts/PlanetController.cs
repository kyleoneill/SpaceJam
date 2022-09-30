using System;
using System.Collections.Generic;
using UnityEngine;

public class PlanetController : MonoBehaviour
{
    [SerializeField] private float habitability;
    public InventoryController inventoryController;
    private List<Building> _buildings;
    private float _maxPopulation;
    private float _currentPopulation;
    public bool _hasSpaceStation;
    [SerializeField] private bool isStarterPlanet = false;
    public int numberOfHouses;
    public string planetName;
    private bool _isSettled;
    private IDictionary<string, int> _buildingCounter;
    [SerializeField] private GameObject spaceStationObj;
    private GameObject _spaceStationInstance;
    [SerializeField] private GameObject spaceship;

    // Start is called before the first frame update
    void Start()
    {
        inventoryController = gameObject.AddComponent<InventoryController>();
        _buildings = new List<Building>();
        _hasSpaceStation = false;
        _maxPopulation = habitability * 100;
        numberOfHouses = 0;
        _buildingCounter = new Dictionary<string, int>();
        InitializeBuildingCounter();

        if (isStarterPlanet)
        {
            // The starter planet needs to start with some population and infrastructure
            inventoryController.AddResources(100, 100, 1000, 100);
            habitability = 0.9f;
            _currentPopulation = 25f;
            _isSettled = true;
            for (int i = 0; i < 5; i++)
            {
                CreateBuilding("Farm", true);
                CreateBuilding("Reactor", true);
                CreateBuilding("FabricationPlant", true);
            }
            GameManager.Instance.AddPlanet(this);
            GameManager.Instance.SetStarterPlanet(gameObject);
            CameraController.Instance.ResetCameraToStarterPlanet();
        }
        else
        {
            _currentPopulation = 0f;
            _isSettled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        GameManager.Instance.SelectPlanet(this);
    }

    public void SettlePlanet()
    {
        if (CanSettle())
        {
            _isSettled = true;
            _currentPopulation = 10f;
            inventoryController.AddResources(100, 100, 1000, 100);
            GameManager.Instance.CloseCurrentUI(); // This is a hack to reset state between unsettled and settled UIs
            GameManager.Instance.AddPlanet(this);
            
            // Create a ship that goes between this planet and the starting one
            GameObject spaceShip = Instantiate(spaceship);
            SpaceshipController spaceshipController = spaceShip.GetComponent<SpaceshipController>();
            spaceshipController.firstPlanet = GameManager.Instance.GetStarterPlanet();
            spaceshipController.secondPlanet = gameObject;
            spaceshipController.CustomStart();
        }
    }

    public void RunPlanetUpdates()
    {
        int intPopulation = (int)_currentPopulation;
        
        // Perform building jobs
        int jobsRemaining = intPopulation;
        foreach (var building in _buildings)
        {
            bool didWork = building.RunUpdateCycle(jobsRemaining);
            if (didWork)
                jobsRemaining -= 1;
        }
        
        // Perform population change
        // If we don't have enough food or consumer goods, we lose population
        // If we do have enough, and we have space for more people, increase population
        if (inventoryController.GetFood() < _currentPopulation ||
            inventoryController.GetConsumerGoods() < _currentPopulation)
        {
            _currentPopulation *= 0.9f;
        }
        else
        {
            if (_currentPopulation < _maxPopulation)
            {
                _currentPopulation = Math.Min(_currentPopulation * 1.05f, _maxPopulation);
            }
        }
        inventoryController.AddResources(-1 * intPopulation, 0, 0, -1 * intPopulation);
    }

    public float GetHabitability()
    {
        return habitability;
    }

    public float ChangeHabitability(float changeBy)
    {
        float tempVal = habitability + changeBy;
        if (tempVal > 1.0f)
        {
            tempVal = 1.0f;
        }
        else if (tempVal < 0)
        {
            tempVal = 0;
        }
        habitability = tempVal;
        _maxPopulation = (habitability * 100) + (numberOfHouses * 10);
        return habitability;
    }

    public void CreateBuilding(string buildingName, bool ignoreCost = false)
    {
        Building newBuilding;
        switch (buildingName)
        {
            case "Hydroponics":
                newBuilding = gameObject.AddComponent<Hydroponics>();
                break;
            case "Farm":
                newBuilding = gameObject.AddComponent<Farm>();
                break;
            case "Reactor":
                newBuilding = gameObject.AddComponent<Reactor>();
                break;
            case "AsteroidMiner":
                newBuilding = gameObject.AddComponent<AsteroidMiner>();
                break;
            case "SpaceStation":
                newBuilding = gameObject.AddComponent<SpaceStation>();
                break;
            case "Mine":
                newBuilding = gameObject.AddComponent<Mine>();
                break;
            case "FabricationPlant":
                newBuilding = gameObject.AddComponent<FabricationPlant>();
                break;
            case "Terraformer":
                newBuilding = gameObject.AddComponent<Terraformer>();
                break;
            case "Housing":
                newBuilding = gameObject.AddComponent<Housing>();
                break;
            default:
                Debug.Log($"Tried to create a building with the invalid name '{buildingName}'.");
                return;
        }
        newBuilding.planet = this;
        if (ignoreCost)
        {
            FinalizeBuilding(newBuilding, buildingName);
        }
        else if (newBuilding.CanBuild())
        {
            newBuilding.ProcessBuildCost();
            FinalizeBuilding(newBuilding, buildingName);
        }
        else
        {
            Destroy(newBuilding);
        }
    }

    private void InitializeBuildingCounter()
    {
        foreach (var buildingName in Building.BuildingList)
        {
            _buildingCounter[buildingName] = 0;
        }
    }

    private void FinalizeBuilding(Building newBuilding, string buildingName)
    {
        newBuilding.FinishedConstruction();
        _buildings.Add(newBuilding);
        _buildingCounter[buildingName] += 1;
    }

    public void AddSpaceStation()
    {
        GameObject planetObject = gameObject;
        
        // Create a space station and set its position
        _hasSpaceStation = true;
        GameObject spaceStation = Instantiate(spaceStationObj);
        Vector3 newPosition = planetObject.transform.position;
        newPosition.x -= 1.3f;
        newPosition.y -= 1.6f;
        spaceStation.transform.position = newPosition;
        
        // Set up space station orbit
        spaceStation.GetComponent<OrbitController>().SetParent(planetObject);
        
        _spaceStationInstance = spaceStation;
    }

    public float GetPopulation()
    {
        return _currentPopulation;
    }

    public bool IsSettled()
    {
        return _isSettled;
    }

    public List<Building> GetBuildings()
    {
        return _buildings;
    }

    public IDictionary<string, int> GetBuildingCounter()
    {
        return _buildingCounter;
    }

    public bool HasSpaceStation()
    {
        return _hasSpaceStation;
    }

    public bool CanSettle()
    {
        return habitability > 0f;
    }
}
