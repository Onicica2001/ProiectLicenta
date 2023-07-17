using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public int iSceneToLoad;
    public string sSceneToLoad;

    public bool useIntegerToLoadScene = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collisionGameObject = collision.gameObject;

        if(collisionGameObject.name == "Player")
        {
            LoadScene();
        }
    }

    public void LoadScene()
    {
        if (useIntegerToLoadScene)
        {
            SceneManager.LoadScene(iSceneToLoad);
        }
        else
        {
            SceneManager.LoadScene(sSceneToLoad);
        }
    }
}
