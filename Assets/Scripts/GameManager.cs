using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject StartLevelUI;
    public GameObject EndLevelUI;

    public void CompleteLevel()
    {
        StartCoroutine(LoadScene());
    }

    private IEnumerator LoadScene()
    {
        EndLevelUI.SetActive(true);
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("Intro");
    }

    public void Start()
    {
        StartCoroutine(WelcomeMsg());
    }

    private IEnumerator WelcomeMsg()
    {
        StartLevelUI.SetActive(true);
        yield return new WaitForSeconds(10);
        StartLevelUI.SetActive(false);
    }
}
