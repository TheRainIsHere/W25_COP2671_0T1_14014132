using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // BTS Management
    public bool isGameActive;
    private bool isGameOver = true;
    public List<GameObject> targets;
    private float spawnRate = 1.0f;
    private float orderRate = 1.0f;
    public AudioSource soundFX;
    public Slider orderBar;
    public TimeKeeper timeKeeper;

    // Stats
    private bool orderInPrg = false;
    private int totalOres;
    private int totalBlowers;
    private int totalAnvils;
    private int totalBuckets;
    private int totalOrders;
    private int completeOrders;
    private int allOrders;
    public TextMeshProUGUI oreText;
    public TextMeshProUGUI blowerText;
    public TextMeshProUGUI anvilText;
    public TextMeshProUGUI bucketText;
    public TextMeshProUGUI ordersText;
    public TextMeshProUGUI finalScore;

    // Menus
    public GameObject gameOverMenu;
    public GameObject titleMenu;
    public GameObject pauseMenu;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timeKeeper.timeOver.AddListener(GameOver);
        //timeKeeper.timeToggled.AddListener(PauseResume);
        gameOverMenu.SetActive(false);
        pauseMenu.SetActive(false);
        titleMenu.SetActive(true);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseResume();
        }
    }

    /// <summary>
    /// When a difficulty button is pressed, start the game and coroutines. Set all scores to 0
    /// </summary>
    /// <param name="speed">Passed in by the button pressed, will determine speed of orders</param>
    public void StartGame(float speed)
    {
        // Set game active
        isGameActive = true;
        isGameOver = false;
        // Close title menu and reset scores
        titleMenu.SetActive(false);
        UpdateScore(0);
        allOrders = 0;
        completeOrders = 0;

        // Start Timer
        timeKeeper.StartTimer(1.5f);

        orderRate = speed;

        // Begin all necessary coroutines
        StartCoroutine(SpawnTarget());
        StartCoroutine(CreateOrders());
        StartCoroutine(CompleteOrder());
    }

    IEnumerator SpawnTarget()
    {
        while (isGameActive)
        {
            yield return new WaitForSeconds(spawnRate);

            int index = UnityEngine.Random.Range(0, targets.Count);
            Instantiate(targets[index]);
        }
    }
    IEnumerator CreateOrders()
    {
        orderRate *= 2f;
        while (isGameActive)
        {
            yield return new WaitForSeconds(orderRate);

            totalOrders++;
            allOrders++;
            ordersText.text = totalOrders.ToString();
        }
    }
    IEnumerator CompleteOrder()
    {
        while (isGameActive)
        {
            yield return new WaitForSeconds(1.5f);

            if (totalOres > 0 && totalBlowers > 0 && totalAnvils > 0 && totalBuckets > 0 && totalOrders > 0 && !orderInPrg)
            {
                orderInPrg = true;
                completeOrders++;

                // Remove one from each stage & order
                totalOres--;
                totalBlowers--;
                totalAnvils--;
                totalBuckets--;
                totalOrders--;

                // Want to have the waiting + updating slider here
                UpdateScore(-1);
                orderInPrg = false;
            }
        }
        
    }

    public void PlaySFX(AudioResource itemSFX)
    {
        soundFX.resource = itemSFX;
        soundFX.Play();
    }

    public void UpdateScore(int itemNumber)
    {
        switch(itemNumber)
        {
            case 0:
                // Reset all to 0
                totalOres = 0;
                totalBlowers = 0;
                totalAnvils = 0;
                totalBuckets = 0;
                break;
            case 1:
                totalOres++;
                break;
            case 2:
                totalBlowers++;
                break;
            case 3:
                totalAnvils++;
                break;
            case 4:
                totalBuckets++;
                break;
            case 5:
                // Goblin
                int goblinItem;
                goblinItem = UnityEngine.Random.Range(1, targets.Count - 1);
                ResetScores(goblinItem);
                break;
            default:
                break;
        }

        // Update the score labels
        oreText.text = totalOres.ToString();
        blowerText.text = totalBlowers.ToString();
        anvilText.text = totalAnvils.ToString();
        bucketText.text = totalBuckets.ToString();
        ordersText.text = totalOrders.ToString();
    }
    private void ResetScores(int itemNumber)
    {
        switch(itemNumber)
        {
            case 1:
                totalOres = 0;
                break;
            case 2:
                totalBlowers = 0;
                break;
            case 3:
                totalAnvils = 0;
                break;
            case 4:
                totalBuckets = 0;
                break;
        }

        // Call for scores to be updated using an invalid case to break out of switch
        UpdateScore(-1);
    }
    private void PauseResume()
    {
        if (isGameActive)
        {
            isGameActive = false;
            timeKeeper.PauseTimer();
            pauseMenu.SetActive(true);
        }
        else if (!isGameActive && !isGameOver)
        {
            isGameActive = true;
            timeKeeper.ResumeTimer();
            pauseMenu.SetActive(false);

            // Restart all necessary coroutines
            StartCoroutine(SpawnTarget());
            StartCoroutine(CreateOrders());
            StartCoroutine(CompleteOrder());
        }
    }
    public void QuitGame()
    {
        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }
    public void RestartGame()
    {
        gameOverMenu.gameObject.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private void GameOver()
    {
        isGameActive = false;
        isGameOver = true;
        gameOverMenu.SetActive(true);
        finalScore.text = completeOrders + " / " + allOrders + "\nOrders Complete";
    }
}
