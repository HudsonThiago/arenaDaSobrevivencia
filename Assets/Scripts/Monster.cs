using UnityEngine;
using UnityEngine.Tilemaps;

public class Monster : MonoBehaviour, HealthSystem
{
    [Header("Monster Attributes")]
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;
    [SerializeField] private float speed;
    [SerializeField] private float damage;
    private bool alive;
    [Header("Monster Settings")]
    [SerializeField] private Vector2 movement;
    private TilemapCollider2D wallCollider;
    private CircleCollider2D monsterCollider;
    private Transform target;
    private Animator animator;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private GameObject damageArea;
    private bool flipControll = true;

    // Start is called before the first frame update
    void Awake()
    {
        currentHealth = maxHealth;
        alive = true;
        damageArea = gameObject.transform.GetChild(0).gameObject;
        rb = GetComponent<Rigidbody2D>();
        monsterCollider = damageArea.GetComponent<CircleCollider2D>();
        wallCollider = GameObject.FindGameObjectWithTag("Wall").GetComponent<TilemapCollider2D>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        Physics2D.IgnoreCollision(monsterCollider, wallCollider, true);
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (alive)
        {
            Vector3 direction = target.position - transform.position;
            direction.Normalize();
            movement = direction;
            monsterFollow(movement);
        }
    }

    public void takeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Debug.Log("Abatido");
            alive = false;
            CircleCollider2D collider = damageArea.GetComponent<CircleCollider2D>();
            collider.enabled = false;
            animator.SetBool("Death", true);
        }
    }

    public void heal(float heal)
    {
        currentHealth += heal;
        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public void death()
    {
        Destroy(gameObject);
    }

    private void monsterFollow(Vector2 direction)
    {
        rb.MovePosition((Vector2)transform.position + (direction * speed * Time.deltaTime));
        if (direction.x >= 0 && !flipControll)
        {
            flip();
        } else if (direction.x < 0 && flipControll)
        {
            flip();
        }
    }

    private void flip()
    {
        Vector3 scale = gameObject.transform.localScale;
        scale.x *= -1;
        gameObject.transform.localScale = scale;
        flipControll = !flipControll;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            HealthSystem healthSystem = collision.gameObject.GetComponent<HealthSystem>();
            healthSystem.takeDamage(damage);
        }
    }

    public float getMaxHealth()
    {
        return maxHealth;
    }

    public float getCurrentHealth()
    {
        return currentHealth;
    }
}
