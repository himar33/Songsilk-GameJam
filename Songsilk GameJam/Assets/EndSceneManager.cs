using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class EndSceneManager : MonoBehaviour
{
    public TMP_Text pressE;
    public float timer = 5.0f;
    bool canExit = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0.0f)
        {
            canExit = true;
            pressE.gameObject.active = true;
        }

        if (canExit)
        {
            if (Input.GetButtonDown("Exit"))
            {
                GetComponent<AudioSource>().Play();
                SceneManager.LoadSceneAsync(0);
            }
        }
    }
}
