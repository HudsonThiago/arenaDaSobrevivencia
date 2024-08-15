using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleArea : MonoBehaviour
{
    private GameObject player;
    private CircleCollider2D collectibleArea;
    private PlayerActions playerActions;

    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerActions = player.GetComponent<PlayerActions>();
        collectibleArea = GetComponent<CircleCollider2D>();
        collectibleArea.radius = playerActions.getCollectibleArea();
    }

    // Update is called once per frame
    void Update()
    {
        if (collectibleArea.radius != playerActions.getCollectibleArea())
        {
            collectibleArea.radius = playerActions.getCollectibleArea();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CollectibleExperience xp = collision.GetComponent<CollectibleExperience>();
        if (xp != null)
        {
            xp.setFollow();
        }
    }
}
