using UnityEngine;
using System.Collections.Generic;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private SO_Enemy _enemyStats;

    private Rigidbody2D _rb;
    private Transform _target;
    private List<Vector3> _path;
    private int _currentWaypointIndex;
    private float _pathRequestCooldown = 0.5f;
    private float _lastPathRequestTime;
    private float _health = 0f;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _health = _enemyStats.Health;
    }

    private void Start() {
        _target = GameManager.Instance.GetPlayer().transform;
        _lastPathRequestTime = -_pathRequestCooldown; // Allow immediate path request
    }

    private void FixedUpdate() {
        if (_target == null) return;

        if (Time.time > _lastPathRequestTime + _pathRequestCooldown) {
            RequestPath();
        }

        FollowTarget();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Using a trigger, the enemy can pass through the player but still detect the overlap.
        if (other.gameObject.TryGetComponent(out Player player))
        {
            // Damage the player upon entering their trigger area.
            other.gameObject.GetComponent<PlayerController>().TakeDamage(_enemyStats.Damage);
        }
    }

    private void RequestPath()
    {
        _lastPathRequestTime = Time.time;

        List<Vector3> newPath = Pathfinding.Instance.FindPath(transform.position, _target.position);
        if (newPath != null && newPath.Count > 0)
        {
            _path = newPath;
            _currentWaypointIndex = 0;
        }
    }

    private void FollowTarget() {
        // If we are within stopping distance of the final target, stop moving.
        if (Vector2.Distance(transform.position, _target.position) <= _enemyStats.StoppingDistance)
        {
            _rb.linearVelocity = Vector2.Lerp(_rb.linearVelocity, Vector2.zero, _enemyStats.Acceleration * Time.fixedDeltaTime);
            return;
        }

        Vector2 targetVelocity = Vector2.zero;

        if (_path != null && _path.Count > 0) {
            Vector3 currentWaypoint = _path[_currentWaypointIndex];
            // Use a small, fixed threshold for switching waypoints.
            if (Vector2.Distance(transform.position, currentWaypoint) < 0.5f) {
                _currentWaypointIndex++;
                if (_currentWaypointIndex >= _path.Count) {
                    _path = null; // Reached end of path
                    _rb.linearVelocity = Vector2.zero; // Stop precisely at the end of the path
                    return;
                }
                currentWaypoint = _path[_currentWaypointIndex];
            }

            if (_currentWaypointIndex < _path.Count) {
                Vector2 moveDirection = (currentWaypoint - transform.position).normalized;
                targetVelocity = moveDirection * _enemyStats.MoveSpeed;
            }
        }
        
        // Set velocity for physics-based movement to ensure collision detection.
        _rb.linearVelocity = Vector2.Lerp(_rb.linearVelocity, targetVelocity, _enemyStats.Acceleration * Time.fixedDeltaTime);
    }

    public void TakeDamage(float damage) {
        _health -= damage;
        if (_health <= 0) {
            // Remove from GameManager's list before destroying
            GameManager.Instance.RemoveEnemy(gameObject);
            Destroy(gameObject);
        }
    }
}
