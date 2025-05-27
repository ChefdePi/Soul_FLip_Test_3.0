using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ResourceManager : MonoBehaviour
{

    [SerializeField]
    Tile Tree;
    [SerializeField]
    GameObject Stone;

    [SerializeField] int numberOfTiles;
    Tilemap UpTop;

    void Start()
    {
        GenerateResources(numberOfTiles);
    }

    void Update()
    {
        
    }

    void GenerateResources(int numberOfTiles)
    {
        List<Vector3Int> xytilePosition = new List<Vector3Int>();
        UpTop = GetComponent<Tilemap>();
        for (int i = UpTop.cellBounds.xMin; i < UpTop.cellBounds.xMax; i++)
        {
            for (int j = UpTop.cellBounds.yMin; j < UpTop.cellBounds.yMax; j++)
            {
                Vector3Int localTilePosition = new Vector3Int(i, j, 0);
               
                if (UpTop.HasTile(localTilePosition))
                {
                    xytilePosition.Add(localTilePosition);
                }
            }
        }
        for (int i = 0; i < numberOfTiles; i++)
        {
            Vector3Int positionToSpawn = xytilePosition[Random.Range(0, xytilePosition.Count)];
            UpTop.SetTile(positionToSpawn, Tree);
        }
    }
}

