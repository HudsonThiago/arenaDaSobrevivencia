using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleHealth : MonoBehaviour
{
    [SerializeField] private float heal;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            HealthSystem healthSystem = collision.gameObject.GetComponent<HealthSystem>();
            healthSystem.heal(heal);
            Destroy(gameObject);
        }
    }
}
