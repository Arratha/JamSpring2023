using System.Collections;

using UnityEngine;

using Drop;


public class StartGameScript : MonoBehaviour
{
    [SerializeField] private Drop_Controller _drop;

    private void Awake()
    {
        if (_drop != null)
            _drop.SetControl(false);

        StartCoroutine(StartGame());
    }

    private IEnumerator StartGame()
    {
        yield return new WaitForSeconds(5f);

        gameObject.SetActive(false);

        if (_drop != null)
            _drop.SetControl(true);
    }
}
