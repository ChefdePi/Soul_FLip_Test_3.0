using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ResourceManager : MonoBehaviour
{
    [Header("Tilemap References")]
    [Tooltip("The Tilemap you painted your ground onto (UpTop).")]
    [SerializeField] private Tilemap groundTilemap;

    [Tooltip("The (initially empty) ResourceTilemap where trees will appear.")]
    [SerializeField] private Tilemap resourceTilemap;

    [Header("Resource Settings")]
    [SerializeField] private Tile treeTile;    // assign Tree_Top_Static_01 here
    [SerializeField] private int numberOfTiles; // how many trees to spawn

    private void Start()
    {
        GenerateResources();
    }

    private void GenerateResources()
    {
        // 1) Collect all positions where groundTilemap.HasTile(cell) == true
        List<Vector3Int> validCells = new List<Vector3Int>();
        for (int x = groundTilemap.cellBounds.xMin; x < groundTilemap.cellBounds.xMax; x++)
        {
            for (int y = groundTilemap.cellBounds.yMin; y < groundTilemap.cellBounds.yMax; y++)
            {
                Vector3Int cellPos = new Vector3Int(x, y, 0);
                if (groundTilemap.HasTile(cellPos))
                {
                    validCells.Add(cellPos);
                }
            }
        }

        // 2) Randomly pick 'numberOfTiles' of those and paint a tree onto resourceTilemap
        for (int i = 0; i < numberOfTiles && validCells.Count > 0; i++)
        {
            int rand = Random.Range(0, validCells.Count);
            Vector3Int chosenCell = validCells[rand];

            // Paint the tree onto the empty ResourceTilemap
            resourceTilemap.SetTile(chosenCell, treeTile);

            // Remove so we don't place multiple trees at the same spot
            validCells.RemoveAt(rand);
        }
    }
}