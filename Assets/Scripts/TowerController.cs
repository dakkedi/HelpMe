using UnityEngine;
using System.Collections.Generic;
using System;

public class TowerController : MonoBehaviour
{
    [SerializeField] private float _attackCooldown = 1f;
    [SerializeField] private float _attackDamage = 1f;
    [SerializeField] private float _attackRange = 10f;
    [SerializeField] private float _projectileSpeed = 6f;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private int _cost = 5;
    public int GetCost() {
        return _cost;
    }

    private GameObject _target = null;
    private float _lastAttackTime = 0f;

    private void Awake()
    {
        PlayerController.Instance.OnPlayerLevelUp += PlayerController_OnPlayerLevelUp;
    }

    private void PlayerController_OnPlayerLevelUp(object sender, EventArgs e)
    {
        // THese only affect current towers, not the same level applies to new towers. 
        _attackDamage++;
        _attackCooldown *= 0.75f;
        _attackRange *= 1.5f;
        _projectileSpeed *= 1.1f;
    }

    private void OnDrawGizmos()
    {
        // Draw a circle around the tower with the attack range as radius
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
        if (_target != null)
        {
            Gizmos.DrawLine(transform.position, _target.transform.position);
        }
    }

    private void Update() {
        if (GameManager.Instance == null) return;

        List<GameObject> enemies = GameManager.Instance.GetCurrentEnemies();

        // find the closest enemy and with the attack range
        if (_target == null || !IsCurrentTargetStillInRange(_target)) {
            _target = FindClosestTarget(enemies);
            // _target = FindFurthestTarget(enemies);
        }

        if (_target != null) {
            // Draw line to the target
            PrepareAttack();
        }
    }

    private void PrepareAttack() {
        // is tower ready to shoot again?
        if (Time.time - _lastAttackTime < _attackCooldown) return;

        // instantiate the bullet and give it its properties 
        GameObject bullet = Instantiate(_bulletPrefab, transform.position, Quaternion.identity);
        bullet.GetComponent<BulletController>();

        if (bullet != null) {
            bullet.GetComponent<BulletController>().SetProperties(_attackDamage, _projectileSpeed, _target);
        }

        // reset cooldown
        _lastAttackTime = Time.time;

    }

    private GameObject FindClosestTarget(List<GameObject> targets) {
        float closestDistance = float.MaxValue;
        GameObject closestTarget = null;
        foreach (var target in targets) {
            float distance = Vector3.Distance(transform.position, target.transform.position);
            if (distance < closestDistance && distance <= _attackRange) {
                closestDistance = distance;
                closestTarget = target;
            }
        }
        return closestTarget;
    }
    private GameObject FindFurthestTarget(List<GameObject> targets) {
        float furthestDistance = 0f;
        GameObject furthestTarget = null;
        foreach (var target in targets) {
            float distance = Vector3.Distance(transform.position, target.transform.position);
            if (distance > furthestDistance && distance <= _attackRange) {
                furthestDistance = distance;
                furthestTarget = target;
            }
        }
        return furthestTarget;
    }

    private bool IsCurrentTargetStillInRange(GameObject target) {
        return Vector3.Distance(transform.position, target.transform.position) <= _attackRange;
    }

}
