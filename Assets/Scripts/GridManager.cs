using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int _gridWidth = 30;
    [SerializeField] private int _gridHeight = 15;
    [SerializeField] private int _cellSize = 1;
    [SerializeField] private GameObject _towerPrefab;
    [SerializeField] private GameObject _highlightPrefab;

    private MouseInputManager _mouseInputManager;
    private GameObject _currentHighlight;
    private bool[,] _grid;

    private void Start() {
        _grid = new bool[_gridWidth, _gridHeight];

        _currentHighlight = Instantiate(_highlightPrefab);
        _currentHighlight.SetActive(false);

        _mouseInputManager = PlayerController.Instance.GetComponent<MouseInputManager>();
    }

    private void Update() {
        Vector3 mousePosition = _mouseInputManager.mousePosition;

        float offsetX = (_gridWidth * _cellSize) / 2f;
        float offsetY = (_gridHeight * _cellSize) / 2f;
        Vector3 adjustedMousePos = new Vector3(mousePosition.x + offsetX, mousePosition.y + offsetY, 0);

        int gridX = Mathf.FloorToInt(adjustedMousePos.x / _cellSize);
        int gridY = Mathf.FloorToInt(adjustedMousePos.y / _cellSize);

        if (gridX >= 0 && gridX < _gridWidth && gridY >= 0 && gridY < _gridHeight)
        {
            _currentHighlight.SetActive(true);
            
            float highlightX = gridX * _cellSize - offsetX + (_cellSize / 2f);
            float highlightY = gridY * _cellSize - offsetY + (_cellSize / 2f);
            _currentHighlight.transform.position = new Vector3(highlightX, highlightY, 0);

            if (_grid[gridX, gridY])
            {
                _currentHighlight.GetComponent<Renderer>().material.color = Color.red;
            }
            else
            {
                _currentHighlight.GetComponent<Renderer>().material.color = Color.green;
            }

            if (_mouseInputManager.leftClickDown)
            {
                if (!_grid[gridX, gridY])
                {
                    PlaceTower(gridX, gridY);
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
        // Samma beräkning som för highlighten
        float offsetX = (_gridWidth * _cellSize) / 2f;
        float offsetY = (_gridHeight * _cellSize) / 2f;
        float towerX = x * _cellSize - offsetX + (_cellSize / 2f);
        float towerY = y * _cellSize - offsetY + (_cellSize / 2f);

        Vector3 towerPos = new Vector3(towerX, towerY, 0);
        Instantiate(_towerPrefab, towerPos, Quaternion.identity);

        _grid[x, y] = true;
    }
}
