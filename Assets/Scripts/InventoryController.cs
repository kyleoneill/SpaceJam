using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    private int _food;
    private int _energy;
    private int _minerals;
    private int _consumerGoods;
    
    private void Awake()
    {
        _food = 0;
        _energy = 0;
        _minerals = 0;
        _consumerGoods = 0;
    }

    void Start()
    {
        
    }
    
    public void AddResources(int newFood, int newEnergy, int newMinerals, int newConsumerGoods)
    {
        ChangeFood(newFood);
        ChangeEnergy(newEnergy);
        ChangeMinerals(newMinerals);
        ChangeConsumerGoods(newConsumerGoods);
    }

    public void TransferResources(InventoryController otherInventory)
    {
        otherInventory.AddResources(_food, _energy, _minerals, _consumerGoods);
        _food = 0;
        _energy = 0;
        _minerals = 0;
        _consumerGoods = 0;
    }
    
    public int[] GetResources()
    {
        return new int[4] { _food, _energy, _minerals, _consumerGoods };
    }

    public void ChangeFood(int food)
    {
        _food = Math.Max(_food + food, 0);
    }
    
    public void ChangeEnergy(int energy)
    {
        _energy = Math.Max(_energy + energy, 0);
    }

    public void ChangeMinerals(int minerals)
    {
        _minerals = Math.Max(_minerals + minerals, 0);
    }
    
    public void ChangeConsumerGoods(int consumerGoods)
    {
        _consumerGoods = Math.Max(_consumerGoods + consumerGoods, 0);
    }

    public int GetMinerals()
    {
        return _minerals;
    }

    public int GetEnergy()
    {
        return _energy;
    }

    public int GetFood()
    {
        return _food;
    }

    public int GetConsumerGoods()
    {
        return _consumerGoods;
    }
}
