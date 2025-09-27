using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int _gridWidth = 30;
    [SerializeField] private int _gridHeight = 15;
    [SerializeField] private int _cellSize = 1;
    [SerializeField] private GameObject _towerPrefab;
    [SerializeField] private GameObject _highlightPrefab;
    [SerializeField] private GameObject _tilePrefab;

    public static GridManager Instance { get; private set; }
    public float MinX { get; private set; }
    public float MaxX { get; private set; }
    public float MinY { get; private set; }
    public float MaxY { get; private set; }

    private MouseInputManager _mouseInputManager;
    private GameObject _currentHighlight;
    private Node[,] _grid;
    private List<Node> _spawnableNodes = new List<Node>();

    private void Awake()
    {
        if (Instance != null && Instance != this) {
            Destroy(this.gameObject);
        } else {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        CreateGrid();

        _currentHighlight = Instantiate(_highlightPrefab);
        _currentHighlight.SetActive(false);
    }

    private void Start() {
        _mouseInputManager = PlayerController.Instance.GetComponent<MouseInputManager>();
    }

    private void CreateGrid()
    {
        _grid = new Node[_gridWidth, _gridHeight];
        float offsetX = (_gridWidth * _cellSize) / 2f;
        float offsetY = (_gridHeight * _cellSize) / 2f;

        MinX = -offsetX;
        MaxX = offsetX;
        MinY = -offsetY;
        MaxY = offsetY;

        for (int x = 0; x < _gridWidth; x++)
        {
            for (int y = 0; y < _gridHeight; y++)
            {
                float worldX = x * _cellSize - offsetX + (_cellSize / 2f);
                float worldY = y * _cellSize - offsetY + (_cellSize / 2f);
                Vector3 worldPoint = new Vector3(worldX, worldY, 0);
                _grid[x, y] = new Node(true, worldPoint, x, y);
                if (x < 2 || x >= _gridWidth - 2 || y < 2 || y >= _gridHeight - 2)
                {
                    _spawnableNodes.Add(_grid[x, y]);
                }
                if (_tilePrefab != null)
                {
                    // Place tiles slightly behind other objects to avoid z-fighting
                    Vector3 tilePosition = new Vector3(worldPoint.x, worldPoint.y, 1f);
                    Instantiate(_tilePrefab, tilePosition, Quaternion.identity, transform);
                }
            }
        }
    }

    private void Update() {
        Vector3 mousePosition = _mouseInputManager.mousePosition;
        Node node = NodeFromWorldPoint(mousePosition);

        if (node != null)
        {
            _currentHighlight.SetActive(true);
            _currentHighlight.transform.position = node.worldPosition;

            if (!node.isWalkable)
            {
                _currentHighlight.GetComponent<Renderer>().material.color = Color.red;
            }
            else
            {
                _currentHighlight.GetComponent<Renderer>().material.color = Color.green;
            }

            if (_mouseInputManager.leftClickDown)
            {
                if (node.isWalkable)
                {
                    PlaceTower(node.gridX, node.gridY);
                }
            }
        }
        else
        {
            _currentHighlight.SetActive(false);
        }        
    }

    void PlaceTower(int x, int y)
    {
        TowerController prefabScript = _towerPrefab.GetComponent<TowerController>();
        if (prefabScript.GetCost() <= GameManager.Instance.GetCurrentCoin())
        {
            Node node = _grid[x, y];
            GameObject tower = Instantiate(_towerPrefab, node.worldPosition, Quaternion.identity);
            if (tower)
            {
                GameManager.Instance.UseCoin(prefabScript.GetCost());
                node.isWalkable = false;
            }
        }
    }

    public bool TryGetRandomSpawnPoint(out Vector3 spawnPoint)
    {
        spawnPoint = Vector3.zero;
        if (_spawnableNodes.Count == 0)
            return false;

        int startIndex = Random.Range(0, _spawnableNodes.Count);
        for (int i = 0; i < _spawnableNodes.Count; i++)
        {
            int currentIndex = (startIndex + i) % _spawnableNodes.Count;
            Node node = _spawnableNodes[currentIndex];
            if (node.isWalkable)
            {
                spawnPoint = node.worldPosition;
                return true;
            }
        }

        return false; // No walkable spawn points found
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float offsetX = (_gridWidth * _cellSize) / 2f;
        float offsetY = (_gridHeight * _cellSize) / 2f;
        Vector3 adjustedPos = new Vector3(worldPosition.x + offsetX, worldPosition.y + offsetY, 0);

        int gridX = Mathf.FloorToInt(adjustedPos.x / _cellSize);
        int gridY = Mathf.FloorToInt(adjustedPos.y / _cellSize);

        if (gridX >= 0 && gridX < _gridWidth && gridY >= 0 && gridY < _gridHeight)
        {
            return _grid[gridX, gridY];
        }
        return null;
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < _gridWidth && checkY >= 0 && checkY < _gridHeight)
                {
                    // If it's a diagonal neighbor, check if we can cut the corner
                    if (x != 0 && y != 0)
                    {
                        // Check if the two adjacent cardinal neighbors are walkable. If not, we can't move diagonally.
                        if (!_grid[node.gridX, checkY].isWalkable || !_grid[checkX, node.gridY].isWalkable)
                            continue;
                    }

                    neighbours.Add(_grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }

    private void OnDrawGizmos() {
        // Optional: Add gizmos to visualize the grid for debugging
    }
}
