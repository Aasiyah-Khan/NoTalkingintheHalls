using UnityEngine;
using Ink.Runtime;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;


public class Dialogue2 : MonoBehaviour
{

    // these are the ink files for each character
    Story NPC1;
    Story NPC2;

    // more ink stuff
    public Ink.UnityIntegration.InkFile inkFile;
    public Ink.UnityIntegration.InkFile inkFile2;


    // to hold text in textbox
    public GameObject dialogueText;

    // the text itself
    string dialoguetxt;

    // entire ui panel
    public GameObject dialoguePanel;


    // for the name of the character
    string nametag;

    // ui for the name
    public GameObject nameUI;

    // to keep track of stuff
    bool dialogueisPlaying;
    int charNum;
    string message;

    private bool talking;
    private bool interacting;
    private string character;


    // setup
 void Start()
{
    // placeholders
    dialoguetxt = "Hello.";
    nametag = "NPC";

    // setup
      NPC1 = new Story(inkFile.storyJson);
    NPC2 = new Story(inkFile2.storyJson);

        dialogueisPlaying = false;
        dialoguePanel.SetActive(false);
        charNum = 0;
    }

    void Update()
    {
        // always checking if dialogue is playing
        if (!dialogueisPlaying)
        {
            return;
        }
        // currently code above has no purpose -> double check and remove
    }

    // the helper function I made to start the dialogue
     public void EnterDialogueMode(Ink.UnityIntegration.InkFile ink)
    {
        dialogueisPlaying = true;
        dialoguePanel.SetActive(true);
      
        if (charNum == 1)
        {
            // if there was UI
            //charUi.SetActive(true);
            if (NPC1.canContinue)
            {
                dialoguetxt = NPC1.Continue();
                dialogueText.GetComponent<TextMeshProUGUI>().text = dialoguetxt;
            }
            else
            {
                exitDialogueMode();
            }
        }
        else if (charNum == 2)
        {
            if (NPC2.canContinue)
            {
                dialoguetxt = NPC2.Continue();
                dialogueText.GetComponent<TextMeshProUGUI>().text = dialoguetxt;
            }
            else
            {
                exitDialogueMode();
            }
        }
    }

    // helper function
     void exitDialogueMode()
    {
        dialogueisPlaying = false;
        dialoguePanel.SetActive(false);
        talking = false;
        interacting = false;
        //charUi.SetActive(false); -> unneeded rn

    }


    // this definitely needs to be cleaned up
    public void storyCon()
    {
         if (NPC1.canContinue &&  dialogueisPlaying == true && charNum == 1)
            {
                dialoguetxt = NPC1.Continue();
                StopAllCoroutines();
                StartCoroutine(TypeSentence(dialoguetxt));
            //textBox.GetComponent<TextMeshProUGUI>().text = dialoguetxt;
            // store the tags in the story...
            List<string> currentTags = NPC1.currentTags;
            //SpeakerUi(currentTags);
            //firstName.GetComponent<TextMeshProUGUI>().text = nametag;

            }
             else if (NPC2.canContinue &&  dialogueisPlaying == true  && charNum == 2)
            {
                dialoguetxt = NPC2.Continue();
                StopAllCoroutines();
                StartCoroutine(TypeSentence(dialoguetxt));
            //textBox.GetComponent<TextMeshProUGUI>().text = dialoguetxt;
            // store the tags in the story...
            List<string> currentTags = NPC2.currentTags;
            //SpeakerUi(currentTags);
            //firstName.GetComponent<TextMeshProUGUI>().text = nametag;

            }
            else
            {
                exitDialogueMode();


            }
    }

    // for input to continue the story    
    public void continueStory(UnityEngine.InputSystem.InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            storyCon();
        }
    }

    // coroutine for typing char by char
    IEnumerator TypeSentence(string sentence)
    {
        message = "";
        foreach (char letter in sentence.ToCharArray())
        {
            message += letter;
            dialogueText.GetComponent<TextMeshProUGUI>().text = message;
            yield return new WaitForSeconds(0.02f);
        }
    }

    // now my trigger enter??
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
           
            if (character == "testChar")
            {

                charNum = 1;
                EnterDialogueMode(inkFile);


            }
            else if (character == "guy")
            {
                charNum = 2;
                EnterDialogueMode(inkFile2);
                
            }
           

        }
        else
        {
            //Debug.Log("Script Not Found");
        }
    }

   



}





