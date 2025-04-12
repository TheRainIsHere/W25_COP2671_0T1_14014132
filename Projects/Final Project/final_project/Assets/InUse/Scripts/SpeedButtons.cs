using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.UI;

public class SpeedButtons : MonoBehaviour
{
    public Button button;
    private GameManager gameManager;
    public int speed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        button = GetComponent<Button>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        button.onClick.AddListener(SetSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetSpeed()
    {
        Debug.Log(gameObject.name + " was clicked");
        gameManager.StartGame(speed);
    }
}
