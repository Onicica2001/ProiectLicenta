using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerObjectItem : MonoBehaviour
{
    public List<ObjectItem> spawnPoolItems;
    public List<GameObject> quads;
    private static float OVERLAP_BOX_SIDE_SIZE = .2f;
    private static int MINIMUM_NUMBER_OF_ITEMS = 4, MAXIMUM_NUMBER_OF_ITEMS = 15, MAXIMUM_NUMBER_OF_TRIALS = 10;
    // Start is called before the first frame update
    void Start()
    {
        spawnObjects();
    }

    public void spawnObjects()
    {
        destroyObjects();
        int i = 0;
        foreach (ObjectItem objectItem in spawnPoolItems)
        {
            MeshCollider quadCollider = quads[i].GetComponent<MeshCollider>();
            int noOfItems = Random.Range(MINIMUM_NUMBER_OF_ITEMS, MAXIMUM_NUMBER_OF_ITEMS);
            float screenX, screenY;
            Vector2 pos;
            for (int j = 0; j < noOfItems; j++)
            {
                int noOfTrials = 1;
                screenX = Random.Range(quadCollider.bounds.min.x, quadCollider.bounds.max.x);
                screenY = Random.Range(quadCollider.bounds.min.y, quadCollider.bounds.max.y);
                pos = new Vector2(screenX, screenY);
                while (Physics2D.OverlapBox(pos, new Vector2(OVERLAP_BOX_SIDE_SIZE, OVERLAP_BOX_SIDE_SIZE), 0))
                {
                    screenX = Random.Range(quadCollider.bounds.min.x, quadCollider.bounds.max.x);
                    screenY = Random.Range(quadCollider.bounds.min.y, quadCollider.bounds.max.y);
                    pos = new Vector2(screenX, screenY);
                    noOfTrials++;
                    if (noOfTrials > MAXIMUM_NUMBER_OF_TRIALS)
                        break;
                }
                Instantiate(objectItem, pos, objectItem.transform.rotation);
            }
            i++;
        }

    }

    private void destroyObjects()
    {
        foreach (ObjectItem obj in FindObjectsOfType<ObjectItem>())
        {
            Destroy(obj);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
