using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Frying Recipe", menuName ="FryingRecipe")]
public class FryingRecipeSO : ScriptableObject
{
    public KitchenObjectSO input, output;
    public float TimeToFry;
}
