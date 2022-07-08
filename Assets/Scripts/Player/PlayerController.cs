using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float horizontalSpeed = 2f;
    [SerializeField] private float gravity = 9.81f;
    [SerializeField] private float maxFallSpeed = 10f;
    [SerializeField] private float maxRiseSpeed = 10f;
    [SerializeField] private float clickForce = 4f;
    private Rigidbody2D _rb;
    private Vector2 _forceToAdd = Vector2.zero;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.velocity = new Vector2(horizontalSpeed, 0f);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _forceToAdd = new Vector2(0f, clickForce);
        }
    }

    private void FixedUpdate()
    {
        if (_forceToAdd != Vector2.zero)
        {
            _rb.AddForce(_forceToAdd, ForceMode2D.Impulse);
            _forceToAdd = Vector2.zero;
        }
        Vector2 curVelocity = _rb.velocity;
        float newYVelocity = Mathf.Clamp(AddGravity(curVelocity.y), -maxFallSpeed, maxRiseSpeed);
        _rb.velocity = new Vector2(curVelocity.x, newYVelocity);
        
    }

    private float AddGravity(float velocityY)
    {
        velocityY += -gravity * Time.fixedDeltaTime;
        return velocityY;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag.Equals("Wall"))
        {
            Debug.Log("yo");
            Vector2 curVelocity = _rb.velocity;
            _rb.velocity = new Vector2(-curVelocity.x, curVelocity.y);
        }
        if (col.gameObject.tag.Equals("Spike"))
        {
            Debug.Log("hit spike");
            Destroy(gameObject);
        }
    }
}