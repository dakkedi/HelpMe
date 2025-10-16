using UnityEngine;

[CreateAssetMenu(fileName = "SO_Player", menuName = "Scriptable Objects/SO_Player")]
public class SO_Player_old : ScriptableObject
{
    [SerializeField] private float _health = 2f;
    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private float _acceleration = 15f;
    [SerializeField] private float _decceleration = 15f;
    [SerializeField] private float _gravitationalPullRadius = 15f;

    public float Health => _health;
    public float MoveSpeed => _moveSpeed;
    public float Acceleration => _acceleration;
    public float Deceleration => _decceleration;
    public float GravitationalPullRadius => _gravitationalPullRadius;
}
