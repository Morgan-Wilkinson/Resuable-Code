using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    //This is a reference to  the Rigidbody component called "rb"
    public Rigidbody rb;
    
    public float forwardForce = 2000f;
    public float sidewaysForce = 50f;

    void FixedUpdate ()
    {
        rb.AddForce(0,0,forwardForce * Time.deltaTime);

        foreach (Touch touch in Input.touches)
        {
            if (touch.position.x < Screen.width / 2)
            {
                rb.AddForce(-sidewaysForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
            }
            else if (touch.position.x > Screen.width / 2)
            {
                rb.AddForce(sidewaysForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
            }
        }

        if (rb.position.y < -1f)
        {
            FindObjectOfType<GameManager>().EndGame();
        }

        if (rb.position.y > 5f)
        {
            FindObjectOfType<GameManager>().EndGame();
        }
    }
}
