using UnityEngine;

[CreateAssetMenu(fileName = "SO_Enemy", menuName = "Scriptable Objects/SO_Enemy")]
public class SO_Enemy : ScriptableObject
{
    [SerializeField] private float _health = 2f;
    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private float _acceleration = 15f;
    [SerializeField] private float _stoppingDistance = 0.1f;
    [SerializeField] private float _damage = 1f;

    public float Health => _health;
    public float MoveSpeed => _moveSpeed;
    public float Acceleration => _acceleration;
    public float StoppingDistance => _stoppingDistance;
    public float Damage => _damage;
}
