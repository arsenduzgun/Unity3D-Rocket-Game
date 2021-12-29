using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyMainAudio : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("BackgroundMusic");
        if(objects.Length > 1){
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
