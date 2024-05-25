using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Cutting Recipe", menuName ="CuttingRecipe")]
public class CuttingRecipeSO : ScriptableObject
{
    public KitchenObjectSO input, output;
    public int numberOfCuts;
}
