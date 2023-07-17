using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveSystemFish : MonoBehaviour
{
    public GameObject correctForm;
    public string correctFormName;
    private GameObject spawner;
    private bool moving;
    public bool swimmingToRight, swimmingToLeft;
    private bool swimming;
    public bool finish;
    public int mismatch;
    private float vToRight = 2.5f, vToLeft = -2.5f;

    private float startPosX;
    private float startPosY;

    private Vector3 resetPosition;
    private static int MAXIMUM_NUMBER_OF_MISTAKES = 5, NUMBER_OF_POINTS = 15;
    // Start is called before the first frame update
    void Start()
    {
        resetPosition = this.transform.localPosition;
        spawner = GameObject.Find("SpawnerFish");
        mismatch = 0;
        swimming = true;
        if(spawner.GetComponent<SpawnerFish>().form.GetComponent<FishType>().fishName == correctFormName)
            {
                correctForm = spawner.GetComponent<SpawnerFish>().form;
            }
        else
        {
            correctForm = new GameObject();
        }
    }
    private void OnMouseDown()
    {
        if (!PauseMenu.isPaused)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePos;
                mousePos = Input.mousePosition;
                mousePos = Camera.main.ScreenToWorldPoint(mousePos);

                startPosX = mousePos.x - this.transform.localPosition.x;
                startPosY = mousePos.y - this.transform.localPosition.y;

                moving = true;
                swimming = false;
            }
        }
    }

    private void SwimRight()
    {
        float dt = Time.deltaTime; //seconds
        float dang = vToRight * dt;
        transform.Translate(new Vector2(dang, 0));
        resetPosition = this.transform.localPosition;
    }

    private void SwimLeft()
    {
        float dt = Time.deltaTime; //seconds
        float dang = vToLeft * dt;
        transform.Translate(new Vector2(dang, 0));
        resetPosition = this.transform.localPosition;
    }

    public void UpdateFishName()
    {
        GameObject[] list = GameObject.FindGameObjectsWithTag("Fish");
        foreach (var item in list)
        {
            if (spawner.GetComponent<SpawnerFish>().form.GetComponent<FishType>().fishName == item.GetComponent<MoveSystemFish>().correctFormName)
            {
                item.GetComponent<MoveSystemFish>().correctForm = spawner.GetComponent<SpawnerFish>().form;
            }
        }
    }

    private void OnMouseUp()
    {
        if (!PauseMenu.isPaused)
        {
            moving = false;

            if (Mathf.Abs(this.transform.localPosition.x - correctForm.transform.localPosition.x) <= 1.7f &&
                Mathf.Abs(this.transform.localPosition.y - correctForm.transform.localPosition.y) <= 1.7f)
            {
                this.transform.position = new Vector3(correctForm.transform.position.x, correctForm.transform.position.y, correctForm.transform.position.z);
                finish = true;
                this.gameObject.SetActive(false);
                spawner.GetComponent<SpawnerFish>().finished++;
                if (spawner.GetComponent<SpawnerFish>().finished != spawner.GetComponent<SpawnerFish>().totalFishCount)
                {
                    spawner.GetComponent<SpawnerFish>().GenerateFish();
                    UpdateFishName();
                }
            }
            else
            {
                swimming = true;
                this.transform.localPosition = new Vector3(resetPosition.x, resetPosition.y, resetPosition.z);
                spawner.GetComponent<SpawnerFish>().mismatch++;
                spawner.GetComponent<SpawnerFish>().MistakesCounter.text = "Mistakes: " + spawner.GetComponent<SpawnerFish>().mismatch + "/" + MAXIMUM_NUMBER_OF_MISTAKES;
            }
            if (spawner.GetComponent<SpawnerFish>().mismatch == MAXIMUM_NUMBER_OF_MISTAKES)
            {
                SceneManager.LoadScene("Lake");
                spawner.GetComponent<SpawnerFish>().player.SetActive(true);
                return;
            }
            if (spawner.GetComponent<SpawnerFish>().finished == spawner.GetComponent<SpawnerFish>().totalFishCount)
            {
                float score = PlayerPrefs.GetFloat("score");
                Debug.Log(score);
                score = score + NUMBER_OF_POINTS - spawner.GetComponent<SpawnerFish>().mismatch * 2;
                Debug.Log(score);
                PlayerPrefs.SetFloat("score", score);
                PlayerPrefs.Save();
                SceneManager.LoadScene("Lake");
                spawner.GetComponent<SpawnerFish>().player.SetActive(true);
                return;
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
                if (swimming)
                {
                    if (swimmingToLeft)
                    {
                        SwimLeft();
                    }
                    else if (swimmingToRight)
                    {
                        SwimRight();
                    }
                }
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
