using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField]
    private Transform _shootPosition;

    [SerializeField]
    private Projectile _bullet;

    [SerializeField]
    private float _fireRate;

    [SerializeField]
    private float _bulletDamage;

    [SerializeField]
    private float _bulletSpeed = 3f;

    public void StartAttack()
    {
        IEAttack();
    }

    private void Start()
    {
        StartCoroutine(IEAttack());
    }

    private IEnumerator IEAttack()
    {
        while (true)
        {
            yield return new WaitForSeconds(_fireRate);

            var projectile = Poolable.TryGetPoolable<Projectile>(_bullet.gameObject);

            if (projectile != null)
            {
                projectile.gameObject.SetActive(true);

                projectile.Initialize(_shootPosition.position, _bulletDamage, _bulletSpeed);
                projectile.StartFly();
            }
        }
    }
}
