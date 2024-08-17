using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using TMPro;
using UnityEngine.SceneManagement;

[Serializable] public class Rune
{
    [SerializeField] private string attribute;
    [SerializeField] private Sprite sprite;
    [SerializeField] private string description;

    public string getAttribute()
    {
        return attribute;
    }
    public Sprite getSprite()
    {
        return sprite;
    }
    public string getDescription()
    {
        return description;
    }
}

public class AttributeSystem : MonoBehaviour
{
    public static AttributeSystem instance;
    [SerializeField] private Volume volume;
    [SerializeField] private List<GameObject> runeGameObjectList;
    [SerializeField] private List<Rune> runeList;
    private GameObject player;
    private PlayerActions playerActions;
    private List<string> indexList;
    [SerializeField] private TextMeshProUGUI timer;


    // Start is called before the first frame update
    void Awake()
    {
        indexList = new List<string>();
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        playerActions = player.GetComponent<PlayerActions>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameOver();
        }
    }

    public void choiceAttribute(int attributeIndex)
    {
        string attribute = indexList[attributeIndex];
        if (attribute == "Health")
        {
            playerActions.addMaxHealth(50f);
        } else if (attribute == "Damage")
        {
            playerActions.addDamage(10f);
        } else if (attribute == "Speed")
        {
            playerActions.addSpeed(0.4f);
        } else if (attribute == "CollectArea")
        {
            playerActions.addCollectibleArea(0.5f);
        }
        else if (attribute == "HealthRegen")
        {
            playerActions.addHealthRegen(0.5f);
        }
        else if (attribute == "ShootSpeed")
        {
            playerActions.addShootSpeed(0.5f);
        }
        
        PanelController.instance.goToScreen(0);
        activeBlur(false);
    }

    public void sortRune()
    {
        PanelController.instance.goToScreen(1);
        activeBlur(true);
        List<Rune> auxRuneList = new List<Rune>(runeList);
        indexList = new List<string>();
        foreach (GameObject runeObject in runeGameObjectList)
        {
            int randomNumber = UnityEngine.Random.Range(0, auxRuneList.Count);
            indexList.Add(auxRuneList[randomNumber].getAttribute());

            //runeObject.GetComponent<Button>().enabled = false;
            Image icon = runeObject.transform.GetChild(1).GetComponent<Image>();
            TextMeshProUGUI text = runeObject.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
            Rune rune = auxRuneList[randomNumber];
            icon.sprite = rune.getSprite();
            text.text = rune.getDescription();
            auxRuneList.Remove(auxRuneList[randomNumber]);
        }
        //StartCoroutine(enableButton());
    }

    IEnumerator enableButton()
    {
        yield return new WaitForSeconds(1);
        foreach (GameObject runeObject in runeGameObjectList)
        {
            runeObject.GetComponent<Button>().enabled = true;
        }
    }

    public Volume getVolume()
    {
        return volume;
    }

    public void gameOver()
    {
        activeBlur(true);
        PanelController.instance.goToScreen(2);
        timer.text = Timer.getTimerToString();
    }

    public void victory()
    {
        activeBlur(true);
        PanelController.instance.goToScreen(3);
    }

    private void activeBlur(bool trigger)
    {
        if (trigger)
        {
            Time.timeScale = 0f;
        }else
        {
            Time.timeScale = 1f;
        }
        if (AttributeSystem.instance.getVolume().profile.TryGet(out DepthOfField depthOfField))
        {
            depthOfField.active = trigger;
        }
    }
}
