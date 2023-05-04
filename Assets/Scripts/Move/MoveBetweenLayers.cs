using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveB : MonoBehaviour
{
    int playerObject, collideObject;
    void Start()
    {
        playerObject = LayerMask.NameToLayer("Drop");
        collideObject = LayerMask.NameToLayer("Level 1");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
