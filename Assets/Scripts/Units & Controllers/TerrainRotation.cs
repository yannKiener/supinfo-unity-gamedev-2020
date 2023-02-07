using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainRotation : MonoBehaviour
{


    public List<GameObject> terrains;
    public float terrainSize;
    public GameObject mainCamera;

    private int counter = 0;
    private int terrainCount;
    private GameObject currentTerrain;

    // Start is called before the first frame update
    void Start()
    {
        terrainCount = terrains.Count;
        UpdateToNextTerrain();
    }

    // Update is called once per frame
    void Update()
    {
        if(mainCamera.transform.position.z >= currentTerrain.transform.position.z + terrainSize)
        {
            currentTerrain.transform.position = new Vector3(currentTerrain.transform.position.x, currentTerrain. transform.position.y, currentTerrain.transform.position.z + terrainCount * terrainSize);
            UpdateToNextTerrain();
        }
        
    }

    private void UpdateToNextTerrain()
    {
        currentTerrain = terrains[counter];
        counter++;
        if(counter >= terrainCount)
        {
            counter = 0;
        }
    }
}
