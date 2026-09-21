using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    private Vector3 offset = new Vector3(0f, -0.3f, -10f);
    private Vector3 velocity = Vector3.zero;

    private float smoothTime = 0.25f;

    [SerializeField] private Transform target;

    //public Transform trans;

    //float xPos = 0f;
    //float yPos = 0f;
    float minX = -29.12f;
    float maxX = 28.99f;
    float minY = -1.76f;
    float maxY = 61f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 targetPosition = target.position + offset;

        //transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

        /*
        xPos = this.gameObject.GetComponent<Transform>().position.x;
        yPos = this.gameObject.GetComponent<Transform>().position.y;

        xPos = Mathf.Clamp(xPos, -29.12f, 28.99f);
        yPos = Mathf.Clamp(yPos, -1.76f, 61f);
        */

        float clampedX = Mathf.Clamp(smoothedPosition.x, minX, maxX);
        float clampedY = Mathf.Clamp(smoothedPosition.y, minY, maxY);

        this.gameObject.GetComponent<Transform>().position = new Vector3(clampedX, clampedY, smoothedPosition.z);
    }
}
