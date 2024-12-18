using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToHomeScreen : MonoBehaviour
{
    public void BackToHome()
    {
        SceneManager.LoadScene("HomeScene");
    }
}
