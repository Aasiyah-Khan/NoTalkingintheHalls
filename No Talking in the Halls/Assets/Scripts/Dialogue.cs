using System.Collections;
using TMPro;
using UnityEngine;


public class Dialogue : MonoBehaviour
{
    // need access to the UI
    public GameObject UI;
    // and the ui text
    public GameObject UIText;
    // and to put the text
    string logs;
    //bool to stop duplicates
    private bool talking;
    private bool interacting;

    public GameObject sprayChoice;
    public GameObject shushChoice;

    private string character;
   

    void Start()
    {
        // hide the UI
        UI.SetActive(false);
        UIText.SetActive(false);
        sprayChoice.SetActive(false);
        shushChoice.SetActive(false);
    }


    void Update()
    {


               
       
    }




    
    IEnumerator UIDisplay(string message)
    {
        // I wanna show the UI
        UI.SetActive(true);
        UIText.SetActive(true);


        // use the typing coroutine i made
        yield return StartCoroutine(TypeSentence(message));






        // wait 2 secs
        yield return new WaitForSeconds(2f);




        // hide the UI
        UI.SetActive(false);
        UIText.SetActive(false);
    }






// to type each friend char by char
    IEnumerator TypeSentence(string sentence)
    {


        logs = "";
        foreach (char letter in sentence.ToCharArray())
        {
            logs += letter;
             UIText.GetComponent<TextMeshProUGUI>().text = logs;
            yield return new WaitForSeconds(0.02f);
        }
        talking = false;
        interacting = false;

        Choices("Test"); 
    }




// just to make this whol process a bit faster (showing dialogue)
    public void Display(string message)
    {
        StartCoroutine(UIDisplay(message));
    }


    // the UI appears after interacting with a character
    void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("Entered trigger");

        character = collider.tag;

        interacting = true;
        Debug.Log("Is interacting");
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        interacting = false;
    }

    public void Interact()
    {


        if (!talking && interacting)
        {
            Debug.Log("Ready to talk");
            talking = true;
            // turn UI on no keypress yet
            UI.SetActive(true);
            UIText.SetActive(true);
            if (character == "testChar")
            {

                logs = "Hi I am a ghost.";


            }
            else if (character == "guy")
            {

                logs = "Hi I am a guy.";
            }
            Display(logs);

        }
        else
        {
            //Debug.Log("Script Not Found");
        }
    }

    // a function for choices 
    private void Choices(string charName){
        // show both choices
        sprayChoice.SetActive(true);
        shushChoice.SetActive(true);
        // then some function for both choices will happen or smth
    }



   


       
}











