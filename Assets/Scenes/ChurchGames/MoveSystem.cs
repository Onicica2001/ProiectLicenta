using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveSystem : MonoBehaviour
{
    public GameObject correctForm;
    private GameObject spawner;
    private bool moving;
    public bool finish;

    private float startPosX;
    private float startPosY;

    private Vector3 resetPosition;
    private static int MAXIMUM_NUMBER_OF_MISTAKES = 5, NUMBER_OF_COMMANDMENTS = 10, NUMBER_OF_POINTS = 10;

    // Start is called before the first frame update
    void Start()
    {
        resetPosition = this.transform.localPosition;
        spawner = GameObject.Find("Spawner");
    }

    private void OnMouseDown()
    {
        if (!PauseMenu.isPaused)
        {
            if (Input.GetMouseButtonDown(0) && !finish)
            {
                Vector3 mousePos;
                mousePos = Input.mousePosition;
                mousePos = Camera.main.ScreenToWorldPoint(mousePos);

                startPosX = mousePos.x - this.transform.localPosition.x;
                startPosY = mousePos.y - this.transform.localPosition.y;

                moving = true;
            }
        }
    }

    private void OnMouseUp()
    {
        if (!PauseMenu.isPaused)
        {
            if (moving)
            {
                moving = false;
                if (Mathf.Abs(this.transform.localPosition.x - correctForm.transform.localPosition.x) <= 0.5f &&
                    Mathf.Abs(this.transform.localPosition.y - correctForm.transform.localPosition.y) <= 0.5f)
                {
                    this.transform.position = new Vector3(correctForm.transform.position.x, correctForm.transform.position.y, correctForm.transform.position.z);
                    finish = true;
                    spawner.GetComponent<Spawner>().finished++;
                }
                else
                {
                    this.transform.localPosition = new Vector3(resetPosition.x, resetPosition.y, resetPosition.z);
                    spawner.GetComponent<Spawner>().mismatch++;
                    spawner.GetComponent<Spawner>().MistakesCounter.text = "Mistakes: " + spawner.GetComponent<Spawner>().mismatch + "/" + MAXIMUM_NUMBER_OF_MISTAKES;
                }
                if (spawner.GetComponent<Spawner>().mismatch == MAXIMUM_NUMBER_OF_MISTAKES)
                {
                    SceneManager.LoadScene("Church");
                    spawner.GetComponent<Spawner>().player.SetActive(true);
                    return;
                }
                if (spawner.GetComponent<Spawner>().finished == NUMBER_OF_COMMANDMENTS)
                {
                    float score = PlayerPrefs.GetFloat("score");
                    Debug.Log(score);
                    score = score + NUMBER_OF_POINTS - spawner.GetComponent<Spawner>().mismatch * 2;
                    Debug.Log(score);
                    PlayerPrefs.SetFloat("score", score);
                    PlayerPrefs.Save();
                    SceneManager.LoadScene("Church");
                    spawner.GetComponent<Spawner>().player.SetActive(true);
                    return;
                }
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseMenu.isPaused)
        {
            if (!finish)
            {
                if (moving)
                {
                    Vector3 mousePos;
                    mousePos = Input.mousePosition;
                    mousePos = Camera.main.ScreenToWorldPoint(mousePos);

                    this.gameObject.transform.localPosition = new Vector3(mousePos.x - startPosX, mousePos.y - startPosY, this.gameObject.transform.localPosition.z);
                }
            }
        }
    }
}
