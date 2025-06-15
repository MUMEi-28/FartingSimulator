using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardBullet : MonoBehaviour
{
    public float moveSpeed;
    public float damage;

    public float destroyTime;
    private void Start()
    {
        Destroy(gameObject, destroyTime);
    }
    void Update()
    {
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            FindObjectOfType<PlayerController>().TakeDamage(damage);
        }
    }
}
