using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public AudioSource audioPlayer;
    public AudioClip Boton;

    public void BotonStart()
    {
        SceneManager.LoadScene("Game");
    }
    public void BotonSalir()
    {
        Application.Quit();
    }
}