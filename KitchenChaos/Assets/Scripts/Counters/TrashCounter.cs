using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCounter : BaseCounter
{
    public static event EventHandler OnTrashObject;
    new public static void ResetStaticData()
    {
        OnTrashObject = null;
    }
    public override void Interact(Player player)
    {
        if (player.HasKitchenObject())
        {
            player.GetKitchenObject().DestroySelf();
            OnTrashObject?.Invoke(this, EventArgs.Empty);
        }
    }
}
