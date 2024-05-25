using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour
{
    [SerializeField] private PlatesCounter platesCounter;
    [SerializeField] private Transform counterTopPoint; 
    [SerializeField] private Transform platePrefab;

    private List<GameObject> plateVisuals;

    private void Awake()
    {
        plateVisuals = new List<GameObject>();
    }

    private void Start()
    {
        platesCounter.OnPlateSpawned += PlatesCounter_OnPlateSpawned;
        platesCounter.OnPlateTaken += PlatesCounter_OnPlateTaken;
    }

    private void PlatesCounter_OnPlateTaken(object sender, System.EventArgs e)
    {
        GameObject plateObject = plateVisuals[plateVisuals.Count - 1];
        plateVisuals.Remove(plateObject);
        Destroy(plateObject);
    }

    private void PlatesCounter_OnPlateSpawned(object sender, System.EventArgs e)
    {
        Transform plateVisual = Instantiate(platePrefab, counterTopPoint);
        float plateOffsetY = 0.1f;
        plateVisual.localPosition = new Vector3(0, plateOffsetY * plateVisuals.Count, 0);
        plateVisuals.Add(plateVisual.gameObject);
    }
}
