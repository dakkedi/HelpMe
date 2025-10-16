using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] private GridStats _gridStats;
    private Node[,] _grid;
    // private List<Node> _spawnableNodes = new List<Node>();

    public float MinX { get; private set; }
    public float MaxX { get; private set; }
    public float MinY { get; private set; }
    public float MaxY { get; private set; }

    private void CreateGrid()
    {
        _grid = new Node[_gridStats.GridWidth, _gridStats.GridHeight];
        float offsetX = _gridStats.GridWidth * _gridStats.CellSize;
        offsetX = offsetX * 0.5f;
        float offsetY = _gridStats.GridHeight * _gridStats.CellSize;
        offsetY = offsetY * 0.5f;

        MinX = -offsetX;
        MaxX = offsetX;
        MinY = -offsetY;
        MaxY = offsetY;

        for (int x = 0; x < _gridStats.GridWidth; x++)
        {
            for (int y = 0; y < _gridStats.GridHeight; y++)
            {
                float worldX = x * _gridStats.CellSize - offsetX + (_gridStats.CellSize / 2f);
                float worldY = y * _gridStats.CellSize - offsetY + (_gridStats.CellSize / 2f);
                Vector3 worldPoint = new Vector3(worldX, worldY, 0);
                _grid[x, y] = new Node(true, worldPoint, x, y);
                // if (x < 2 || x >= _gridStats.GridWidth - 2 || y < 2 || y >= _gridStats.GridHeight - 2)
                // {
                //     _spawnableNodes.Add(_grid[x, y]);
                // }
                // if (_tilePrefab != null)
                // {
                //     // Place tiles slightly behind other objects to avoid z-fighting
                //     Vector3 tilePosition = new Vector3(worldPoint.x, worldPoint.y, 1f);
                //     Instantiate(_tilePrefab, tilePosition, Quaternion.identity, transform);
                // }
            }
        }
    }
}
