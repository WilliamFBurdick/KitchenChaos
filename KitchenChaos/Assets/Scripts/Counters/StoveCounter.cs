using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CuttingCounter;

public class StoveCounter : BaseCounter, IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }

    public enum State
    {
        Idle, Frying, Fried, Burnt
    }
    [SerializeField] private FryingRecipeSO[] fryingRecipes;
    [SerializeField] private BurningRecipeSO[] burningRecipes;

    private State currentState;
    private float fryingTimer, burningTimer;
    private FryingRecipeSO fryingRecipe;
    private BurningRecipeSO burningRecipe;

    private void Start()
    {
        currentState = State.Idle;
    }

    private void Update()
    {
        if (HasKitchenObject())
        {
            switch(currentState)
            {
                case State.Idle:
                    break;
                case State.Frying:
                    fryingTimer += Time.deltaTime;
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized = fryingTimer / fryingRecipe.TimeToFry });
                    if (fryingTimer > fryingRecipe.TimeToFry)
                    {
                        // Finished frying
                        fryingTimer = 0f;
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(fryingRecipe.output, this);
                        currentState = State.Fried;
                        burningTimer = 0f;
                        burningRecipe = GetBurningRecipe(GetKitchenObject().GetKitchenObjectSO());

                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = currentState });

                    }
                    break;
                case State.Fried:
                    burningTimer += Time.deltaTime;
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized = burningTimer / burningRecipe.TimeToBurn });
                    if (burningTimer > burningRecipe.TimeToBurn)
                    {
                        // Finished burning
                        burningTimer = 0f;
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(burningRecipe.output, this);
                        currentState = State.Burnt;
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = currentState });
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized = 0f });
                    }
                    break;
                case State.Burnt:
                    break;
                default:
                    break;
            }
        }
    }

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            // No kitchen object
            if (player.HasKitchenObject())
            {
                // Player is holding object
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    // Player holding object that can be fried
                    player.GetKitchenObject().SetKitchenObjectParent(this);

                    fryingRecipe = GetFryingRecipe(GetKitchenObject().GetKitchenObjectSO());

                    currentState = State.Frying;
                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = currentState });
                    fryingTimer = 0f;
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized = fryingTimer / fryingRecipe.TimeToFry });
                }
            }
            else
            {
                // Player not carrying anything
            }
        }
        else
        {
            // Has kitchen object
            if (player.HasKitchenObject())
            {
                // Player has kitchen object
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plate))
                {
                    if (plate.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();

                        currentState = State.Idle;
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = currentState });
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized = 0f });
                    }
                }
            }
            else
            {
                // Player does not have kitchen object
                GetKitchenObject().SetKitchenObjectParent(player);
                currentState = State.Idle;
                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = currentState });
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized = 0f });
            }
        }
    }

    private bool HasRecipeWithInput(KitchenObjectSO input)
    {
        FryingRecipeSO recipe = GetFryingRecipe(input);
        return recipe != null;
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO input)
    {
        FryingRecipeSO recipe = GetFryingRecipe(input);
        if (recipe != null)
            return recipe.output;
        else
            return null;
    }

    private FryingRecipeSO GetFryingRecipe(KitchenObjectSO input)
    {
        foreach (FryingRecipeSO recipe in fryingRecipes)
            if (recipe.input == input)
                return recipe;
        return null;
    }

    private BurningRecipeSO GetBurningRecipe(KitchenObjectSO input)
    {
        foreach (BurningRecipeSO recipe in burningRecipes)
            if (recipe.input == input)
                return recipe;
        return null;
    }
}
