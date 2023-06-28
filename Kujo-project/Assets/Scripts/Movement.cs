using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator anim; // controls kujo's animations
    private Vector3 target;

    private Vector3 direction;

    private float currentSpeed = 0f; // kujo's current speed
    private float moveSpeed = 8f; // kujo's starting speed
    private float minSpeed = 2f;
    private float acceleration = -2f; // deceleration of drag, etc.
    //public float rotateSpeed = 25f;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        target = transform.position;
        
        //anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && rb.bodyType == RigidbodyType2D.Dynamic)
        {
            target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            target.z = transform.position.z;

            direction = new Vector3(
                target.x - transform.position.x,
                target.y - transform.position.y,
                transform.position.z);

            transform.right = direction; // faces kujo towards mouse position
            currentSpeed = moveSpeed;
        }

        AccelerateKujo();

        transform.position = Vector3.MoveTowards(transform.position, target, currentSpeed * Time.deltaTime);

        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        return;
    }

    private void AccelerateKujo()
    {
        if (transform.position == target)
        {
            currentSpeed = 0f;
            return;
        }
        else if (currentSpeed > minSpeed)
        {
            currentSpeed += (acceleration * Time.deltaTime);
        }
    }

    // Player Collision
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("collisions <3");
        // Collides with plastic bag or plastic bottle or seaweed
        if (collision.gameObject.CompareTag("Plastic") || collision.gameObject.CompareTag("TempSeaweed"))
        {
            Debug.Log("YOOP SLOW DOWN");
            Slow();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Stops colliding with plastic bag or plastic bottle or seaweed
        if (collision.gameObject.CompareTag("Plastic") || collision.gameObject.CompareTag("TempSeaweed"))
        {
            RevertSpeed();
        }
    }

    // Slows Kujo down
    private void Slow()
    {
        moveSpeed = 4f;

        if (currentSpeed > 0)
        {
            currentSpeed /= 2;
        }
    }

    // Returns to normal speed
    private void RevertSpeed()
    {
        moveSpeed = 8f;
    }
}
