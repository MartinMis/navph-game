using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DisplayCredits : MonoBehaviour
{
    public void ShowCredits()
    {
        Debug.Log("Showing credits");
        SceneManager.LoadScene("Credits");
    }
}
