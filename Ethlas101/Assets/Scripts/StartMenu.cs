using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    public GameObject MainMenu;
    public Text ScoreCounter;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0f;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartGame() 
    {
        //Unpauses the game and hides start menu
        MainMenu.SetActive(false);
        ScoreCounter.gameObject.SetActive(true);
        Time.timeScale = 1.0f;
    }
}
