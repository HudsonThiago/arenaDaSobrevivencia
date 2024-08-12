using System.Collections;
using UnityEngine;

public class MagicShoot : MonoBehaviour
{
    private GameObject player;
    private Rigidbody2D rb;
    private Vector3 direction;
    [SerializeField] private float force;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        direction = CameraTarget.getTarget() - player.transform.position;
        rb.velocity = new Vector2(direction.x, direction.y).normalized * force;
        float rotation = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotation + 180);
        StartCoroutine(shootFinish());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player") && !collision.gameObject.CompareTag("Untagged"))
        {
            if (collision.gameObject.CompareTag("Monster"))
            {
                Monster monster = collision.gameObject.GetComponent<Monster>();
                PlayerActions playerAction = player.GetComponent<PlayerActions>();
                monster.takeDamage(playerAction.getDamage());
            }
            Destroy(gameObject);

        }
    }

    IEnumerator shootFinish()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
