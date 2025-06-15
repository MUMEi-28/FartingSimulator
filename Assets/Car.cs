using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public GameObject[] tires;
    public GameObject[] wayPoints;
    public Transform centerOfMass;
    private Rigidbody rb;
    private Collider carCollider;
    private Collider[] tireColliders;

    public Transform LookAtReference;

    public float rotationSpeed;
    public float tireRotateSpeed;
    public float moveSpeed;

    private int currentIndex;
    private void Start()
    {
        wayPoints = GameObject.FindGameObjectsWithTag("WayPoint");

        rb = GetComponent<Rigidbody>();
        carCollider = GetComponent<Collider>();

        for (int i = 0; i < tires.Length; i++)
        {
           tireColliders =  tires[i].GetComponents<Collider>();
        }
    }
    private void Update()
    {
        RotateTires();
        Move();
    }

    private void FixedUpdate()
    {
        IgnoreTireCollisions();
        LookAt();

  //      rb.centerOfMass = centerOfMass.position;
    }
    private void LookAt()
    {
        Vector3 targetPosition = new Vector3(wayPoints[currentIndex].transform.position.x, transform.position.y, wayPoints[currentIndex].transform.position.z);
        Vector3 direction = targetPosition - transform.position;

        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);

        // Interpolate between the current rotation and the target rotation
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
    private void Move()
    {
        if (Vector3.Distance(transform.position, wayPoints[currentIndex].transform.position) <= 0.5f)
        {
            currentIndex++;
        }
        if (currentIndex >= wayPoints.Length)
        {
            currentIndex = 0;
        }
        transform.position = Vector3.MoveTowards(transform.position, wayPoints[currentIndex].transform.position, moveSpeed * Time.deltaTime);
    }
    private void RotateTires()
    {
        for (int i = 0; i < tires.Length; i++)
        {
            tires[i].transform.Rotate(Vector3.right, rb.velocity.magnitude * tireRotateSpeed);
        }
    }
    private void IgnoreTireCollisions()
    {
        foreach (Collider col in tireColliders)
        {

            Physics.IgnoreCollision(carCollider, col);     //  IGNORES THE TIRE COLLIDERS 
        }
    }
    
}
