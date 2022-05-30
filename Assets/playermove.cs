using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playermove : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        float speed = 0.0625f;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            pos.z += speed;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            pos.z -= speed;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            pos.x -= speed;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            pos.x += speed;
        }
        transform.position = pos;
    }
}
