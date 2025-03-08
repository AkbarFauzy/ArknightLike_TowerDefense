using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefence.Module.Characters {
    public class Enemy_NormalRanged : Enemy
    {
        [Space]
        [Header("Projectiles")]
        [SerializeField] private GameObject _projectilePrefab;
        [SerializeField] private Transform _projectileSpawnPoint;
        private Queue<Projectile> _projectilePool;

        protected override void Start()
        {
            base.Start();
            _projectilePool = new Queue<Projectile>();
            for (int i = 0; i < 3; i++)
            {
                GameObject projectile = Instantiate(_projectilePrefab, _projectileSpawnPoint.position, _projectileSpawnPoint.rotation);
                projectile.SetActive(false);
                _projectilePool.Enqueue(projectile.GetComponent<Projectile>());
            }
        }

        public void ShootProjectile()
        {
            if (_projectilePool.Count > 0)
            {
                Projectile currentProjectile = _projectilePool.Dequeue();
                currentProjectile.transform.SetPositionAndRotation(_projectileSpawnPoint.position, _projectileSpawnPoint.rotation);
                currentProjectile.gameObject.SetActive(true);

                StartCoroutine(MoveProjectileTowardsTarget(currentProjectile));
            }
        }

        private IEnumerator MoveProjectileTowardsTarget(Projectile projectile)
        {
            while (Targets.Count > 0)
            {
                Debug.Log("Moving Projectile");
                Vector3 direction = (Targets[0].transform.position - projectile.transform.position);

                float distanceThisFrame = Time.deltaTime * 15f;

                if (direction.magnitude <= distanceThisFrame)
                {
                    Attack();
                    yield return StartCoroutine(projectile.OnProjectileHit());
                    _projectilePool.Enqueue(projectile);
                    break;
                }

                projectile.transform.Translate(direction.normalized * distanceThisFrame, Space.World);
                projectile.transform.rotation = Quaternion.LookRotation(direction);

                yield return null;
            }

            projectile.gameObject.SetActive(false);
            _projectilePool.Enqueue(projectile);

            yield return null;
        }


    }

}
