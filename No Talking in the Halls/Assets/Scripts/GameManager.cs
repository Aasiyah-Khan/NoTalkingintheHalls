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

    public string characterName;


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
        ghostChances = 3;
        humanChances = 3;

        characterName = "name";
    }



    void FixedUpdate()
    {
        Debug.Log(characterName);
    }
}
