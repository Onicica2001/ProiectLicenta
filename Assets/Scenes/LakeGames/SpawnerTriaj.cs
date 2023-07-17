using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnerTriaj : MonoBehaviour
{
    public Text MistakesCounter;
    public List<Item> spawnPool;
    public GameObject quadLeft, quadMiddle, quadRight;
    public int mismatch;
    public int finished;
    public GameObject player;
    public int wasteCount;
    private static int MINIMUM_NUMBER_OF_WASTE = 7, MAXIMUM_NUMBER_OF_WASTE = 25;
    private static float OVERLAP_BOX_SIDE_SIZE = 1f;
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
        Item toSpawn;
        MeshCollider quadLeftCollider = quadLeft.GetComponent<MeshCollider>();
        MeshCollider quadMiddleCollider = quadMiddle.GetComponent<MeshCollider>();
        MeshCollider quadRightColiider = quadRight.GetComponent<MeshCollider>();

        float screenX, screenY;
        Vector2 pos;

        wasteCount = Random.Range(MINIMUM_NUMBER_OF_WASTE, MAXIMUM_NUMBER_OF_WASTE);

        for (int i = 0; i < spawnPool.Count; i++)
        {
            spawnPool[i].GetComponent<MoveSystemTriaj>().correctForm = GameObject.FindWithTag(spawnPool[i].GetComponent<Item>().correctBin);
        }
        for (int i = 0; i < wasteCount; i++)
        {
            randomItem = Random.Range(0, spawnPool.Count);
            toSpawn = spawnPool[randomItem];
            int firstThirdWasteCount = wasteCount / 3, secondThirdWasteCount = (2 * wasteCount) / 3;
            if (i < firstThirdWasteCount)
            {
                screenX = Random.Range(quadLeftCollider.bounds.min.x, quadLeftCollider.bounds.max.x);
                screenY = Random.Range(quadLeftCollider.bounds.min.y, quadLeftCollider.bounds.max.y);
                pos = new Vector2(screenX, screenY);
                while (Physics2D.OverlapBox(pos, new Vector2(OVERLAP_BOX_SIDE_SIZE, OVERLAP_BOX_SIDE_SIZE), 0))
                {
                    screenX = Random.Range(quadLeftCollider.bounds.min.x, quadLeftCollider.bounds.max.x);
                    screenY = Random.Range(quadLeftCollider.bounds.min.y, quadLeftCollider.bounds.max.y);
                    pos = new Vector2(screenX, screenY);
                }
                Instantiate(toSpawn, pos, toSpawn.transform.rotation);
            }else if (i < secondThirdWasteCount)
            {
                screenX = Random.Range(quadMiddleCollider.bounds.min.x, quadMiddleCollider.bounds.max.x);
                screenY = Random.Range(quadMiddleCollider.bounds.min.y, quadMiddleCollider.bounds.max.y);
                pos = new Vector2(screenX, screenY);
                while (Physics2D.OverlapBox(pos, new Vector2(OVERLAP_BOX_SIDE_SIZE, OVERLAP_BOX_SIDE_SIZE), 0))
                {
                    screenX = Random.Range(quadMiddleCollider.bounds.min.x, quadMiddleCollider.bounds.max.x);
                    screenY = Random.Range(quadMiddleCollider.bounds.min.y, quadMiddleCollider.bounds.max.y);
                    pos = new Vector2(screenX, screenY);
                }
                Instantiate(toSpawn, pos, toSpawn.transform.rotation);
            }
            else
            {
                screenX = Random.Range(quadRightColiider.bounds.min.x, quadRightColiider.bounds.max.x);
                screenY = Random.Range(quadRightColiider.bounds.min.y, quadRightColiider.bounds.max.y);
                pos = new Vector2(screenX, screenY);
                while (Physics2D.OverlapBox(pos, new Vector2(OVERLAP_BOX_SIDE_SIZE, OVERLAP_BOX_SIDE_SIZE), 0))
                {
                    screenX = Random.Range(quadRightColiider.bounds.min.x, quadRightColiider.bounds.max.x);
                    screenY = Random.Range(quadRightColiider.bounds.min.y, quadRightColiider.bounds.max.y);
                    pos = new Vector2(screenX, screenY);
                }
                Instantiate(toSpawn, pos, toSpawn.transform.rotation);
            }
            
        }

    }

    private void destroyObjects()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Item"))
        {
            Destroy(obj);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
