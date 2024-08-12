using UnityEngine;
using UnityEngine.InputSystem;

public enum Direction { Down, Right, Up, Left, DownLeft, DownRight, UpRight, UpLeft };

public class PlayerActions : MonoBehaviour, HealthSystem
{
    [Header("Player Attributes")]
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;
    [SerializeField] private float speed;
    [SerializeField] private float damage;
    [Header("Player Settings")]
    [SerializeField] private Animator animator;
    [SerializeField] private Direction direction;
    private Rigidbody2D rigidBody;
    private Vector2 movement;
    [Header("Magic Shoot Settings")]
    public GameObject magicShootPrefab;
    private Transform shootParent;
    private Vector3 target;

    // Start is called before the first frame update
    void Awake()
    {
        currentHealth = maxHealth;
        speed = 2;
        rigidBody = GetComponent<Rigidbody2D>();
        animator.SetFloat("x", 0f);
        animator.SetFloat("y", -1f);
        target = CameraTarget.getTarget();
        shootParent = GameObject.FindGameObjectWithTag("PlayerShoot").transform;
    }

    public void setMovement(InputAction.CallbackContext value)
    {
        movement = value.ReadValue<Vector2>();
    }

    public void setDash(InputAction.CallbackContext value)
    {
        if (value.performed)
        {
            rigidBody.AddForce(movement*50);
        }
    }

    public void setShoot(InputAction.CallbackContext value)
    {
        if (value.performed)
        {
            shoot();
        }
    }
    void shoot()
    {
        Instantiate(magicShootPrefab, gameObject.transform.position, Quaternion.identity, shootParent);
    }


    private void idleAnimation()
    {
        if (movement.magnitude > 0)
        {
            float x = animator.GetFloat("Horizontal");
            float y = animator.GetFloat("Vertical");
            animator.SetFloat("x", x);
            animator.SetFloat("y", y);
            setDirection(x, y);
        }
    }

    private void movementAnimation()
    {
        rigidBody.velocity = new Vector2(movement.x * speed, movement.y * speed);
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.magnitude);
    }

    private void FixedUpdate()
    {
        idleAnimation();
        movementAnimation();
    }

    public void setDirection(float x, float y)
    {
        if (x > 0 && y > 0) direction = Direction.UpRight;
        else if (x == 0 && y > 0) direction = Direction.Up;
        else if (x < 0 && y > 0) direction = Direction.UpLeft;
        else if (x < 0 && y == 0) direction = Direction.Left;
        else if (x < 0 && y < 0) direction = Direction.DownLeft;
        else if (x == 0 && y < 0) direction = Direction.Down;
        else if (x > 0 && y < 0) direction = Direction.DownRight;
        else if (x > 0 && y == 0) direction = Direction.Right;
    }

    public void takeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0)
        {
            Debug.Log("Abatido");
        }
    }

    public void heal(float heal)
    {
        currentHealth += heal;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public float getMaxHealth()
    {
        return maxHealth;
    }

    //public void setMaxHealth(float maxHealth)
    //{
    //    this.maxHealth = maxHealth;
    //}

    public float getCurrentHealth()
    {
        return currentHealth;
    }

    //public void setCurrentHealth(float currentHealth)
    //{
    //    this.currentHealth = currentHealth;
    //}

    public float getDamage()
    {
        return damage;
    }
}
