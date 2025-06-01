using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ResourceManager : MonoBehaviour
{
    [Header("Tilemap References (Ground & Resources)")]

    [Tooltip("The Tilemap you painted your UpTop ground onto.")]
    [SerializeField] private Tilemap upTopTilemap;

    [Tooltip("The Tilemap you use for trees and rocks (UpTop resources).")]
    [SerializeField] private Tilemap resourceUpTopTilemap;

    [Tooltip("The Tilemap you painted your DownBelow ground onto.")]
    [SerializeField] private Tilemap downBelowTilemap;

    [Tooltip("The Tilemap you use for crystals (DownBelow resources).")]
    [SerializeField] private Tilemap resourceDownBelowTilemap;

    [Header("Altar References (avoid spawning near them)")]

    [Tooltip("Assign your UpTop altar GameObject here so trees/rocks won't spawn nearby.")]
    [SerializeField] private GameObject upTopAltarObject;

    [Tooltip("Assign your Bottom altar GameObject here so crystals won't spawn nearby.")]
    [SerializeField] private GameObject bottomAltarObject;

    [Header("UpTop Resources (Light Phase)")]

    [Tooltip("Which tree tile to paint when spawning.")]
    [SerializeField] private Tile treeTile;
    [Tooltip("How many trees to spawn.")]
    [SerializeField] private int numberOfTrees = 5;

    [Tooltip("Which rock tile to paint when spawning.")]
    [SerializeField] private Tile rockTile;
    [Tooltip("How many rocks to spawn.")]
    [SerializeField] private int numberOfRocks = 5;

    [Header("DownBelow Resources (Dark Phase)")]

    [Tooltip("Large stalactite (Crystal_bottom_large).")]
    [SerializeField] private Tile crystalLargeTile;

    [Tooltip("Medium stalactite (Crystal_bottom_medium).")]
    [SerializeField] private Tile crystalMediumTile;

    [Tooltip("Small stalactite (Crystal_bottom_small).")]
    [SerializeField] private Tile crystalSmallTile;

    [Tooltip("How many large crystals to spawn.")]
    [SerializeField] private int numberOfLargeCrystals = 1;

    [Tooltip("How many medium crystals to spawn.")]
    [SerializeField] private int numberOfMediumCrystals = 1;

    [Tooltip("How many small crystals to spawn.")]
    [SerializeField] private int numberOfSmallCrystals = 5;

    private void Start()
    {
        GenerateResources();
    }

    private void GenerateResources()
    {
        // STEP 1: Build a "forbidden" set around the UpTop altar (9 cells total)
        var forbiddenUpTop = new HashSet<Vector3Int>();
        if (upTopAltarObject != null && upTopTilemap != null)
        {
            Vector3Int altarCell = upTopTilemap.WorldToCell(upTopAltarObject.transform.position);
            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    forbiddenUpTop.Add(new Vector3Int(altarCell.x + dx, altarCell.y + dy, 0));
                }
            }
        }
        else
        {
            Debug.LogError("[ResourceManager] Assign UpTopAltarObject and UpTopTilemap in the Inspector.");
        }

        // STEP 2: Gather valid UpTop ground cells (exclude forbiddenUpTop)
        var upTopCells = new List<Vector3Int>();
        if (upTopTilemap != null)
        {
            var bounds = upTopTilemap.cellBounds;
            for (int x = bounds.xMin; x < bounds.xMax; x++)
            {
                for (int y = bounds.yMin; y < bounds.yMax; y++)
                {
                    var cell = new Vector3Int(x, y, 0);
                    if (!forbiddenUpTop.Contains(cell) && upTopTilemap.HasTile(cell))
                    {
                        upTopCells.Add(cell);
                    }
                }
            }
        }

        // STEP 3: Spawn trees on UpTop, painted to resourceUpTopTilemap
        int treesToSpawn = Mathf.Min(numberOfTrees, upTopCells.Count);
        for (int i = 0; i < treesToSpawn; i++)
        {
            int idx = Random.Range(0, upTopCells.Count);
            var chosen = upTopCells[idx];
            resourceUpTopTilemap.SetTile(chosen, treeTile);
            upTopCells.RemoveAt(idx);
        }

        // STEP 4: Spawn rocks on UpTop, painted to resourceUpTopTilemap
        int rocksToSpawn = Mathf.Min(numberOfRocks, upTopCells.Count);
        for (int i = 0; i < rocksToSpawn; i++)
        {
            int idx = Random.Range(0, upTopCells.Count);
            var chosen = upTopCells[idx];
            resourceUpTopTilemap.SetTile(chosen, rockTile);
            upTopCells.RemoveAt(idx);
        }

        // STEP 5: Build a "forbidden" set around the DownBelow altar (9 cells total)
        var forbiddenDownBelow = new HashSet<Vector3Int>();
        if (bottomAltarObject != null && downBelowTilemap != null)
        {
            Vector3Int bottomCell = downBelowTilemap.WorldToCell(bottomAltarObject.transform.position);
            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    forbiddenDownBelow.Add(new Vector3Int(bottomCell.x + dx, bottomCell.y + dy, 0));
                }
            }
        }
        else
        {
            Debug.LogError("[ResourceManager] Assign bottomAltarObject and DownBelowTilemap in the Inspector.");
        }

        // STEP 6: Compress DownBelow bounds so we only loop over painted tiles
        downBelowTilemap.CompressBounds();
        var downBounds = downBelowTilemap.cellBounds;

        // STEP 7: Gather valid DownBelow cells (exclude forbiddenDownBelow)
        var downCells = new List<Vector3Int>();
        foreach (Vector3Int cellPos in downBounds.allPositionsWithin)
        {
            if (downBelowTilemap.HasTile(cellPos) && !forbiddenDownBelow.Contains(cellPos))
            {
                downCells.Add(cellPos);
            }
        }

        // STEP 8: Spawn large crystals on DownBelow, painted to resourceDownBelowTilemap
        int largeSpawned = Mathf.Min(numberOfLargeCrystals, downCells.Count);
        for (int i = 0; i < largeSpawned; i++)
        {
            int idx = Random.Range(0, downCells.Count);
            var chosen = downCells[idx];
            resourceDownBelowTilemap.SetTile(chosen, crystalLargeTile);
            downCells.RemoveAt(idx);
        }

        // STEP 9: Spawn medium crystals on DownBelow, painted to resourceDownBelowTilemap
        int mediumSpawned = Mathf.Min(numberOfMediumCrystals, downCells.Count);
        for (int i = 0; i < mediumSpawned; i++)
        {
            int idx = Random.Range(0, downCells.Count);
            var chosen = downCells[idx];
            resourceDownBelowTilemap.SetTile(chosen, crystalMediumTile);
            downCells.RemoveAt(idx);
        }

        // STEP 10: Spawn small crystals on DownBelow, painted to resourceDownBelowTilemap
        int smallSpawned = Mathf.Min(numberOfSmallCrystals, downCells.Count);
        for (int i = 0; i < smallSpawned; i++)
        {
            int idx = Random.Range(0, downCells.Count);
            var chosen = downCells[idx];
            resourceDownBelowTilemap.SetTile(chosen, crystalSmallTile);
            downCells.RemoveAt(idx);
        }

        // Everything spawns on the correct resource tilemap layer:
        // - Trees & rocks on resourceUpTopTilemap above UpTop
        // - Crystals on resourceDownBelowTilemap above DownBelow
    }
}
