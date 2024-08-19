using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public enum Direction { Down, Right, Up, Left, DownLeft, DownRight, UpRight, UpLeft };

public class PlayerActions : MonoBehaviour, HealthSystem
{
    [Header("Main Player Attributes")]
    public float maxHealth;
    public float speed;
    public float damage;
    public float healthRegen = 0.5f;
    public float collectibleRadius = 1f;
    public float shootSpeed = 1f;
    [Header("Other Player Attributes")]
    [SerializeField] private float currentHealth;
    [SerializeField] private int currentXP = 0;
    [SerializeField] private int maxXP = 10;
    [SerializeField] private int level = 1;
    [Header("Player Settings")]
    [SerializeField] private Animator animator;
    [SerializeField] private Direction direction;
    private Rigidbody2D rigidBody;
    private Vector2 movement;
    private bool regenControll = true;
    private bool shootControll = true;
    [Header("Magic Shoot Settings")]
    public GameObject magicShootPrefab;
    private GameObject shootParent;
    private Vector3 target;
    private CircleCollider2D collectibleArea;

    // Start is called before the first frame update
    void Awake()
    {
        currentHealth = maxHealth;
        rigidBody = GetComponent<Rigidbody2D>();
        collectibleArea = transform.GetChild(0).GetComponent<CircleCollider2D>();
        animator.SetFloat("x", 0f);
        animator.SetFloat("y", -1f);
        target = CameraTarget.getTarget();
        shootParent = GameObject.FindGameObjectWithTag("PlayerShoot").gameObject;
    }

    private void FixedUpdate()
    {
        idleAnimation();
        movementAnimation();
    }

    private void Update()
    {
        if (regenControll && currentHealth < maxHealth)
        {
            StartCoroutine(healthRegenSystem());
        }
    }

    IEnumerator healthRegenSystem()
    {
        regenControll = false;
        yield return new WaitForSeconds(1/healthRegen);
        heal(1);
        regenControll = true;
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
        if (value.performed && shootControll)
        {
            StartCoroutine(shoot());
        }
    }
    IEnumerator shoot()
    {
        shootControll = false;
        SoundController.instance.echoSound(0);
        Vector3 shootPosition = gameObject.transform.position;
        shootPosition.y += 0.3f;
        Instantiate(magicShootPrefab, shootPosition, Quaternion.identity, shootParent.transform);
        yield return new WaitForSeconds(1/shootSpeed);
        shootControll = true;
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
            AttributeSystem.instance.gameOver();
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

    public void addMaxHealth(float add)
    {
        maxHealth += add;
        currentHealth += add;
    }

    public float getCurrentHealth()
    {
        return currentHealth;
    }

    public int getMaxXP()
    {
        return maxXP;
    }

    public int getCurrentXP()
    {
        return currentXP;
    }

    public float getDamage()
    {
        return damage;
    }

    public void addDamage(float add)
    {
        damage += add;
    }
    public void addSpeed(float add)
    {
        speed += add;
    }
    public void addCollectibleArea(float add)
    {
        collectibleRadius += add;
        collectibleArea.radius = collectibleRadius;
    }

    public void addHealthRegen(float add)
    {
        healthRegen += add;
    }

    public void addShootSpeed(float add)
    {
        shootSpeed += add;
    }

    public int getLevel()
    {
        return level;
    }

    public void addXP(int xp)
    {
        currentXP += xp;
        if (currentXP >= maxXP)
        {
            levelUp();
        }
    }

    private void levelUp()
    {
        level++;
        SoundController.instance.echoSound(1);
        currentXP = currentXP - maxXP;
        increaseMaxXp();
        heal((int) maxHealth / 4);
        AttributeSystem.instance.sortRune();
    }

    private void increaseMaxXp()
    {
        if (level == 1) maxXP = 10;
        else if (level == 2) maxXP = 25;
        else if (level == 3) maxXP = 50;
        else if (level == 4) maxXP = 100;
        else maxXP += 25;
    }

    public float getCollectibleArea()
    {
        return collectibleRadius;
    }

    public void getCollectibleArea(float collectibleRadius)
    {
        this.collectibleRadius = collectibleRadius;
    }
}
