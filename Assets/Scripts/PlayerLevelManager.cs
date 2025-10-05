using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLevelManager : MonoBehaviour
{
    [Header("Data")]
    [Tooltip("Reference to the Scriptable Object with the XP formula constants.")]
    [SerializeField] private SO_LevelProgression _levelData;

    [Header("Current State")]
    [SerializeField] private int _currentLevel = 1;
    [SerializeField] private float _currentXP = 0f;
    [SerializeField] private float _xpToNextLevel = 0f;

    private Slider _uiXpSlider = null;

    public event EventHandler OnPlayerLevelUp;

    private void Awake()
    {
        // Calculate the XP requirement for the very first level
        _xpToNextLevel = CalculateXPForLevel(_currentLevel + 1);
    }

    private void Start()
    {
        _uiXpSlider = GameManager.Instance.GetXpSlider();
        
        GameManager.Instance.OnXpCollect += GameManager_OnXpCollect;
    }

    public void InitSlider()
    {
        _uiXpSlider.minValue = 0f;
        _uiXpSlider.maxValue = _xpToNextLevel;
        _uiXpSlider.value = 0;
    }

    public void AddXP(float amount)
    {
        _currentXP += amount;
        _uiXpSlider.value += amount;

        // Continuously check if the player has enough XP to level up
        while (_currentXP >= _xpToNextLevel && _currentLevel < _levelData.MaxLevel)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        // 1. Subtract the XP that was used to reach this level.
        // In this system, it's often better to use 'XP for next level'
        // instead of total XP, but for simplicity, we'll keep the
        // current approach:
        _currentXP -= _xpToNextLevel;

        // 2. Increase the level
        _currentLevel++;

        // 3. Calculate the new XP requirement for the next level
        // Calls the method for the next level (_currentLevel + 1)
        _xpToNextLevel = CalculateXPForLevel(_currentLevel + 1);

        // 4. Here you can add events
        Debug.Log($"Congratulations! You reached level {_currentLevel}. Next level requires {_xpToNextLevel} XP.");
        OnPlayerLevelUp?.Invoke(this, EventArgs.Empty);
        _uiXpSlider.minValue = _currentXP;
        _uiXpSlider.maxValue = _xpToNextLevel;
        _uiXpSlider.value = _currentXP;
    }


    // METHOD FOR XP CALCULATION
    // This method can be called at any time to know the total XP for a given level.
    public float CalculateXPForLevel(int targetLevel)
    {
        // If the target level is outside the max limit
        if (targetLevel > _levelData.MaxLevel)
        {
            return float.MaxValue;
        }

        // Our formula: XP_n = Base * (n^2 + (n * C))
        float baseVal = _levelData.BaseXP;
        float C = _levelData.CurveMultiplier;
        float n = targetLevel;

        float requiredXP = baseVal * (n * n + (n * C));

        // Return the total XP required to reach this level
        return requiredXP;
    }
    
    private void GameManager_OnXpCollect(object sender, EventArgs e)
    {
        AddXP(1);
    }
}
