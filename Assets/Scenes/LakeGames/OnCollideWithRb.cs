using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollideWithRb : MonoBehaviour
{
    private Collider col;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Fish")
        {
            col = gameObject.GetComponent<Collider>();
            Physics.IgnoreCollision(col, collision.collider);
        }
    }
}
