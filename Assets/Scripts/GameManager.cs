using UnityEngine;
using UnityEngine.Events;

public enum GameState
{
    planningStage, // Player places portals down to plan their route
    drivingStage, // Player is driving to complete the delivery
}
// The class manages transitions between planning and driving stages of the game
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameState currentGameState;
    public UnityEvent onEnterPlanningStage;
    public UnityEvent onEnterDrivingStage;
    [SerializeField] private Vector2 playerStartPosition;
    [SerializeField] private Vector2 playerStartRotation;
    [SerializeField] private Camera planningCamera;
    [SerializeField] private Camera drivingCamera;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        EnterPlanningStage();
    }

    public void EnterPlanningStage()
    {
        currentGameState = GameState.planningStage;
        planningCamera.gameObject.SetActive(true);
        drivingCamera.gameObject.SetActive(false);
        onEnterPlanningStage.Invoke();
    }
    public void EnterDrivingStage()
    {
        currentGameState = GameState.drivingStage;
        planningCamera.gameObject.SetActive(false);
        drivingCamera.gameObject.SetActive(true);
        onEnterDrivingStage.Invoke();
    }
}
