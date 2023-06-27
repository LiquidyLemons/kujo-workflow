using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Obstacles : MonoBehaviour
{
    // public Movement Movement;
    // private float speed;

    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator anim; // controls kujo's animations
    private Vector3 target;
    private ScriptableObject script;
    private GameObject obstacle;

    private float currentSpeed = 0f; // kujo's current speed
    private float moveSpeed = 2f; // kujo's starting speed

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        target = transform.position;
        anim = GetComponent<Animator>();
        // speed = Movement.moveSpeed;
    }

    // Detects collision with current
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Collides with current
        if (collision.gameObject.CompareTag("Current"))
        {
            obstacle = collision.gameObject;   
            Push();
        }

        if (collision.gameObject.CompareTag("Coral"))
        {
            Die();
        }
    }

    // Current
    private void Push()
    {
        currentSpeed = moveSpeed;
        target = new Vector3(transform.position.x-5, transform.position.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, target, currentSpeed * Time.deltaTime);
    }

    // Kujo Death
    private void Die()
    {
        rb.bodyType = RigidbodyType2D.Static;
        anim.SetTrigger("death"); // plays death animation
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
