using UnityEngine;

public class BulletController : MonoBehaviour
{
    // needs a target
    // gets bullet characteristics from the tower
    // should the bullet track or go straight? 
    private GameObject _target = null;
    private float _damage = 0f;
    private float _speed = 0f;

    private Vector3 _direction = Vector3.zero;

    public void SetProperties(float damage, float speed, GameObject target) {
        _damage = damage;
        _speed = speed;
        _target = target;
        Vector3 direction = _target.transform.position - transform.position;
        _direction = direction.normalized;
    }

    private void Update() {
        transform.Translate(_direction * _speed * Time.deltaTime, Space.World);
    }

    private void OnBecameInvisible() {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        // Debug.Log("Bullet hit " + other.gameObject.name);
        if (other != null &&other.gameObject.tag == "Enemy") {
            HitTarget();
        }
    }

    private void HitTarget() {
        if (_target != null) {
            _target.GetComponent<EnemyController>().TakeDamage(_damage);
        }
        Destroy(gameObject);
    }
}
