using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Escape : MonoBehaviour
{
    public GameObject escapePanel;
    void Update (){
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            escapePanel.SetActive(!escapePanel.activeSelf);
        }
    }
    public void doExitGame() {
        Application.Quit();
    }
}