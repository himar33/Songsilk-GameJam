using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    /*public GameObject[] buttons;
    private GameObject lastSelected;*/
    private AudioSource aSource;
    public AudioSource bgMusic;

    // Start is called before the first frame update
    void Start()
    {
        aSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadGame()
    {
        StartCoroutine(AudioFadeOut.FadeOut(bgMusic, 0.01f));
        aSource.Play();
        SceneManager.LoadSceneAsync(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
