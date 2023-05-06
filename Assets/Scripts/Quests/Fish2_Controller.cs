using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish2_Controller : MonoBehaviour
{
    [SerializeField] GameObject _fish2;
    [SerializeField] GameObject _final;
    private Animator _animFish;
    void Start()
    {
        _animFish = _fish2.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SecondFish()
    {
        _animFish.Play("FishMove2");
        _final.SetActive(true);
    }
}
