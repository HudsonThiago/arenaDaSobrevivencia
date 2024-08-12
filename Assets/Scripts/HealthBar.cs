using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private PlayerActions playerAction;


    // Start is called before the first frame update
    void Awake()
    {
        playerAction = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerActions>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = playerAction.getCurrentHealth() +"/"+ playerAction.getMaxHealth();
        healthBar.fillAmount = playerAction.getCurrentHealth() / playerAction.getMaxHealth();
    }
}
