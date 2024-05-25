using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;
    public class OnIngredientAddedEventArgs : EventArgs
    {
        public KitchenObjectSO ingredient;
    }
    [SerializeField] private List<KitchenObjectSO> validKitchenObjects;
    private List<KitchenObjectSO> kitchenObjectSOs;

    private void Awake()
    {
        kitchenObjectSOs = new List<KitchenObjectSO>();
    }

    public bool TryAddIngredient(KitchenObjectSO kitchenObject)
    {
        if (!validKitchenObjects.Contains(kitchenObject))
        {
            // Not a valid ingredient
            return false;
        }
        if (kitchenObjectSOs.Contains(kitchenObject))
        {
            return false;
        }
        else
        {
            kitchenObjectSOs.Add(kitchenObject);
            OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs { ingredient = kitchenObject });
            return true;
        }
    }

    public List<KitchenObjectSO> GetKitchenObjects() { return kitchenObjectSOs; }
}
