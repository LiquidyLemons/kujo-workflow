using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform Player;
    [SerializeField] Rigidbody2D rb;
    private Vector3 target;
    private float currentSpeed = 0f; // camera's current speed
    private float moveSpeed = 8f; // starting speed
    private float minSpeed = 2f; 
    private float acceleration = -3f; // deceleration

    public BoxCollider2D boundBox;
    private Vector3 minBounds;
    private Vector3 maxBounds;

    private Camera theCamera;
    private float halfHeight;
    private float halfWidth;

    private void Start()
    {
        // camera bounds and size
        minBounds = boundBox.bounds.min;
        maxBounds= boundBox.bounds.max;
        theCamera = GetComponent<Camera>();
        halfHeight = theCamera.orthographicSize;
        halfWidth = halfHeight * Screen.width / Screen.height;
        rb = GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>();

        target = transform.position;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && rb.bodyType == RigidbodyType2D.Dynamic)
        {
            target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            target.z = transform.position.z;
            Vector2 direction = new Vector2(
                target.x - transform.position.x,
                target.y - transform.position.y);
            currentSpeed = moveSpeed;
        }

        CameraAccerlation();

        transform.position = Vector3.MoveTowards(transform.position, target, currentSpeed * Time.deltaTime);

        // Constricts movement out of bounds
        float clampedX = Mathf.Clamp(transform.position.x, minBounds.x + halfWidth, maxBounds.x - halfWidth);
        float clampedY = Mathf.Clamp(transform.position.y, minBounds.y + halfHeight, maxBounds.y - halfHeight);
        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }

    private void CameraAccerlation()
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
}
