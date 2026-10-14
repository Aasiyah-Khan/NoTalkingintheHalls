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
    Story NPC4;
    Story NPC5;
    Story NPC6;

    // so the script knows what ink file is being read
     Story currentStory;

    // more ink stuff
    public Ink.UnityIntegration.InkFile inkFile;
    public Ink.UnityIntegration.InkFile inkFile2;
    public Ink.UnityIntegration.InkFile inkFile3;
    public Ink.UnityIntegration.InkFile inkFile4;
    public Ink.UnityIntegration.InkFile inkFile5;
    public Ink.UnityIntegration.InkFile inkFile6;


    // to hold text in textbox
    [SerializeField] private GameObject dialogueText;

    // the text itself
    string dialoguetxt;

    // entire ui panel
     [SerializeField] private GameObject dialoguePanel;


    // for the name of the character
    string nametag;
    
    // ui for the name
    [SerializeField] private GameObject nameUI;
    [SerializeField] private GameObject nameUIYou;
    [SerializeField] private GameObject charName;
    [SerializeField] private GameObject yourName;

    // to keep track of stuff
    bool dialogueisPlaying;
    int charNum;
    string message;

    private bool talking;
    private bool interacting;
    private string character;

// to know if player has choice options on screen
    private bool ismakingChoice;


    // the choice buttons

    // for my choices 
    [SerializeField] private GameObject optionPanel;
   [SerializeField] private GameObject buttons;
     // this is what the player chooses to select
    static Choice choiceSelected;

   
    //public Choice element;

    private GameObject currentNPC;

    public AudioSource crowd;

// where the image sprite for the the character speaking goes
     [SerializeField] private  GameObject speaker;
      [SerializeField] private  GameObject speakerYou;

    // all character images below
    [SerializeField] private Sprite nerdNorm;


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
    NPC4 = new Story(inkFile4.storyJson);
    NPC5 = new Story(inkFile5.storyJson);
    NPC6 = new Story(inkFile6.storyJson);

    // stuff that need to be hiiden (UI)
    dialogueisPlaying = false;
     dialoguePanel.SetActive(false);
        charNum = 0;
        nameUI.SetActive(false);
        nameUIYou.SetActive(false);
        speaker.SetActive(false);
        speakerYou.SetActive(false);
       
        optionPanel.SetActive(false);

        // set the bools
        ismakingChoice = false;
        interacting = false;

        //crowd = GetComponent<AudioSource>();

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
        else if (charNum == 4)
        {
            currentStory = NPC4;
            // to display the first tag in the story 

        }
        else if (charNum == 5)
        {
            currentStory = NPC5;
            // to display the first tag in the story 

        }
        else if (charNum == 6)
        {
            currentStory = NPC6;
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
        speaker.SetActive(false);
        speakerYou.SetActive(false);
        nameUIYou.SetActive(false);
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
        else if( charNum == 4)
        {
            currentStory = NPC4;
        }
        else if (charNum == 5)
        {
            currentStory = NPC5;
        }
        else if (charNum == 6)
        {
            currentStory = NPC6;
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
                            GameManager.instance.GentlemanNeutral.SetActive(false);
                            GameManager.instance.GentlemanAngry.SetActive(true);
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
                                SceneManager.LoadScene("GhostLose");
                            }
                        break;

                        case "wrongghost2":
                            GameManager.instance.ghostChances--;
                            GameManager.instance.BuffGuyNeutral.SetActive(false);
                            GameManager.instance.BuffGuyAngry.SetActive(true);
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
                                SceneManager.LoadScene("GhostLose");
                            }
                        break;

                        case "wrongghost3":
                            GameManager.instance.ghostChances--;
                            GameManager.instance.ThespianNeutral.SetActive(false);
                            GameManager.instance.ThespianAngry.SetActive(true);
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
                                SceneManager.LoadScene("GhostLose");
                            }
                        break;

                        case "rightlady":
                            Debug.Log("You're right!");
                            GameManager.instance.correct++;
                            GameManager.instance.LadyNeutral.SetActive(false);
                            GameManager.instance.LadyHappy.SetActive(true);
                            crowd.volume -= 0.17f;
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
                            crowd.volume -= 0.17f;
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
                            crowd.volume -= 0.17f;
                            if (GameManager.instance.correct == 6)
                            {
                                SceneManager.LoadScene("Win");
                            }
                        break;

                        case "rightgentleman":
                            Debug.Log("You're right!");
                            GameManager.instance.correct++;
                            GameManager.instance.GentlemanNeutral.SetActive(false);
                            GameManager.instance.Sound1.SetActive(false);
                            crowd.volume -= 0.17f;
                            if (GameManager.instance.correct == 6)
                            {
                                SceneManager.LoadScene("Win");
                            }
                        break;

                        case "rightbuff":
                            Debug.Log("You're right!");
                            GameManager.instance.correct++;
                            GameManager.instance.BuffGuyNeutral.SetActive(false);
                            GameManager.instance.Sound2.SetActive(false);
                            crowd.volume -= 0.17f;
                            if (GameManager.instance.correct == 6)
                            {
                                SceneManager.LoadScene("Win");
                            }
                        break;

                        case "rightthespian":
                            Debug.Log("You're right!");
                            GameManager.instance.correct++;
                            GameManager.instance.ThespianNeutral.SetActive(false);
                            GameManager.instance.Sound3.SetActive(false);
                            crowd.volume -= 0.15f;
                            if (GameManager.instance.correct == 6)
                            {
                                SceneManager.LoadScene("Win");
                            }
                        break;

                        case "you":
                            //nametag = "You";
                            //charName.GetComponent<TextMeshProUGUI>().color = new Color(0.3215686f, 0.04705883f, 0.4862745f);
                            nameUIYou.SetActive(true);
                            nameUI.SetActive(false);
                            // hide the character you're speaking with 
                            speaker.SetActive(false);
                            // show you
                            speakerYou.SetActive(true);
                        break;
                        case "lady":
                            nameUIYou.SetActive(false);
                            nametag = "Lady";
                            charName.GetComponent<TextMeshProUGUI>().color = new Color(0.1843137f, 0.5607843f, 0.3568628f);
                            nameUI.SetActive(true);
                            // hide you
                            speakerYou.SetActive(false);
                            // gotta set the sprite for the lady first then set the speaker UI active
                            // change sprite here

                             speaker.SetActive(true);

                        break;
                        case "child":
                            nameUIYou.SetActive(false);
                            nametag = "Child";
                            charName.GetComponent<TextMeshProUGUI>().color = new Color(0, 0.5764706f, 0.7333333f);
                            nameUI.SetActive(true);
                            speakerYou.SetActive(false);
                            // change sprite here

                             speaker.SetActive(true);
                        break;
                        case "nerd":
                            nameUIYou.SetActive(false);
                            nametag = "Nerd";
                            charName.GetComponent<TextMeshProUGUI>().color = new Color(0.8509805f, 0.7294118f, 0.09803922f);
                            nameUI.SetActive(true);
                            speakerYou.SetActive(false);
                            // this is where showing the sprite would happen
                            speaker.GetComponent<Image>().sprite = nerdNorm;
                            speaker.SetActive(true);

                        break;
                        case "gentleman":
                            nameUIYou.SetActive(false);
                            nametag = "Gentleman";
                            charName.GetComponent<TextMeshProUGUI>().color = new Color(0.882353f, 0.03921569f, 0f);
                            nameUI.SetActive(true);
                            speakerYou.SetActive(false);

                            // change sprite here

                             speaker.SetActive(true);
                            break;
                        case "buff":
                            nameUIYou.SetActive(false);
                            nametag = "Buff Guy";
                            charName.GetComponent<TextMeshProUGUI>().color = new Color(0.08235294f, 0.3372549f, 0.9568628f);
                            nameUI.SetActive(true);
                            speakerYou.SetActive(false);
                            // change sprite here

                             speaker.SetActive(true);
                            break;
                        case "thespian":
                            nameUIYou.SetActive(false);
                            nametag = "Thespian";
                            charName.GetComponent<TextMeshProUGUI>().color = new Color(0.4156863f, 0.3490196f, 0.007843138f);
                            nameUI.SetActive(true);
                            speakerYou.SetActive(false);
                            // change sprite here

                             speaker.SetActive(true);
                            break;
                        // don't add anything below default
                        default:
                        // no Ui shows
                            nameUIYou.SetActive(false);
                            nameUI.SetActive(false);
                            speaker.SetActive(false);
                            speakerYou.SetActive(false);
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
            else if (character == "Gentleman")
            {
                charNum = 4;
                EnterDialogueMode(inkFile4);

            }
            else if (character == "Buff Guy")
            {
                charNum = 5;
                EnterDialogueMode(inkFile5);

            }
            else if (character == "Thespian")
            {
                charNum = 6;
                EnterDialogueMode(inkFile6);

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
       
         ismakingChoice = false;
        // make it so that you can press continue again
        //ismakingChoice = false;
        // continue the story
         storyCon();
        

    }



} 





