using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private GameObject _hitPrefab;

    private GameObject _hitGameObject;

    private void Start()
    {
        if (_hitPrefab)
        {
            _hitGameObject = Instantiate(_hitPrefab, transform.position, transform.rotation);
            _hitGameObject.SetActive(false);
        }
    }

    public IEnumerator OnProjectileHit() {
        gameObject.SetActive(false);
        _hitGameObject.transform.position = gameObject.transform.position;
        _hitGameObject.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        _hitGameObject.SetActive(false);
    }

}
