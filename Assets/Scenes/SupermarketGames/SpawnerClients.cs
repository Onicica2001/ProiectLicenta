using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerClients : MonoBehaviour
{
    public List<GameObject> spawnPoolClients;
    private List<Unit> clients;
    public GameObject quad;
    public List<string> spawnPoolItems;
    public GameObject endPos;
    public string item;
    public GameObject QuestionMenu;
    float timer2;
    public float delay2 = .1f;
    public static int MAX_NUMBER_OF_CLIENTS_ONCE = 5;

    // Start is called before the first frame update
    void Start()
    {
        clients = new List<Unit>();
        destroyObjects();
        timer2 = 0;
        //StopAllCoroutines();
        //spawnClients();
    }

    private string GenerateItem()
    {
        int pos = Random.Range(0, spawnPoolItems.Count);
        string line = spawnPoolItems[pos];
        return line;
    }

    IEnumerator SpawnerClient(MeshCollider c)
    {
        GameObject client;
        int randomItem = 0;
        randomItem = Random.Range(0, spawnPoolClients.Count);
        client = spawnPoolClients[randomItem];
        float screenX, screenY;
        Vector2 pos;
        screenX = Random.Range(c.bounds.min.x, c.bounds.max.x);
        screenY = Random.Range(c.bounds.min.y, c.bounds.max.y);
        pos = new Vector2(screenX, screenY);
        while (Physics2D.OverlapBox(pos, new Vector2(.2f, .2f), 0))
        {
            screenX = Random.Range(c.bounds.min.x, c.bounds.max.x);
            screenY = Random.Range(c.bounds.min.y, c.bounds.max.y);
            pos = new Vector2(screenX, screenY);
        }
        client.GetComponent<Unit>().QuestionMenu = QuestionMenu;
        client.GetComponent<Unit>().endPosition = endPos.transform.position;
        client.GetComponent<Unit>().item = GenerateItem();
        GameObject game = Instantiate(client, pos, client.transform.rotation);
        Unit unit = game.GetComponent<Unit>();
        clients.Add(unit);
        yield return null;
    }

    public void spawnClients()
    {
        //destroyObjects();
        MeshCollider quadCollider = quad.GetComponent<MeshCollider>();
        StartCoroutine(SpawnerClient(quadCollider));
    }

    private void destroyObjects()
    {
        foreach (Unit obj in FindObjectsOfType<Unit>())
        {
            Destroy(obj);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (clients.Count < MAX_NUMBER_OF_CLIENTS_ONCE)
        {
            spawnClients();
            
        }
        else
        {
            timer2 += Time.deltaTime;
            if (timer2 > delay2)
            {
                timer2 -= delay2;
                List<Unit> clientsToBeDeleted = new List<Unit>();
                foreach (Unit unit in clients)
                {
                    if (unit.startPosition == unit.lastPosition)
                    {
                        clientsToBeDeleted.Add(unit);
                    }
                }
                foreach (Unit unit1 in clientsToBeDeleted)
                {
                    clients.Remove(unit1);
                    unit1.StopAllCoroutines();
                    Destroy(unit1.gameObject);
                    Destroy(unit1);
                }
            }
            foreach (Unit unit in clients)
            {
                if (Vector2.Distance(unit.transform.position, unit.endPosition) <= 1f && unit.target == null)
                {
                    unit.StopAllCoroutines();
                    clients.Remove(unit);
                    Destroy(unit.gameObject);
                    Destroy(unit);
                    break;
                }
            }
        }
        
    }
}
