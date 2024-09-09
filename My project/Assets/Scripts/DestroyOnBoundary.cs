using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnBoundary : MonoBehaviour
{
    public float boundaryX = -15.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < boundaryX)
        {
            Destroy(gameObject);
        }
    }
}
