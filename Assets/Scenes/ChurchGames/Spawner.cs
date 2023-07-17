using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    public Text MistakesCounter;
    public List<Porunci> spawnPool;
    private List<Porunci> usingSpawnPool;
    public List<PlaceHolder> placeHolders;
    public GameObject quad;
    public int mismatch;
    public int finished;
    public GameObject player;
    private static int NUMBER_OF_COMMANDMENTS=10;
    private static float OVERLAP_BOX_SIDE_SIZE = 1.5f;
    private static int MAXIMUM_NUMBER_OF_MISTAKES = 5;
    // Start is called before the first frame update
    void Start()
    {
        spawnObjects();
        mismatch = 0;
        finished = 0;
        player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            player.SetActive(false);
        }
        MistakesCounter.text = "Mistakes: " + mismatch + "/" + MAXIMUM_NUMBER_OF_MISTAKES;
    }

    public void spawnObjects()
    {
        destroyObjects();
        int randomItem = 0;
        Porunci toSpawn;
        MeshCollider quadCollider = quad.GetComponent<MeshCollider>();

        float screenX, screenY;
        Vector2 pos;

        for (int i = 0; i < NUMBER_OF_COMMANDMENTS; i++)
        {
            spawnPool[i].GetComponent<MoveSystem>().correctForm = placeHolders[i].gameObject;
        }

        usingSpawnPool = spawnPool;
        for (int i = 0; i < NUMBER_OF_COMMANDMENTS; i++)
        {
            randomItem = Random.Range(0, usingSpawnPool.Count);
            toSpawn = usingSpawnPool[randomItem];
            screenX = Random.Range(quadCollider.bounds.min.x, quadCollider.bounds.max.x);
            screenY = Random.Range(quadCollider.bounds.min.y, quadCollider.bounds.max.y);
            pos = new Vector2(screenX, screenY);
            while (Physics2D.OverlapBox(pos, new Vector2(OVERLAP_BOX_SIDE_SIZE, OVERLAP_BOX_SIDE_SIZE), 0))
            {
                screenX = Random.Range(quadCollider.bounds.min.x, quadCollider.bounds.max.x);
                screenY = Random.Range(quadCollider.bounds.min.y, quadCollider.bounds.max.y);
                pos = new Vector2(screenX, screenY);
            }
            Instantiate(toSpawn, pos, toSpawn.transform.rotation);
            usingSpawnPool.Remove(toSpawn);
        }

    }

    private void destroyObjects()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Porunca")) 
        {
            Destroy(obj);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
