using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeComplete;

    public event EventHandler OnRecipeSuccess;
    public event EventHandler OnRecipeFailed;
    public static DeliveryManager Instance { get; private set; }

    [SerializeField] private RecipeListSO recipes;
    private List<RecipeSO> ordersList;
    private float spawnRecipeTimer;
    [SerializeField] private float recipeSpawnTime = 4f;
    [SerializeField] private int maxOrders = 4;
    private int successfulDeliveriesCount;

    private void Awake()
    {
        Instance = this;
        ordersList = new List<RecipeSO>();
    }
    private void Update()
    {
        spawnRecipeTimer -= Time.deltaTime;
        if (spawnRecipeTimer <= 0.0f)
        {
            spawnRecipeTimer = recipeSpawnTime;
            if (ordersList.Count < maxOrders)
            {
                RecipeSO waitingRecipe = recipes.Recipes[UnityEngine.Random.Range(0, recipes.Recipes.Count)];
                ordersList.Add(waitingRecipe);

                OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public void DeliverRecipe(PlateKitchenObject plate)
    {
        for (int i = 0; i < ordersList.Count; i++)
        {
            RecipeSO order = ordersList[i];

            if (order.kitchenObjects.Count == plate.GetKitchenObjects().Count)
            {
                // Same number of ingredients
                bool plateContentMatchesRecipe = true;
                foreach(KitchenObjectSO recipeObject in order.kitchenObjects)
                {
                    // Going through order ingredients
                    bool ingredientFound = false;
                    foreach(KitchenObjectSO plateItem in plate.GetKitchenObjects())
                    {
                        // Plate ingredients
                        if (plateItem == recipeObject)
                        {
                            // Ingredient does match
                            ingredientFound = true;
                            break;
                        }
                    }
                    if (!ingredientFound)
                    {
                        // This recipe ingredient not found on plate
                        plateContentMatchesRecipe = false;
                    }
                }

                if (plateContentMatchesRecipe)
                {
                    // Player delivered correct recipe
                    ordersList.RemoveAt(i);
                    OnRecipeComplete?.Invoke(this, EventArgs.Empty);
                    OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
                    successfulDeliveriesCount++;
                    return;
                }
            }
        }
        // No matches found. Player did not deliver valid recipe.
        OnRecipeFailed?.Invoke(this, EventArgs.Empty);
    }

    public List<RecipeSO> GetOrders()
    {
        return ordersList;
    }

    public int GetSuccessfulDeliveries() { return successfulDeliveriesCount; }
}
