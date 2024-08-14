using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEditor.ShaderGraph;
using UnityEngine;

public class CollectibleExperience : MonoBehaviour
{
    [SerializeField] private int xp;
    private GameObject player;
    private PlayerActions playerAction;
    private Transform target;
    private bool follow = false;
    private bool increaseSpeedControll = true;
    private Rigidbody2D rb;
    private float speed = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerAction = player.GetComponent<PlayerActions>();
        target = player.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (follow)
        {
            if (increaseSpeedControll)
            {
                StartCoroutine(increaseXPSpeed());
            }
            Vector3 direction = target.position - transform.position;
            direction.Normalize();
            Vector3 movement = direction;
            rb.MovePosition((Vector3)transform.position + (movement * speed * Time.deltaTime));

            if (Vector3.Distance(transform.position, target.position) < 0.2f)
            {
                playerAction.addXP(xp);
                Destroy(gameObject);
            }
        }
    }

    IEnumerator increaseXPSpeed()
    {
        increaseSpeedControll = false;
        speed += speed;
        yield return new WaitForSeconds(0.2f);
        increaseSpeedControll = true;
    }

    public void setFollow()
    {
        follow = true;
    }
}
