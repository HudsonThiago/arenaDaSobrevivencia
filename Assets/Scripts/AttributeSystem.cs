using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public enum Attribute { Health, HealthRegen, Damage, Speed, CollectArea, ShootSpeed };


public class AttributeSystem : MonoBehaviour
{
    public static AttributeSystem instance;
    [SerializeField] private Volume volume;
    private GameObject player;
    private PlayerActions playerActions;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        playerActions = player.GetComponent<PlayerActions>();
    }

    public void choiceAttribute(int attributeIndex)
    {
        PanelController.instance.goToScreen(0);
        Time.timeScale = 1f;
        if (volume.profile.TryGet(out DepthOfField depthOfField))
        {
            depthOfField.active = false;
        }
    }

    public Volume getVolume()
    {
        return volume;
    }
}
