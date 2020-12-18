using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesManager : MonoBehaviour
{
    public static ResourcesManager instance;

    [SerializeField] private int foodQuantity = 0;

    public int FoodQuantity
    {
        get { return foodQuantity; }
    }

    [SerializeField] private int woodQuantity = 0;

    public int WoodQuantity
    {
        get { return woodQuantity; }
    }


    private void Start()
    {
        instance = this;
    }

    #region Food
    public void AddFood(int quantity)
    {
        foodQuantity += quantity;
        if (foodQuantity > 9999)
            foodQuantity = 9999;
    }

    public void UseFood(int quantity)
    {
        foodQuantity -= quantity;
    }
    #endregion

    #region Wood
    public void AddWood(int quantity)
    {
        woodQuantity += quantity;
        if (woodQuantity > 9999)
            woodQuantity = 9999;
    }

    public void UseWood(int quantity)
    {
        woodQuantity -= quantity;
    }
    #endregion


}
