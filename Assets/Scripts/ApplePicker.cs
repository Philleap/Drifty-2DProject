using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;
public class ApplePicker : MonoBehaviour
{
    [Header("Inscribed")]
    public GameObject basketPrefab;
    public int numBaskets = 1;
    public float basketBottomY = 10f;
    public float basketSpacingY = 2f;
    public List<GameObject> basketList;
    public TMP_Text startText;
    public GameObject finishPanel;
    public enum GameState
    {
        Waiting,
        Racing,
        Finished
    }
    public GameState currentState = GameState.Waiting;
    public RaceTimer raceTimer;
    public Basket playerCar;
    void Start()
    {
        currentState = GameState.Waiting;
        basketList = new List<GameObject>();
        GameObject tBasketGO = Instantiate(
            basketPrefab,
            new Vector3(0f, 0f, 0f),
            Quaternion.Euler(0f, 0f, -90f)
        );
        Vector3 pos = new Vector3(0f, 0f, 0f);
        tBasketGO.transform.position = pos;
        basketList.Add(tBasketGO);
        playerCar = tBasketGO.GetComponent<Basket>();
        CameraBehavior cameraFollow = Camera.main.GetComponent<CameraBehavior>();
        if (cameraFollow != null)
        {
            cameraFollow.car = tBasketGO.transform;
        }
        startText.gameObject.SetActive(true);
        finishPanel.SetActive(false);
        // raceTimer.StartRace();
    }
    public void AppleMissed()
    {
        GameObject[] appleArray = GameObject.FindGameObjectsWithTag("Apple");
        foreach (GameObject tempGO in appleArray)
        {
            Destroy(tempGO);
        }
        int basketIndex = basketList.Count - 1;
        GameObject basketGO = basketList[basketIndex];
        basketList.RemoveAt(basketIndex);
        Destroy(basketGO);
        if (basketList.Count == 0)
        {
            SceneManager.LoadScene("_Scene_0");
        }
    }
    void Update()
    {
        if (currentState == GameState.Waiting &&
            Input.GetMouseButtonDown(0))
        {
            StartRace();
        }
    }
    public void StartRace()
    {
        if (currentState != GameState.Waiting)
            return;

        currentState = GameState.Racing;
        raceTimer.StartRace();
        startText.gameObject.SetActive(false);
    }
    public void FinishRace()
    {
        Debug.Log("FinishRace() was called!");

        if (currentState != GameState.Racing)
        {
            Debug.LogWarning("Race is not in Racing state!");
            return;
        }

        currentState = GameState.Finished;

        raceTimer.FinishRace();
        playerCar.DisableControls();

        if (finishPanel != null)
        {
            finishPanel.SetActive(true);
            Debug.Log("FinishPanel activated!");
        }
        
    }
    public void RestartRace()
    {
        Debug.Log("Restart button clicked!");
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
