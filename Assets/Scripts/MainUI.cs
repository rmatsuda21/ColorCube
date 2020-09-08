using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUI : MonoBehaviour
{
    public GameObject colorSelector;
    public GameObject lvlBtns, win, lose;

    public enum Elements
    {
        ColorSelector,
        LvlBtns,
        Win,
        Lose,
        None
    }

    private void Awake()
    {
        colorSelector.SetActive(true);
        colorSelector.SetActive(false);
        lvlBtns.SetActive(false);
        win.SetActive(false);
        lose.SetActive(false);
    }

    public void setActive(Elements e)
    {
        colorSelector.SetActive(false);
        lvlBtns.SetActive(false);
        win.SetActive(false);
        lose.SetActive(false);

        if (e == Elements.ColorSelector)
            colorSelector.SetActive(true);
        else if (e == Elements.LvlBtns)
            lvlBtns.SetActive(true);
        else if (e == Elements.Win)
            win.SetActive(true);
        else if (e == Elements.Lose)
            lose.SetActive(true);
    }
}
