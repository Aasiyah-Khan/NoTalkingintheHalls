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

     Story currentStory;

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
    public GameObject charName;

    // to keep track of stuff
    bool dialogueisPlaying;
    int charNum;
    string message;

    private bool talking;
    private bool interacting;
    private string character;

// where the image sprite for the the character speaking goes
    public GameObject speaker;
    // the choice buttons

    // for my choices 
     public GameObject optionPanel;
    public GameObject buttons;
     // this is what the player chooses to select
    static Choice choiceSelected;

    private bool isWaitingForChoice;
    //public Choice element;

    private GameObject currentNPC;



    // setup
 void Start()
{
    // placeholders
    dialoguetxt = "Hello.";
    nametag = "NPC";

    // setup
      NPC1 = new Story(inkFile.storyJson);
    NPC2 = new Story(inkFile2.storyJson);

    // stuff that need to be hiiden (UI)
    dialogueisPlaying = false;
     dialoguePanel.SetActive(false);
        charNum = 0;
        nameUI.SetActive(false);
       
        optionPanel.SetActive(false);
    }

    void Update()
    {
        // always checking if dialogue is playing
        if (!dialogueisPlaying)
        {
            return;
        }
        // currently code above has no purpose -> double check and remove

        // always check game states... if gamestate is 3 exit the dialouge and change the gamestate back to 2
    }

    // the helper function I made to start the dialogue
     public void EnterDialogueMode(Ink.UnityIntegration.InkFile ink)
    {
        dialogueisPlaying = true;
        dialoguePanel.SetActive(true);
      
        if (charNum == 1)
        {
            currentStory = NPC1;

        }
        else if (charNum == 2)
        {
            currentStory = NPC2;
        }
        else
        {
                
            exitDialogueMode();
        }
         // if there was UI
        //charUi.SetActive(true);
        dialoguetxt = currentStory.Continue();
        dialogueText.GetComponent<TextMeshProUGUI>().text = dialoguetxt;
    
    }



   
    // helper function
     void exitDialogueMode()
    {
        // hide everything
        dialogueisPlaying = false;
        dialoguePanel.SetActive(false);
        talking = false;
        interacting = false;
        nameUI.SetActive(false);
        //charUi.SetActive(false); -> unneeded rn
        if(currentNPC.GetComponent<BoxCollider2D>().isTrigger == true)
        {
             currentNPC.GetComponent<BoxCollider2D>().isTrigger = false;
             Debug.Log("trigger removed");
        }
        else
        {
            Debug.Log("trigger already gone");
        }
      

    }


    // this definitely needs to be cleaned up
    public void storyCon()
    {
        if(charNum == 1)
        {
            currentStory = NPC1;
        }
        else if(charNum == 2)
        {
            currentStory = NPC2;
        }
        // always check if dialogue is dialoguing
         if (currentStory.canContinue &&  dialogueisPlaying == true)
        {
                dialoguetxt = currentStory.Continue();
                StopAllCoroutines();
                // this is just to make the sentences type out
                StartCoroutine(TypeSentence(dialoguetxt));

                // store the tags in the story...
                List<string> currentTags = currentStory.currentTags;
                 // then use these tags
                foreach (string tag in currentTags)
                {
                    switch (tag.ToLower())
                    {
                        case "wrong":
                            // subtact from points
                            Debug.Log("Wrong Choice, Loser");
                            break;

                        case "correct":
                            Debug.Log("You're right!");
                            break;
                        
                        case "you":     
                            nametag = "You";
                            nameUI.SetActive(true);
                            break;
                        case "testchar":
                            nametag = "testChar";
                            nameUI.SetActive(true);
                            break;



                }

             
                }

            //SpeakerUi(currentTags);
            charName.GetComponent<TextMeshProUGUI>().text = nametag;
                if(currentStory.currentChoices.Count != 0)
                {
                    ShowChoices();
                }

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

        currentNPC = collider.gameObject;
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



     void ShowChoices()
    {
        isWaitingForChoice = true;
        optionPanel.SetActive(true);

        // Clear any old choices sitting in the panel first
        foreach (Transform child in optionPanel.transform)
        {
            Destroy(child.gameObject);
        }

        List<Choice> choices = currentStory.currentChoices;

        for (int i = 0; i < choices.Count; i++)
        {
            GameObject buttonObj = Instantiate(buttons, optionPanel.transform);
            buttonObj.GetComponentInChildren<TextMeshProUGUI>().text = choices[i].text;

           
            int choiceIndex = i; 

            Button butt = buttonObj.GetComponent<Button>();
            butt.onClick.AddListener(() => MakeChoice(choiceIndex));
        }
    }

    // for when a choice button is clicked
    public void MakeChoice(int index)
    {
        Debug.Log("BUTTON CLICKED! INDEX: " + index);
        currentStory.ChooseChoiceIndex(index); 
        
       // hide my UI
        optionPanel.SetActive(false);
        isWaitingForChoice = false;
        // continue the story
         storyCon();

    }



}





