using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.T))
        {
            transform.Rotate(-50 * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.G))
        {
            transform.Rotate(50 * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.H))
        {
            transform.Rotate( 0,50 * Time.deltaTime,0,Space.World);
        }
        if (Input.GetKey(KeyCode.F))
        {
            transform.Rotate(0,-50 * Time.deltaTime,0,Space.World);
        }
    }
}
