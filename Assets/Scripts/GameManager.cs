using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool gameStarted = false;

    public bool GameStarted => gameStarted;

    void Start()
    {
        if (Menu.startGameOnLoad)
        {
            StartGame();
            Menu.startGameOnLoad = false;
        }
    }

    public void StartGame()
    {
        Debug.Log("Game Started!");
        gameStarted = true;
    }

    void Update()
    {
        if (!gameStarted)
            return;
    }
}
