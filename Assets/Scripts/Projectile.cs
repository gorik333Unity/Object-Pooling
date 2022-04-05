using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector3 _startPos;
    private float _damage;
    private float _speed;

    public void Initialize(Vector3 startPos, float damage, float speed)
    {
        _startPos = startPos;
        _damage = damage;
        _speed = speed;
    }

    public void StartFly()
    {
        StartCoroutine(IEBulletMove());
    }

    private IEnumerator IEBulletMove()
    {
        transform.position = _startPos;

        while (true)
        {
            transform.position += Vector3.forward * Time.deltaTime * _speed;

            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        DetectDamageable(other);
    }

    private void DetectDamageable(Collider other)
    {
        var damageable = other.GetComponent<IDamageable>();

        if (damageable != null)
            OnDetectDamageable(damageable);
    }

    private void OnDetectDamageable(IDamageable damageable)
    {
        damageable.TakeDamage(_damage);

        Poolable.TryPool(gameObject);
    }
}
