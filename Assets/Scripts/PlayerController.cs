using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float deceleration = 10f;
    [SerializeField] private float _health = 10f;
    [SerializeField] private GameObject _playerSprite;

    [SerializeField] private float _gravitationalPullRadius = 5f;
    public float GetPullRadius() { return _gravitationalPullRadius; }
    [SerializeField] private float _xpGained = 0f;
    [SerializeField] private int[] _levelThresholds;
    [SerializeField] private int _currentLevel = 1;
    [SerializeField] private Slider _uiXpSlider;


    private float _currentHealth = 0;

    public static PlayerController Instance { get; private set; }
    public event EventHandler OnPlayerLevelUp;
    private Vector2 _moveInput;
    private Vector2 _currentVelocity;
    private Rigidbody2D _rb;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        _currentHealth = _health;
        _rb = GetComponent<Rigidbody2D>();

        _uiXpSlider.minValue = 0f;
        _uiXpSlider.maxValue = _levelThresholds[0];
        _uiXpSlider.value = 0;

    }

    private void GameManager_OnCoinCollect(object sender, EventArgs e)
    {
        _xpGained += 1;
        _uiXpSlider.value++;
        CheckLevelUp();
    }

    private void CheckLevelUp()
    {
        // Since towers spawn at level 1 atm, the level up stats dont apply to new towers when player is max level. 
        // if tower placed at player level 4, it only gets one level up since 5 is current max.
        int currentLevelIndex = _currentLevel - 1;
        for (int i = currentLevelIndex; i < _levelThresholds.Length; i++)
        {
            if (_xpGained >= _levelThresholds[i])
            {
                LevelUp();
                return;
            }
        }
    }

    private void LevelUp()
    {
        _uiXpSlider.minValue = _levelThresholds[_currentLevel-1];
        _uiXpSlider.maxValue = _levelThresholds[_currentLevel];
        _uiXpSlider.value = _levelThresholds[_currentLevel-1];
        _currentLevel++;
        Debug.Log("Player leveled up!");
        OnPlayerLevelUp?.Invoke(this, EventArgs.Empty);
    }

    private void Start()
    {
        GameManager.Instance.OnCoinCollect += GameManager_OnCoinCollect;
        UpdatePlayerHpUi();
    }

    private void FixedUpdate()
    {
        Vector2 direction = _rb.linearVelocity.normalized;
        if (direction.x < 0 && _playerSprite.transform.localScale.x > 0)
        {
            _playerSprite.transform.localScale = new Vector2(Mathf.Abs(_playerSprite.transform.localScale.x) * -1, _playerSprite.transform.localScale.y);
        } else if (direction.x > 0 && _playerSprite.transform.localScale.x < 0) {
            _playerSprite.transform.localScale = new Vector2(Mathf.Abs(_playerSprite.transform.localScale.x), _playerSprite.transform.localScale.y);
        }

        Vector2 targetVelocity = _moveInput * moveSpeed;
        float accelRate = _moveInput.magnitude > 0.1f ? acceleration : deceleration;

        _currentVelocity = Vector2.Lerp(_currentVelocity, targetVelocity, accelRate * Time.fixedDeltaTime);
        _rb.linearVelocity = _currentVelocity;

        // Clamp player position to grid boundaries
        var gridManager = GridManager.Instance;
        Vector2 clampedPosition = _rb.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, gridManager.MinX, gridManager.MaxX);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, gridManager.MinY, gridManager.MaxY);
        _rb.position = clampedPosition;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        UpdatePlayerHpUi();
        if (_currentHealth <= 0)
        {
            // for now, reload the scene on death
            Debug.Log("player did, insert logic for ending game.");
        }
    }

    private void UpdatePlayerHpUi()
    {
        GameManager.Instance.UpdateHpUi(Mathf.CeilToInt(_currentHealth)); 
    }
}
