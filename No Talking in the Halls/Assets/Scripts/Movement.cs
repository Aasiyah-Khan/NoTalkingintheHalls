using UnityEngine;

public class movement : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;
    float moveSpeed = 5f;

    public Camera mainCam;

   // float outsideX;
    //float outsideY;

    //public Canvas Canvas;
    
    
     public Vector2 MoveInput { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // start inside
       //mainCam.GetComponent<Transform>().position = new Vector3(25, 1.5f, -10);
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        
       
        
        
    }  
    // Update is called once per frame
    void FixedUpdate()
    {
        //Canvas.GetComponent<RectTransform>().position = new Vector3(rb.position.x, rb.position.y + 2.3f, -4f);
        
        Vector2 movement = MoveInput * moveSpeed;
        rb.linearVelocity = movement;

    if (MoveInput.x > 0) // Moving right
    {
       // transform.localScale = new Vector3(3.5f, 3.5f, 1); 
    }
    else if (MoveInput.x < 0) // Moving left
    {
       // transform.localScale = new Vector3(-3.5f, 3.5f, 1);
    }


        //animator.SetFloat("speed", Mathf.Abs(MoveInput.x));
        //animator.SetFloat("speedy", Mathf.Abs(MoveInput.y));

        
        
        
    }

    public void OnMove(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>();
        //animator.SetFloat("speed",);
    }

    //void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    //{
        
   // }
}

