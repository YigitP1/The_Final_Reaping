using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Playables;
using UnityEngine.SceneManagement;

public class video : MonoBehaviour
{
    float totalTime = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        totalTime += Time.deltaTime;

        if (totalTime > 37f)
        {
            SceneManager.LoadScene("Stage_1");
        }
    }
}