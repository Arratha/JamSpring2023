using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITalkable : MonoBehaviour
{
    [SerializeField] GameObject replicRef;
    private bool startDilogue = true;
    [SerializeField] string[] replics;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        replicRef.GetComponent<SpriteRenderer>().enabled = !replicRef.GetComponent<SpriteRenderer>().enabled;

        if (startDilogue)
        {

        }
        else
        {

        }
    }
}
