using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AutorManager : MonoBehaviour
{
    public Text MistakesCounter;
    public Text QuestionCounter;
    public List<string> spawnPoolQuestions, spawnPoolAutors;
    public List<string> possibleQuestions, possibleAutors;
    public int mismatch, questionNumber;
    public GameObject player;
    public TextMeshProUGUI Question;
    public List<GameObject> buttons;
    public string correctAutor;
    private static int NUMBER_OF_POINTS = 10, NUMBER_OF_QUESTIONS = 5, MAXIMUM_NUMBER_OF_MISTAKES = 3;

    void Start()
    {
        mismatch = 0;
        questionNumber = 0;
        player = GameObject.FindWithTag("Player"); 
        if (player != null)
        {
            player.SetActive(false);
        }
        MistakesCounter.text = "Mistakes: " + mismatch + "/" + MAXIMUM_NUMBER_OF_MISTAKES;
        QuestionCounter.text = "Question: " + questionNumber + "/" + NUMBER_OF_QUESTIONS;
        GenerateSpawnPoolQuestions();
        GenerateButtons();
        InitializeButtons();
    }

    private void InitializeButtons()
    {
        buttons[0].GetComponentInChildren<TextMeshProUGUI>().text = "";
        buttons[1].GetComponentInChildren<TextMeshProUGUI>().text = "";
        buttons[2].GetComponentInChildren<TextMeshProUGUI>().text = "";
        correctAutor = "";
        GenerateQuestion();
        GenerateAnswers();
    }

    private void GenerateSpawnPoolQuestions()
    {
        spawnPoolQuestions = possibleQuestions;
    }

    private void GenerateSpawnPoolAutors()
    {
        spawnPoolAutors = possibleAutors;
    }

    private void GenerateQuestion()
    {
        int pos = Random.Range(0, spawnPoolQuestions.Count);
        string line = spawnPoolQuestions[pos];
        spawnPoolQuestions.Remove(line);
        string[] word = line.Split(";");
        Question.text = "Cine a scris " + word[0] + "?";
        correctAutor = word[1];
        pos = Random.Range(0, 3);
        buttons[pos].GetComponentInChildren<TextMeshProUGUI>().text = correctAutor;
    }

    private void GenerateAnswers()
    {
        GenerateSpawnPoolAutors();
        while (spawnPoolAutors.Contains(correctAutor))
            spawnPoolAutors.Remove(correctAutor);
        int numberOfChoicesLeft = 2;
        for (int i = 0; i < numberOfChoicesLeft; i++)
        {
            int pos = Random.Range(0, spawnPoolAutors.Count), numberOfButtons = 3;
            string line = spawnPoolAutors[pos];
            spawnPoolAutors.Remove(line);
            pos = Random.Range(0, numberOfButtons);
            while (buttons[pos].GetComponentInChildren<TextMeshProUGUI>().text != "")
            {
                pos = Random.Range(0, numberOfButtons);
            }
            buttons[pos].GetComponentInChildren<TextMeshProUGUI>().text = line;
        }
        
    }

    private void GenerateButtons()
    {
        buttons[0].GetComponent<Button>().onClick.AddListener(delegate { CheckAnswer(buttons[0]); });
        buttons[1].GetComponent<Button>().onClick.AddListener(delegate { CheckAnswer(buttons[1]); });
        buttons[2].GetComponent<Button>().onClick.AddListener(delegate { CheckAnswer(buttons[2]); });
    }

    private void CheckAnswer(GameObject button)
    {
        if (button.GetComponentInChildren<TextMeshProUGUI>().text == correctAutor)
        {
            if (questionNumber == NUMBER_OF_QUESTIONS - 1)
            {
                float score = PlayerPrefs.GetFloat("score");
                Debug.Log(score);
                score = score + NUMBER_OF_POINTS - mismatch * 2;
                Debug.Log(score);
                PlayerPrefs.SetFloat("score", score);
                PlayerPrefs.Save();
                SceneManager.LoadScene("Library");
                player.SetActive(true);
            }
            else
            {
                questionNumber++;
                QuestionCounter.text = "Question: " + questionNumber + "/" + NUMBER_OF_QUESTIONS;
                InitializeButtons();
            }
        }
        else
        {
            if (questionNumber == NUMBER_OF_QUESTIONS - 1 && mismatch != MAXIMUM_NUMBER_OF_MISTAKES - 1)
            {
                float score = PlayerPrefs.GetFloat("score");
                Debug.Log(score);
                score = score + (NUMBER_OF_POINTS - mismatch * 2);
                Debug.Log(score);
                PlayerPrefs.SetFloat("score", score);
                PlayerPrefs.Save();
                SceneManager.LoadScene("Library");
                player.SetActive(true);
            }else if (mismatch == MAXIMUM_NUMBER_OF_MISTAKES - 1)
            {
                SceneManager.LoadScene("Library");
                player.SetActive(true);
            }
            else
            {
                mismatch++;
                questionNumber++;
                MistakesCounter.text = "Mistakes: " + mismatch + "/" + MAXIMUM_NUMBER_OF_MISTAKES;
                QuestionCounter.text = "Question: " + questionNumber + "/" + NUMBER_OF_QUESTIONS;
                InitializeButtons();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
