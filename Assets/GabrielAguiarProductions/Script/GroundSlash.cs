using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSlash : MonoBehaviour
{
    public float speed = 30;
    public float slowDownRate = 0.01f;
    public float detectingDistance = 0.1f;
    public float destroyDelay = 5;
    private float initialSpeed;

    private Rigidbody rb;
    private bool stopped;

    void Start()
    {
        if (GetComponent<Rigidbody>() != null)
        {
            rb = GetComponent<Rigidbody>();
        }
        else
            Debug.Log("No Rigidbody");

        initialSpeed = speed;
    }

    private void OnEnable()
    {
        stopped = false;
    }

    private void FixedUpdate()
    {
        // Move the object forward by its current speed
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    IEnumerator SlowDown ()
    {
        float t = 1;
        while (t > 0)
        {
            rb.velocity = Vector3.Lerp(Vector3.zero, rb.velocity, t);
            t -= slowDownRate;
            yield return new WaitForSeconds(0.1f);
        }

        stopped = true;
    }
}
