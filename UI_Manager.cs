using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    public GameObject bottomMenuPanel;

    public GameObject storeMenuPanel;
    public GameObject mainMenuPanel;
    public GameObject levelMenuPanel;

    void Start()
    {
        ChangeMenuToMain();
    }


    public void ChangeMenuToMain()
    {
        mainMenuPanel.SetActive(true);
        bottomMenuPanel.SetActive(true);


        storeMenuPanel.SetActive(false);
        levelMenuPanel.SetActive(false);
    }
    
    public void ChangeMenuToStore()
    {
        storeMenuPanel.SetActive(true);

        mainMenuPanel.SetActive(false);
    }

    public void ChangeToLevelMenu()
    {
        levelMenuPanel.SetActive(true);

        mainMenuPanel.SetActive(false);
        bottomMenuPanel.SetActive(false);
    }
}
