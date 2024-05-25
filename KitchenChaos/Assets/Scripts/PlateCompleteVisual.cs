using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCompleteVisual : MonoBehaviour
{
    [Serializable]
    public struct KitchenObjectSO_GameObject
    {
        public KitchenObjectSO kitchenObject;
        public GameObject gameObject;
    }

    [SerializeField] private PlateKitchenObject plate;
    [SerializeField] private List<KitchenObjectSO_GameObject> kitchenObjectSO_GameObjects;

    private void Start()
    {
        plate.OnIngredientAdded += Plate_OnIngredientAdded;
        foreach (KitchenObjectSO_GameObject gameObject in kitchenObjectSO_GameObjects)
            gameObject.gameObject.SetActive(false);
    }

    private void Plate_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e)
    {
        foreach(KitchenObjectSO_GameObject gameObject in kitchenObjectSO_GameObjects)
        {
            if (gameObject.kitchenObject == e.ingredient)
            {
                gameObject.gameObject.SetActive(true);
            }
        }
    }
}
