using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class XPBar : MonoBehaviour
{
    [SerializeField] private Image xpBar;
    [SerializeField] private TextMeshProUGUI level;
    [SerializeField] private PlayerActions playerAction;


    // Start is called before the first frame update
    void Awake()
    {
        playerAction = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerActions>();
    }

    // Update is called once per frame
    void Update()
    {
        level.text = playerAction.getLevel().ToString();
        xpBar.fillAmount = (float) playerAction.getCurrentXP() / (float) playerAction.getMaxXP();
    }
}
