using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float acceleration = 5f;
    [SerializeField] private float stoppingDistance = 1f;
    [SerializeField] private float _health = 5f;

    private Vector2 _currentVelocity;
    private Rigidbody2D _rb;
    private Transform _target;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start() {
        SetTarget();
    }

    private void FixedUpdate() {
        if (_target == null) return;
        FollowTarget();
    }

    private void FollowTarget() {
        Vector2 directionToTarget = (_target.position - transform.position);
        float distanceToTarget = directionToTarget.magnitude;

        Vector2 moveDirection = directionToTarget.normalized;
        Vector2 targetVelocity = Vector2.zero;

        if (distanceToTarget > stoppingDistance)
        {
            targetVelocity = moveDirection * moveSpeed;
        }

        _currentVelocity = Vector2.Lerp(_currentVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
        _rb.linearVelocity = _currentVelocity;
    }

    private void SetTarget() {
        _target = PlayerController.Instance.transform;
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
