using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind_Controller : MonoBehaviour
{
    [SerializeField] GameObject _wind;
    private Animator _animWind;
    void Start()
    {
        _animWind = _wind.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void StartWind()
    {
        _wind.GetComponent<SpriteRenderer>().enabled = !_wind.GetComponent<SpriteRenderer>().enabled;
        _animWind.Play("Start");
    }
}
