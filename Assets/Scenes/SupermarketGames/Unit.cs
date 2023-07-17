using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
    private enum State
    {
        GoAfterItem,
        GoBackToStart
    }
    public PathRequestManager pathRequestManager;
    public bool intercepted, isStealing, takesItem;
    public PlayerScript player;
    public List<string> spawnPoolQuestions;
    [SerializeField] TextAsset possibleQuestions;
    public GameObject Question;
    public List<GameObject> buttons;
    public string correctAnswer;
    public GameObject QuestionMenu;
    private State state;
    public GameObject targetObject;
    public ObjectItem target;
    public Vector3 endPosition, lastPosition, startPosition;
    private Vector3 targetPosition;
    public string item;
    float speed = 2f;
    Vector3[] path;
    int targetIndex;
    const float pathUpdateMoveThreshold = .5f;

    private void Awake()
    {
        state = State.GoAfterItem;
        intercepted = false;
        isStealing = false;
        takesItem = false;
        player = FindObjectOfType<PlayerScript>();
        pathRequestManager = FindObjectOfType<PathRequestManager>();
    }

    private void GenerateSpawnPoolQuestions()
    {
        string[] wordList = possibleQuestions.text.Split("\n");
        for (int i = 0; i < wordList.Length - 1; i++)
        {
            spawnPoolQuestions.Add(wordList[i]);
        }

    }

    private void InitializeButtons()
    {
        buttons[0].GetComponentInChildren<TextMeshProUGUI>().text = "";
        buttons[1].GetComponentInChildren<TextMeshProUGUI>().text = "";
        correctAnswer = "";
        GenerateQuestion();
    }

    private void GenerateQuestion()
    {
        int pos = Random.Range(0, spawnPoolQuestions.Count);
        string line = spawnPoolQuestions[pos];
        spawnPoolQuestions.Remove(line);
        string[] word = line.Split(";");
        Question.GetComponent<TextMeshProUGUI>().text = word[0];
        correctAnswer = word[1];
        pos = Random.Range(0, 2);
        buttons[pos].GetComponentInChildren<TextMeshProUGUI>().text = correctAnswer;
        if (correctAnswer.Length>5)
        {
            for (int i = 0; i < 2; i++)
            {
                if (buttons[i].GetComponentInChildren<TextMeshProUGUI>().text == "")
                {
                    buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = "Fals";
                }
            }
        }
        else
        {
            for (int i = 0; i < 2; i++)
            {
                if (buttons[i].GetComponentInChildren<TextMeshProUGUI>().text == "")
                {
                    buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = "Adevarat";
                }
            }
        }
    }

    private void GenerateButtons()
    {
        buttons[0].GetComponent<Button>().onClick.AddListener(delegate { CheckAnswer(buttons[0]); });
        buttons[1].GetComponent<Button>().onClick.AddListener(delegate { CheckAnswer(buttons[1]); });
    }

    private void CheckAnswer(GameObject button)
    {
        if (button.GetComponentInChildren<TextMeshProUGUI>().text == correctAnswer)
        {
            StartCoroutine(RightAnswer());
        }
        else
        {
            StartCoroutine(WrongAnswer());
        }
    }

    private IEnumerator WrongAnswer()
    {
        yield return new WaitForSecondsRealtime(5);
        player.StealItem(target.itemName);
        Destroy(target.gameObject);
        this.state = State.GoBackToStart;
        QuestionMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    private IEnumerator RightAnswer()
    {
        yield return new WaitForSecondsRealtime(5);
        player.cart.PlayerIsBack();
        QuestionMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Duel()
    {
        QuestionMenu.SetActive(true);
        GenerateSpawnPoolQuestions();
        GenerateButtons();
        InitializeButtons();
        Time.timeScale = 0f;
    }

    public void SetStateGoBackToStart()
    {
        if (target != null)
        {
            if (player.cart.boughtList.Contains(target))
            {
                if (!intercepted)
                {
                    player.StealItem(target.itemName);
                    Destroy(target.gameObject);
                    this.state = State.GoBackToStart;
                }
                else
                {
                    Duel();
                }
            }
            else
            {
                ObjectItem[] objectItems = FindObjectsOfType<ObjectItem>();
                List<ObjectItem> items = new List<ObjectItem>();
                foreach (ObjectItem objectItem in objectItems)
                {
                    if (objectItem.itemName == item)
                    {
                        items.Add(objectItem);
                    }
                }
                if (items.Count == 0)
                {
                    this.state = State.GoBackToStart;
                }
                else
                {
                    this.state = State.GoBackToStart;
                    items.Remove(target);
                    Destroy(target.gameObject);
                }
            }
        }
        else
        {
            this.state = State.GoBackToStart;
        }
    }

    private void Update()
    {
        switch (state)
        {
            default:
            case State.GoAfterItem:
                ObjectItem oldTarget = target;
                target = GetTarget();
                if (target == null)
                {
                    StopCoroutine("UpdatePath");
                    state = State.GoBackToStart;
                    break;
                }
                if (oldTarget != target || targetPosition != target.transform.position)
                {
                    targetObject = target.gameObject;
                    targetPosition = targetObject.transform.position;
                    StopCoroutine("UpdatePath");
                    StartCoroutine("UpdatePath");
                }
                if (Vector2.Distance(transform.position, targetObject.transform.position) <= 1f)
                {
                    if (player.cart.boughtList.Contains(target))
                    {
                        isStealing = true;
                    }
                    takesItem = true;
                    Invoke(nameof(SetStateGoBackToStart), 5);
                }
                else
                {
                    isStealing = false;
                }
                if ((Mathf.Abs(lastPosition.x - transform.position.x) < 0.000001) && (Mathf.Abs(lastPosition.y - transform.position.y) < 0.000001))
                {
                    if (!takesItem)
                    {
                        StopCoroutine("UpdatePath");
                        StartCoroutine("UpdatePath");
                    }
                }
                lastPosition = transform.position;
                break;
            case State.GoBackToStart:
                takesItem = false;
                PathRequestManager.RequestPath(new PathRequest(transform.position, endPosition, OnPathFound));
                break;
        }
    }

    private ObjectItem GetTarget()
    {
        ObjectItem[] objectItems = FindObjectsOfType<ObjectItem>(false);
        List<ObjectItem> items = new List<ObjectItem>();
        foreach (ObjectItem objectItem in objectItems)
        {
            if (objectItem.itemName == item)
            {
                items.Add(objectItem);
            }
        }
        if (items.Count == 0)
        {
            return null;
        }
        target = items[0];
        foreach (ObjectItem obj in items)
        {
            if (Vector3.Distance(obj.gameObject.transform.position, transform.position) < Vector3.Distance(target.gameObject.transform.position, transform.position))
            {
                return obj;
            }
        }
        return target;
    }
    private void Start()
    {
        target = GetTarget();
        lastPosition = transform.position;
        startPosition = transform.position;
        if (target != null)
        {
            targetObject = target.gameObject;
            targetPosition = target.transform.position;
            PathRequestManager.RequestPath(new PathRequest(transform.position, targetObject.transform.position, OnPathFound));
            TextMeshProUGUI[] texts = QuestionMenu.GetComponentsInChildren<TextMeshProUGUI>(true);
            foreach(TextMeshProUGUI text in texts)
            {
                if (text.gameObject.CompareTag("Question"))
                {
                    Question = text.gameObject;
                }
            }
            Button[] buttons2 = QuestionMenu.GetComponentsInChildren<Button>(true);
            foreach (Button button in buttons2)
            {
                buttons.Add(button.gameObject);
            }
            QuestionMenu.SetActive(false);
        }
        else
        {
            Debug.Log("Target null");
        }
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful && this!=null)
        {
            targetIndex = 0;
            path = newPath;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator UpdatePath()
    {
        Vector3 targetPosOld = target.transform.position;
        PathRequestManager.RequestPath(new PathRequest(transform.position, targetPosition, OnPathFound));
        float sqrMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;
        while (true)
        {
            yield return null;
            if (target != null)
            {
                if ((target.transform.position - targetPosOld).sqrMagnitude > sqrMoveThreshold)
                {
                    PathRequestManager.RequestPath(new PathRequest(transform.position, targetPosition, OnPathFound));
                    targetPosOld = target.transform.position;
                }
            }
        }
        
    }

    IEnumerator FollowPath()
    {
        if (path.Length > 0)
        {
            Vector3 currentWaypoint = path[0];
            targetIndex = 0;
            while (true)
            {
                if (transform.position == currentWaypoint)
                {
                    targetIndex++;
                    if (targetIndex >= path.Length)
                    {
                        targetIndex = 0;
                        path = null;
                        yield break;
                    }

                    currentWaypoint = path[targetIndex];
                }
                transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
                yield return null;
            }
        }
    }
}
