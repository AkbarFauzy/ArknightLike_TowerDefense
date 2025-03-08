using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefence.Module.Characters {
    public class HealerOperator : RangedOperator
    {
        public void Heal()
        {
            if (Targets.Count == 0)
            {
                return;
            }
            else {
                Targets.Sort((x, y) => x.CurrentHP.CompareTo(y.CurrentHP));
            }

            int targetLimit = Mathf.Min(NumberOfAttackedTarget, Targets.Count);

            for (int i = 0; i < targetLimit; i++)
            {
                Targets[i].TakeHeal(CurrentATK);
            }
        }

        protected override IEnumerator MoveProjectileTowardsTarget(Projectile projectile)
        {
            while (Targets.Count > 0)
            {
                Vector3 direction = (Targets[0].transform.position - projectile.transform.position);

                float distanceThisFrame = Time.deltaTime * 15f;

                if (direction.magnitude <= distanceThisFrame)
                {
                    Heal();
                    StartCoroutine(projectile.OnProjectileHit());
                    _projectilePool.Enqueue(projectile);
                    yield break;
                }

                projectile.transform.Translate(direction.normalized * distanceThisFrame, Space.World);
                projectile.transform.rotation = Quaternion.LookRotation(direction);

                yield return null;
            }
        }

        protected new void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("Operator"))
            {
                Operator op = other.gameObject.GetComponent<Operator>();
                if (!op.IsFullHealth && !Targets.Contains(op))
                {
                    Targets.Add(op);
                }
            }
        }
    }
}

