using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnerFish : MonoBehaviour
{
    public Text MistakesCounter;
    public List<Fish> spawnPool;
    public GameObject form;
    [SerializeField] TextAsset possibleFish;
    public GameObject quadLeft, quadRight;
    public int mismatch;
    public int finished;
    public GameObject player;
    public int fishCount, totalFishCount = 5, countOfFishToBeSpawnedPerRound = 3;
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
        form.GetComponent<FishType>().fishName = GenerateWord();
    }
    private string GenerateWord()
    {
        string[] wordList = possibleFish.text.Split("\n");
        int pos = Random.Range(0, wordList.Length - 1);
        string line = wordList[pos];
        return line.Substring(0, line.Length - 1);
    }

    public void GenerateFish()
    {
        form.GetComponent<FishType>().fishName = GenerateWord();
    }

    public void spawnObjects()
    {
        destroyObjects();
        
        for (int i = 0; i < spawnPool.Count; i++)
        {
            spawnPool[i].GetComponent<MoveSystemFish>().correctFormName = spawnPool[i].GetComponent<Fish>().correctFish;
        }
        int rand = Random.Range(countOfFishToBeSpawnedPerRound, totalFishCount);
        spawnFish(rand);
    }

    private void spawnFish(int max)
    {
        int randomItem = 0, minGeneratedFishCount = 2;

        MeshCollider quadLeftCollider = quadLeft.GetComponent<MeshCollider>();
        MeshCollider quadRightCollider = quadRight.GetComponent<MeshCollider>();

        Fish toSpawn;
        float screenX, screenY;
        Vector2 pos;
        fishCount = Random.Range(minGeneratedFishCount, max);
        for (int i = 0; i < fishCount; i++)
        {
            randomItem = Random.Range(0, spawnPool.Count);
            toSpawn = spawnPool[randomItem];
            int mediumFishCount = fishCount / 2 + 1;
            if (i < mediumFishCount)
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
                toSpawn.GetComponent<MoveSystemFish>().swimmingToLeft = false;
                toSpawn.GetComponent<MoveSystemFish>().swimmingToRight = true;
                toSpawn.GetComponentInChildren<SpriteRenderer>().sprite = toSpawn.GetComponent<Fish>().ImageFaceRight;
                Instantiate(toSpawn, pos, toSpawn.transform.rotation);
            }
            else
            {
                screenX = Random.Range(quadRightCollider.bounds.min.x, quadRightCollider.bounds.max.x);
                screenY = Random.Range(quadRightCollider.bounds.min.y, quadRightCollider.bounds.max.y);
                pos = new Vector2(screenX, screenY);
                while (Physics2D.OverlapBox(pos, new Vector2(OVERLAP_BOX_SIDE_SIZE, OVERLAP_BOX_SIDE_SIZE), 0))
                {
                    screenX = Random.Range(quadRightCollider.bounds.min.x, quadRightCollider.bounds.max.x);
                    screenY = Random.Range(quadRightCollider.bounds.min.y, quadRightCollider.bounds.max.y);
                    pos = new Vector2(screenX, screenY);
                }
                toSpawn.GetComponent<MoveSystemFish>().swimmingToLeft = true;
                toSpawn.GetComponent<MoveSystemFish>().swimmingToRight = false;
                toSpawn.GetComponentInChildren<SpriteRenderer>().sprite = toSpawn.GetComponent<Fish>().ImageFaceLeft;
                Instantiate(toSpawn, pos, toSpawn.transform.rotation * Quaternion.Euler(0, 180, 0));
            }

        }
    }

    private void destroyObjects()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Fish"))
        {
            Destroy(obj);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseMenu.isPaused)
        {
            if (finished < totalFishCount && GameObject.FindGameObjectsWithTag("Fish").Length == 0)
            {
                int rand = Random.Range(3, totalFishCount);
                spawnFish(rand);
            }
        }
    }
}
