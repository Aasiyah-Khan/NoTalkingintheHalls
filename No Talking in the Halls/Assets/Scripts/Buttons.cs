using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    int gamestate;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // for the play button
    public void Play()
    {
      // change state of game
       GameManager.instance.gameState = 1;
       // change scene
        SceneManager.LoadScene("Library");
        
    }

    public void Spray()
    {
        // check if talking to ghost
        if(GameManager.instance.ghost == false)
        {
            
        }
    }
}
