using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu, options, levelSelect;

    public void goToMain()
    {
        mainMenu.SetActive(true);
        options.SetActive(false);
        levelSelect.SetActive(false);
    }

    public void goToOptions()
    {
        mainMenu.SetActive(false);
        options.SetActive(true);
        levelSelect.SetActive(false);
    }

    public void goToLevelSelect()
    {
        mainMenu.SetActive(false);
        options.SetActive(false);
        levelSelect.SetActive(true);
    }
}
