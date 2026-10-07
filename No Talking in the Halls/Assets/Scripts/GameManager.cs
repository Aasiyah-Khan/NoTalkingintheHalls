using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // so that the game manager doesnt get destroyed during scene changes
    public static GameManager instance;
    // when the game starts --> when first scene is opened
    // state management
    public int ghostChances;
    public int humanChances;
    public int correct;

    public string characterName;

    public int gameState;

    // so the game can tell whether or not you're talking to a ghost
    public bool ghost;

    public GameObject HR1;
    public GameObject HR2;
    public GameObject HR3;

    public GameObject Ghost1;
    public GameObject Ghost2;
    public GameObject Ghost3;

    public GameObject Sound1;
    public GameObject Sound2;
    public GameObject Sound3;

    public GameObject LadyNeutral;
    public GameObject LadyHappy;
    public GameObject LadyUpset;

    public GameObject ChildNeutral;
    public GameObject ChildHappy;
    public GameObject ChildUpset;

    public GameObject NerdNeutral;
    public GameObject NerdHappy;
    public GameObject NerdUpset;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // Prevent duplication
        }

    }

    void Start()
    {
        ghost = false;
        ghostChances = 3;
        humanChances = 3;
        correct = 0;

        characterName = "";

        // state 0 is home screen
        gameState = 0;
        
        // state 1 is dialogue
        // state 2 is choices
        // state 3 is lose
        // state 4 is win
    }



    void FixedUpdate()
    {
        Debug.Log(gameState);
    }
}
