using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Animator animator;
    public GameObject StartButton;
    public float sec = 0.5f;

    public void Start()
    {
        //StartCoroutine(BlinkStartButton());
    }

    public void OnClickStartButton()
    {
        SceneManager.LoadScene("Username");
    }

    IEnumerator BlinkStartButton()
    {
        while (true)
        {
            yield return new WaitForSeconds(sec * 2);
            StartButton.SetActive(false);
            yield return new WaitForSeconds(sec);
            StartButton.SetActive(true);
        }
    }
}
