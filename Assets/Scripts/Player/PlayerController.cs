using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float horizontalSpeed = 1.5f;
    [SerializeField] private float gravity = 5f;
    [SerializeField] private float maxFallSpeed = 4f;
    [SerializeField] private float maxRiseSpeed = 3f;
    [SerializeField] private float clickForce = 5f;
    [SerializeField] private GameObject hitWallParticle;
    [SerializeField] private GameObject deathParticle;
    [SerializeField] private AudioClip hitWallSound;
    
    private Rigidbody2D _rb;
    private Vector2 _forceToAdd = Vector2.zero;
    private SpriteRenderer _playerSprite;
    public static PlayerController Instance;
    public static event Action OnHitWall = delegate { };
    public static event Action OnDeath = delegate { };

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        _rb = GetComponent<Rigidbody2D>();
        _rb.velocity = new Vector2(horizontalSpeed, 0f);
        _playerSprite = GetComponent<SpriteRenderer>();
        _playerSprite.sprite = SkinManager.Instance.GetSkin();
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
            ObjectAudioController.Instance.PlaySound("Jump");
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
            var particle = Instantiate(hitWallParticle,
                new Vector2(transform.position.x + curVelocity.x > 0f ? 2f : -2f, transform.position.y),
                Quaternion.identity);
            particle.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, curVelocity.x > 0f ? 90f : -90f));
            ObjectAudioController.Instance.PlaySound("HitWall");
            OnHitWall.Invoke();
            return;
        }
        if (col.gameObject.tag.Equals("Spike") || col.gameObject.tag.Equals("KillZone"))
        {
            OnDeath.Invoke();
            Instantiate(deathParticle, transform.position, Quaternion.identity);
            ObjectAudioController.Instance.PlaySound("Die");
            Destroy(gameObject);
            return;
        }
    }
}