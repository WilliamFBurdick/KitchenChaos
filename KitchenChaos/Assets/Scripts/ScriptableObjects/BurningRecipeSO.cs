using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Burning Recipe", menuName ="BurningRecipe")]
public class BurningRecipeSO : ScriptableObject
{
    public KitchenObjectSO input, output;
    public float TimeToBurn;
}
