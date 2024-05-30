using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateTaken;

    [SerializeField] private KitchenObjectSO plateKitchenObject;

    [SerializeField] private float plateSpawnTime = 4f;
    [SerializeField] private int maxPlates = 4;
    private float spawnPlateTimer;
    private int platesCount;

    private void Update()
    {
        spawnPlateTimer += Time.deltaTime;
        if (GameManager.Instance.IsGamePlaying() && spawnPlateTimer > plateSpawnTime && platesCount < maxPlates)
        {
            platesCount++;

            OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            spawnPlateTimer = 0f;
        }
    }

    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {
            // Player is empty handed
            if (platesCount > 0)
            {
                // There's at least 1 plate
                platesCount--;
                KitchenObject.SpawnKitchenObject(plateKitchenObject, player);
                OnPlateTaken?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
