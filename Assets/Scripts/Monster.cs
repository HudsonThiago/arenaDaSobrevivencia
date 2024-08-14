using System.Collections;
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
    [SerializeField] private GameObject xpObject;
    [SerializeField] private Vector2 movement;
    private Transform xpParent;
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
        xpParent = GameObject.FindGameObjectWithTag("XPParent").transform;
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
            alive = false;
            gameObject.tag = "Untagged";
            damageArea.SetActive(false);
            animator.SetBool("Death", true);
            StartCoroutine(death(0.7f));
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

    IEnumerator death(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
        Instantiate(xpObject, gameObject.transform.position, Quaternion.identity, xpParent);
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
            if (alive)
            {
                HealthSystem healthSystem = collision.gameObject.GetComponent<HealthSystem>();
                healthSystem.takeDamage(damage);
            }
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
