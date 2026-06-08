using UnityEngine;

public enum GameState
{
    planningStage, // Player places portals down to plan their route
    drivingStage, // Player is driving to complete the delivery
    deliveryComplete, // Player successfully completes the delivery
}
public class GameManager : MonoBehaviour
{
    public GameState currentState;
    private bool isPaused;
    private float transitionDelay = 1f; // Time to wait before transitioning to the next state
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void StartPlanningStage()
    {
        currentState = GameState.planningStage;
        // Enable portal placement, disable player control
    }

    void StartDrivingStage()
    {
        currentState = GameState.drivingStage;
        // Disable portal placement, enable player control
    }

    void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f; // Pause the game
    }
}
