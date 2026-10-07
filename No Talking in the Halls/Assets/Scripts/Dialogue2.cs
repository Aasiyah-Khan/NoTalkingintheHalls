using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Dialogue2 : MonoBehaviour
{

    // these are the ink files for each character
    Story NPC1;
    Story NPC2;
    Story NPC3;

    // so the script knows what ink file is being read
     Story currentStory;

    // more ink stuff
    public Ink.UnityIntegration.InkFile inkFile;
    public Ink.UnityIntegration.InkFile inkFile2;
    public Ink.UnityIntegration.InkFile inkFile3;


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

// to know if player has choice options on screen
    private bool ismakingChoice;

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
    NPC3 = new Story(inkFile3.storyJson);

    // stuff that need to be hiiden (UI)
    dialogueisPlaying = false;
     dialoguePanel.SetActive(false);
        charNum = 0;
        nameUI.SetActive(false);
       
        optionPanel.SetActive(false);

        // set the bools
        ismakingChoice = false;
        interacting = false;

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
      
      // depending on the character interacted with the current story file is set
        if (charNum == 1)
        {
            currentStory = NPC1;
            // to display the first tag in the story 
       

        }
        else if (charNum == 2)
        {
            currentStory = NPC2;
            // to display the first tag in the story 
    
        }
        else if (charNum == 3)
        {
            currentStory = NPC3;
            // to display the first tag in the story 
        
        }
        else
        {
                
            exitDialogueMode();
        }
        
         // if there was UI
        //charUi.SetActive(true);
        dialoguetxt = currentStory.Continue();
        dialogueText.GetComponent<TextMeshProUGUI>().text = dialoguetxt;
        // so you can see the first tag
        tagSorter();
    
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
             currentNPC.GetComponent<BoxCollider2D>().enabled = false;
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
        else if (charNum == 3)
        {
            currentStory = NPC3;
        }

        // if youre in choice mode just end this whole thing
        if (ismakingChoice || !dialogueisPlaying || currentStory == null)
        {
            return;
        }
       
        // always check if dialogue is dialoguing
        if (currentStory != null && currentStory.canContinue && dialogueisPlaying == true)
        {
            dialoguetxt = currentStory.Continue();
            StopAllCoroutines();
            // this is just to make the sentences type out
            StartCoroutine(TypeSentence(dialoguetxt));

            // run my tag sorter function
            tagSorter(); 
            //SpeakerUi(currentTags);
           
            if (currentStory.currentChoices.Count != 0)
            {
                ShowChoices();
            }

        }
        else
        {
            exitDialogueMode();


        }
    }

    // a method to show nametags and control other things based on ink tags
    // moved this so i dont have to repeat it
    void tagSorter()
    {
        
            // store the tags in the story...
            List<string> currentTags = currentStory.currentTags;
            // then use these tags
            foreach (string tag in currentTags)
            {
                // so its case insensitive turn everything to lowercase
                // all tested cases must be lowercase regardless of their original spelling
                switch (tag.ToLower())
                {
                        case "wronglady":
                            // subtact from points
                            Debug.Log("Wrong Choice, Loser");
                            GameManager.instance.humanChances--;
                            GameManager.instance.LadyNeutral.SetActive(false);
                            GameManager.instance.LadyUpset.SetActive(true);
                            if (GameManager.instance.humanChances <= 2)
                            {
                                GameManager.instance.HR1.SetActive(true);
                            }
                            if (GameManager.instance.humanChances <= 1)
                            {
                                GameManager.instance.HR2.SetActive(true);
                            }
                            if (GameManager.instance.humanChances <= 0)
                            {
                                GameManager.instance.HR3.SetActive(true);
                                SceneManager.LoadScene("Lose");
                            }
                        break;

                        case "wrongchild":
                            // subtact from points
                            Debug.Log("Wrong Choice, Loser");
                            GameManager.instance.humanChances--;
                            GameManager.instance.ChildNeutral.SetActive(false);
                            GameManager.instance.ChildUpset.SetActive(true);
                            if (GameManager.instance.humanChances <= 2)
                            {
                                GameManager.instance.HR1.SetActive(true);
                            }
                            if (GameManager.instance.humanChances <= 1)
                            {
                                GameManager.instance.HR2.SetActive(true);
                            }
                            if (GameManager.instance.humanChances <= 0)
                            {
                                GameManager.instance.HR3.SetActive(true);
                                SceneManager.LoadScene("Lose");
                            }
                        break;

                        case "wrongnerd":
                            // subtact from points
                            Debug.Log("Wrong Choice, Loser");
                            GameManager.instance.humanChances--;
                            GameManager.instance.ChildNeutral.SetActive(false);
                            GameManager.instance.ChildUpset.SetActive(true);
                            if (GameManager.instance.humanChances <= 2)
                            {
                                GameManager.instance.HR1.SetActive(true);
                            }
                            if (GameManager.instance.humanChances <= 1)
                            {
                                GameManager.instance.HR2.SetActive(true);
                            }
                            if (GameManager.instance.humanChances <= 0)
                            {
                                GameManager.instance.HR3.SetActive(true);
                                SceneManager.LoadScene("Lose");
                            }
                        break;

                        case "wrongghost1":
                            GameManager.instance.ghostChances--;
                            GameManager.instance.Sound1.SetActive(false);
                            if (GameManager.instance.ghostChances <= 2)
                            {
                                GameManager.instance.Ghost1.SetActive(true);
                            }
                            if (GameManager.instance.ghostChances <= 1)
                            {
                                GameManager.instance.Ghost2.SetActive(true);
                            }
                            if (GameManager.instance.ghostChances <= 0)
                            {
                                GameManager.instance.Ghost3.SetActive(true);
                                SceneManager.LoadScene("Lose");
                            }
                        break;

                        case "wrongghost2":
                            GameManager.instance.ghostChances--;
                            GameManager.instance.Sound2.SetActive(false);
                            if (GameManager.instance.ghostChances <= 2)
                            {
                                GameManager.instance.Ghost1.SetActive(true);
                            }
                            if (GameManager.instance.ghostChances <= 1)
                            {
                                GameManager.instance.Ghost2.SetActive(true);
                            }
                            if (GameManager.instance.ghostChances <= 0)
                            {
                                GameManager.instance.Ghost3.SetActive(true);
                                SceneManager.LoadScene("Lose");
                            }
                        break;

                        case "wrongghost3":
                            GameManager.instance.ghostChances--;
                            GameManager.instance.Sound3.SetActive(false);
                            if (GameManager.instance.ghostChances <= 2)
                            {
                                GameManager.instance.Ghost1.SetActive(true);
                            }
                            if (GameManager.instance.ghostChances <= 1)
                            {
                                GameManager.instance.Ghost2.SetActive(true);
                            }
                            if (GameManager.instance.ghostChances <= 0)
                            {
                                GameManager.instance.Ghost3.SetActive(true);
                                SceneManager.LoadScene("Lose");
                            }
                        break;

                        case "rightlady":
                            Debug.Log("You're right!");
                            GameManager.instance.correct++;
                            GameManager.instance.LadyNeutral.SetActive(false);
                            GameManager.instance.LadyHappy.SetActive(true);
                            if (GameManager.instance.correct == 3)
                            {
                                SceneManager.LoadScene("Win");
                            }
                        break;

                        case "rightchild":
                            Debug.Log("You're right!");
                            GameManager.instance.correct++;
                            GameManager.instance.ChildNeutral.SetActive(false);
                            GameManager.instance.ChildHappy.SetActive(true);
                            if (GameManager.instance.correct == 3)
                            {
                                SceneManager.LoadScene("Win");
                            }
                        break;

                        case "rightnerd":
                            Debug.Log("You're right!");
                            GameManager.instance.correct++;
                            GameManager.instance.NerdNeutral.SetActive(false);
                            GameManager.instance.NerdHappy.SetActive(true);
                            if (GameManager.instance.correct == 3)
                            {
                                SceneManager.LoadScene("Win");
                            }
                        break;

                        case "you":
                            nametag = "You";
                            charName.GetComponent<TextMeshProUGUI>().color = new Color(0.3215686f, 0.04705883f, 0.4862745f);
                            nameUI.SetActive(true);
                        break;
                        case "lady":
                            nametag = "Lady";
                            charName.GetComponent<TextMeshProUGUI>().color = new Color(0.1843137f, 0.5607843f, 0.3568628f);
                            nameUI.SetActive(true);
                        break;
                        case "child":
                            nametag = "Child";
                            charName.GetComponent<TextMeshProUGUI>().color = new Color(0, 0.5764706f, 0.7333333f);
                            nameUI.SetActive(true);
                        break;
                        case "nerd":
                            nametag = "Nerd";
                            charName.GetComponent<TextMeshProUGUI>().color = new Color(0.8509805f, 0.7294118f, 0.09803922f);
                            nameUI.SetActive(true);
                        break;
                }


            }
             charName.GetComponent<TextMeshProUGUI>().text = nametag;
    }

    // for input to continue the story    
    public void continueStory(UnityEngine.InputSystem.InputAction.CallbackContext callbackContext)
    {
        // i need to check if there even is a character being interacted with... 
        // also need to make sure tehre are no choices on the screen
        if (callbackContext.performed && currentStory != null && ismakingChoice == false && dialogueisPlaying )
        {
            // then run whatever is in the continue story function
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
            // how fast each letter appears
            yield return new WaitForSeconds(0.02f);
        }
    }

    // now my trigger enter??
     // the UI appears after interacting with a character
    void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("Entered trigger");

        // the current character is set to whatever one you triggered
        character = collider.tag;

        // now that interacting is true, you can press E
        interacting = true;
        Debug.Log("Is interacting");

        currentNPC = collider.gameObject;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        interacting = false;
    }


// for when you press E
    public void Interact()
    {


        if (!talking && interacting)
        {
            Debug.Log("Ready to talk");
            talking = true;
           
            if (character == "Lady")
            {
                // this way of doing things is so uncessary but yeah the character number is also set
                charNum = 1;
                EnterDialogueMode(inkFile);
            


            }
            else if (character == "Child")
            {
                charNum = 2;
                EnterDialogueMode(inkFile2);
                
            }
            else if (character == "Nerd")
            {
                charNum = 3;
                EnterDialogueMode(inkFile3);

            }
        }
        else
        {
            Debug.Log("Script Not Found");
        }
    }


    // this is when a choice appears
     void ShowChoices()
    {
        // now i shouldnt be able to continue with the story until a choice is made
        ismakingChoice = true;
        // i think i can remove this var
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
            // make the choice buttons appear for as many choices are available 
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
        // once again i think this var is the same as the one below...
        isWaitingForChoice = false;
         ismakingChoice = false;
        // make it so that you can press continue again
        //ismakingChoice = false;
        // continue the story
         storyCon();
        

    }



} 





