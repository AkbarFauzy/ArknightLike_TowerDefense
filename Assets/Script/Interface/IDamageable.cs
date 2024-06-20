using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void TakeArtDamage(float damage);
    void TakePhysicalDamage(float damage);
    IEnumerator OnDied();
}
