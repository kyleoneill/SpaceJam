using System;
using UnityEngine;

// AddResources order - food, energy, minerals, consumerGoods

public class Building : MonoBehaviour
{
    public static readonly string[] BuildingList = new string[]
        { "Hydroponics", "Farm", "Reactor", "AsteroidMiner", "SpaceStation", "Mine", "FabricationPlant", "Terraformer", "Housing" };

    protected bool IsResourceBearing;
    protected bool HasAction;
    public PlanetController planet;
    protected int MineralCost;
    protected int EnergyRunCost;
    
    private void Awake()
    {
        IsResourceBearing = false;
        HasAction = false;
        MineralCost = 5;
        EnergyRunCost = 1;
    }

    void Start()
    {
        
    }

    public virtual bool MeetsBuildingPrerequisite()
    {
        return true;
    }

    public bool RunUpdateCycle(int workersRemaining)
    {
        bool didWork = false;
        if (planet.inventoryController.GetEnergy() >= EnergyRunCost)
        {
            planet.inventoryController.ChangeEnergy(-1 * EnergyRunCost);
            if (IsResourceBearing && workersRemaining > 0)
            {
                ProcessResources(planet.inventoryController);
                didWork = true;
            }

            if (HasAction && workersRemaining > 0)
            {
                PerformAction();
                didWork = true;
            }   
        }
        return didWork;
    }

    public virtual bool CanBuild()
    {
        return planet.inventoryController.GetMinerals() > MineralCost && MeetsBuildingPrerequisite();
    }

    public virtual void ProcessBuildCost()
    {
        planet.inventoryController.ChangeMinerals(MineralCost * -1);
    }
    
    public virtual void ProcessResources(InventoryController planetInv)
    {
        Debug.Log(("Tried to add to inventory as a building which did not implement ProcessResources."));
    }

    public virtual void PerformAction()
    {
        Debug.Log("Tried to perform an action on a building which did not implement PerformAction.");
    }

    public virtual void FinishedConstruction()
    {
        
    }
}

public class Hydroponics : Building
{
    private void Awake()
    {
        IsResourceBearing = true;
        HasAction = false;
        MineralCost = 5;
        EnergyRunCost = 1;
    }
    
    public override void ProcessResources(InventoryController planetInv)
    {
        planetInv.ChangeFood(2);
    }
}

public class Farm : Building
{
    private void Awake()
    {
        IsResourceBearing = true;
        HasAction = false;
        MineralCost = 5;
        EnergyRunCost = 1;
    }

    public override void ProcessResources(InventoryController planetInv)
    {
        int modifyValue = Math.Max((int)Math.Pow(planet.GetHabitability() + 1, 3) - 1, 0);
        planetInv.ChangeFood(modifyValue);
    }
}

public class Reactor : Building
{
    private void Awake()
    {
        IsResourceBearing = true;
        HasAction = false;
        MineralCost = 5;
        EnergyRunCost = 0;
    }

    public override void ProcessResources(InventoryController planetInv)
    {
        planetInv.ChangeEnergy(5);
    }
}

public class AsteroidMiner : Building
{
    private void Awake()
    {
        IsResourceBearing = true;
        HasAction = false;
        MineralCost = 5;
        EnergyRunCost = 1;
    }

    public override bool MeetsBuildingPrerequisite()
    {
        return planet.HasSpaceStation();
    }
    
    public override void ProcessResources(InventoryController planetInv)
    {
        planetInv.ChangeMinerals(5);
    }
}

public class SpaceStation : Building
{
    public override bool MeetsBuildingPrerequisite()
    {
        return !planet.HasSpaceStation();
    }
    
    private void Awake()
    {
        HasAction = false;
        IsResourceBearing = false;
        MineralCost = 5;
        EnergyRunCost = 1;
    }
    
    public override void FinishedConstruction()
    {
        planet.AddSpaceStation();
    }
}

public class Mine : Building
{
    private void Awake()
    {
        IsResourceBearing = true;
        HasAction = false;
        MineralCost = 5;
        EnergyRunCost = 1;
    }

    public override void ProcessResources(InventoryController planetInv)
    {
        planetInv.ChangeMinerals(5);
        planet.ChangeHabitability(-0.01f);
    }
}

public class FabricationPlant : Building
{
    private void Awake()
    {
        IsResourceBearing = true;
        HasAction = false;
        MineralCost = 5;
        EnergyRunCost = 1;
    }
    
    public override void ProcessResources(InventoryController planetInv)
    {
        if (planetInv.GetMinerals() >= 5)
        {
            planetInv.AddResources(0, 0, -5, 10);
        }
    }
}

public class Terraformer : Building
{
    private void Awake()
    {
        HasAction = true;
        IsResourceBearing = false;
        MineralCost = 5;
        EnergyRunCost = 1;
    }
    
    public override void PerformAction()
    {
        planet.ChangeHabitability(0.02f);
    }
}

public class Housing : Building
{
    private void Awake()
    {
        HasAction = false;
        IsResourceBearing = false;
        MineralCost = 5;
        EnergyRunCost = 1;
    }

    public override void FinishedConstruction()
    {
        planet.numberOfHouses += 1;
    }
}
