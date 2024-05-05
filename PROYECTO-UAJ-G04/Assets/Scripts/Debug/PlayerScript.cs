using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(0, 1, 0);
        }
        else
            if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(0, -1, 0);

        }
    }
    private void FixedUpdate()
    {
        Debug.DrawRay(transform.position, transform.forward*100,Color.blue);
        Debug.DrawRay(transform.position, transform.right*100,Color.red);
    }
}
