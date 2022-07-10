using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float horizontalSpeed = 1.5f;
    [SerializeField] private float gravity = 5f;
    [SerializeField] private float maxFallSpeed = 4f;
    [SerializeField] private float maxRiseSpeed = 3f;
    [SerializeField] private float clickForce = 5f;
    private Rigidbody2D _rb;
    private Vector2 _forceToAdd = Vector2.zero;
    public static PlayerController Instance;
    public static event Action OnHitWall = delegate { };
    public static event Action OnDeath = delegate { };

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.velocity = new Vector2(horizontalSpeed, 0f);
        if (Instance == null)
        {
            Instance = this;
            return;
        }
        Destroy(gameObject);
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
            Vector2 curVelocity = _rb.velocity;
            _rb.velocity = new Vector2(-curVelocity.x, curVelocity.y);
            OnHitWall.Invoke();
            return;
        }
        if (col.gameObject.tag.Equals("Spike"))
        {
            OnDeath.Invoke();
            Destroy(gameObject);
            return;
        }
        if (col.gameObject.tag.Equals("KillZone"))
        {
            OnDeath.Invoke();
            Destroy(gameObject);
            return;
        }
    }
}