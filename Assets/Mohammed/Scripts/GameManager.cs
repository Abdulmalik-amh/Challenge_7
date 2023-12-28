using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PlayerInfo player;

    private void Start()
    {
        // Example: Start the game by initializing player and other game elements
        InitializeGame();
    }

    private void Update()
    {
        // Example: Check for game over conditions
        if (player.health <= 0)
        {
            GameOver();
        }
    }

    private void InitializeGame()
    {
        // Example: Set up the initial state of the game
        player = FindObjectOfType<PlayerInfo>();

        // Other initialization tasks...
    }

    private void GameOver()
    {
        // Example: Handle game over logic, such as displaying a game over screen
        Debug.Log("Game Over!");
    }
}
