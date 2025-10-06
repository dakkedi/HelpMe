using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLevelManager : MonoBehaviour
{
    [SerializeField] private int _currentLevel = 1;
    [SerializeField] private float _currentXP = 0f;
    [SerializeField] private int _xpToNextLevel;
    private Slider _uiXpSlider = null;

    public event EventHandler OnPlayerLevelUp;

    private void Awake()
    {
        _xpToNextLevel = CalculateXPForLevel(_currentLevel + 1);
    }

    private void Start()
    {
        _uiXpSlider = Hud.Instance.UiXpSlider;
        InitSlider();
        
        GameManager.Instance.OnXpCollect += GameManager_OnXpCollect;
    }

    private void GameManager_OnXpCollect(object sender, EventArgs e)
    {
        AddXP(1);
    }

    private void InitSlider()
    {
        _uiXpSlider.minValue = 0f;
        _uiXpSlider.maxValue = _xpToNextLevel;
        _uiXpSlider.value = 0;
    }

    private void AddXP(float amount)
    {
        _currentXP += amount;
        _uiXpSlider.value += amount;

        // Continuously check if the player has enough XP to level up
        while (_currentXP >= _xpToNextLevel)
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
    private int CalculateXPForLevel(int targetLevel)
    {
        int requiredXP = targetLevel * 2;
        return requiredXP;
    }
}
