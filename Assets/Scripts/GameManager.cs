using UnityEngine;
using UnityEngine.Events;

public enum GameState
{
    //planningStage, // Player places portals down to plan their route
    //drivingStage, // Player is driving to complete the delivery
    wheelSpin, //slot style wheel shows 
    delivery,
    upgradeShop
}
// The class manages transitions between planning and driving stages of the game
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameState currentGameState;
    public UnityEvent onEnterSpinningStage;
    public UnityEvent onEnterDrivingStage;
    [SerializeField] private Vector2 playerStartPosition;
    [SerializeField] private Vector2 playerStartRotation;
    [SerializeField] private Camera spinningCamera;
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
        EnterSpinningStage();
    }

    public void EnterSpinningStage()
    {
        currentGameState = GameState.wheelSpin;
        spinningCamera.gameObject.SetActive(true);
        drivingCamera.gameObject.SetActive(false);
        onEnterSpinningStage.Invoke();
    }
    public void EnterDrivingStage()
    {
        currentGameState = GameState.delivery;
        spinningCamera.gameObject.SetActive(false);
        drivingCamera.gameObject.SetActive(true);
        onEnterDrivingStage.Invoke();
    }
}
