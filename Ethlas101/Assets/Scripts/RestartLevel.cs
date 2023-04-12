using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartLevel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //Reloads the level
    public void ResetLevel() 
    {
        SceneManager.LoadScene("Ethlas101", LoadSceneMode.Single);
    }
}
