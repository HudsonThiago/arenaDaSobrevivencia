using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class PanelController : MonoBehaviour
{
    public static PanelController instance;
    [SerializeField] private List<GameObject> screenList;
    [SerializeField] private GameObject currentScreen;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        screenList.ForEach(screen =>
        {
            if (screen.activeSelf == true)
            {
                currentScreen = screen;
            }
        });
    }

    public GameObject getPanel(int index)
    {
        return screenList[index];
    }


    public void goToScreen(int targetScreen)
    {
        currentScreen.SetActive(false);
        screenList[targetScreen].SetActive(true);
        currentScreen = screenList[targetScreen];
    }

    public void loadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
