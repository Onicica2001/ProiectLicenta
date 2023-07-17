using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    public GenerateBuyList buyList;
    public Cart cart;
    public float moveSpeed = 5f, speedUp = 1.5f;
    public Rigidbody2D rb;
    Vector2 movement;
    Unit client;
    public GameObject endMenu; 
    public GameObject endPos;

    public GameObject[] players;
    private static int NUMBER_OF_POINTS = 25;
    void Start()
    {
        FindStartPos();
        cart.isWithPlayer = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseMenu.isPaused)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
            if (Input.GetKeyDown(KeyCode.E))
            {
                ObjectItem[] objects = FindObjectsOfType<ObjectItem>();
                ObjectItem item = objects[0];
                foreach (ObjectItem objectItem in objects)
                {
                    if (Vector2.Distance(transform.position, objectItem.gameObject.transform.position) < Vector2.Distance(transform.position, item.gameObject.transform.position))
                    {
                        item = objectItem;
                    }
                }
                if (Vector2.Distance(transform.position, item.gameObject.transform.position) <= 2f)
                {
                    if (buyList.buyList.Contains(item.itemName))
                    {
                        if (!cart.boughtList.Contains(item))
                        {
                            //Ar trebui modificat sa aiba si player un inventar in care sa tina iteme. Momentan se pun automat in cos
                            cart.AddItem(buyList, item);
                        }
                    }
                }

            }
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                if (cart.isWithPlayer)
                {
                    cart.PlayerLeft();
                }
                else
                {
                    if (Vector2.Distance(transform.position, cart.transform.position) <= 1f)
                    {
                        if (client != null)
                        {
                            if (!client.isStealing)
                            {
                                cart.PlayerIsBack();
                            }
                        }
                        else
                        {
                            cart.PlayerIsBack();
                        }
                    }
                }
            }
        }
    }

    void FindStartPos()
    {
        transform.position = GameObject.FindWithTag("Entrance").transform.position;
    }

    public void StealItem(string itemName)
    {
        cart.StealItem(itemName, buyList);
    }

    void FixedUpdate()
    {
        if (!PauseMenu.isPaused)
        {
            if (cart.isWithPlayer)
            {
                rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
                cart.transform.position = transform.position;
                cart.PlayerIsBack();
            }
            else
            {
                rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime * speedUp);
                cart.PlayerLeft();
            }
            Unit[] clientAIs = FindObjectsOfType<Unit>();
            if (clientAIs.Length > 0)
            {
                client = clientAIs[0];
                foreach (Unit clientAI in clientAIs)
                {
                    if (Vector2.Distance(cart.transform.position, clientAI.transform.position) < Vector2.Distance(cart.transform.position, client.transform.position))
                    {
                        client = clientAI;
                    }
                }
                if (Vector2.Distance(cart.transform.position, client.transform.position) <= 1f)
                {
                    if (client.isStealing)
                    {
                        if (Vector2.Distance(transform.position, client.transform.position) <= 2f)
                        {
                            client.intercepted = true;
                        }
                    }
                }
            }
            if (Vector2.Distance(transform.position,endPos.transform.position)<= 1f && buyList.buyList.Count == 0 && cart.isWithPlayer)
            {
                float score = PlayerPrefs.GetFloat("score");
                Debug.Log(score);
                score = score + NUMBER_OF_POINTS;
                Debug.Log(score);
                PlayerPrefs.SetFloat("score", score);
                PlayerPrefs.Save();
                endMenu.GetComponentInChildren<TextMeshProUGUI>().text = "You Won!";
                endMenu.SetActive(true);
                Time.timeScale = 0f;
                StartCoroutine(BackToSupermarket());
            } 
        }
    }

    private IEnumerator BackToSupermarket()
    {
        yield return new WaitForSecondsRealtime(5);
        LoadingPanel.player.SetActive(true);
        Time.timeScale = 1f;
        SceneManager.LoadScene("Supermarket");
    }
}
