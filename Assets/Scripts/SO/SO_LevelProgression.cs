using UnityEngine;

[CreateAssetMenu(fileName = "SO_LevelProgression", menuName = "Scriptable Objects/SO_LevelProgression")]
public class SO_LevelProgression : ScriptableObject
{
    [Header("XP Curve Constants")]
    [Tooltip("Default value in the formula  (Base in our formula).")]
    [SerializeField] private float _baseXP = 50f;

    [Tooltip("Multiplier (C in our formula) that will adjust how fast the curve gets steep.")]
    [SerializeField] private float _curveMultiplier = 50f;

    [Tooltip("Max level.")]
    [SerializeField] private int _maxLevel = 100;

    public float BaseXP => _baseXP;
    public float CurveMultiplier => _curveMultiplier;
    public int MaxLevel => _maxLevel;
}
