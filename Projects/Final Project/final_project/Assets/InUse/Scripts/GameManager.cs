using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // BTS Management
    public bool isGameActive;
    public List<GameObject> targets;
    private float spawnRate = 1.0f;
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
    public TextMeshProUGUI oreText;
    public TextMeshProUGUI blowerText;
    public TextMeshProUGUI anvilText;
    public TextMeshProUGUI bucketText;
    public TextMeshProUGUI ordersText;
    // Menus
    public GameObject gameOverMenu;
    public GameObject titleMenu;
    public GameObject pauseMenu;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timeKeeper.timeOver.AddListener(GameOver);

    }

    // Update is called once per frame
    void Update()
    {
        // if game is active, run timer
    }

    /// <summary>
    /// When a difficulty button is pressed, start the game and coroutines. Set all scores to 0
    /// </summary>
    /// <param name="speed">Passed in by the button pressed, will determine speed of orders</param>
    public void StartGame(float speed)
    {
        // Set game active
        isGameActive = true;
        // Close title menu and reset scores
        titleMenu.SetActive(false);
        UpdateScore(0);

        // Start Timer
        timeKeeper.StartTimer(1.5f);

        // Begin all necessary coroutines
        StartCoroutine(SpawnTarget());
        StartCoroutine(CreateOrders(speed));
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

    IEnumerator CreateOrders(float speed)
    {
        float orderRate = speed * 2f;
        while (isGameActive)
        {
            yield return new WaitForSeconds(orderRate);

            totalOrders++;
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
                Debug.Log("Completing next order");

                orderInPrg = true;

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
                Debug.Log("Goblin clicked");
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

    private void GameOver()
    {
        isGameActive = false;
        gameOverMenu.SetActive(true);
    }
}
