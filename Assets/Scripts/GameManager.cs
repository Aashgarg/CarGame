using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public enum GameState
{
    
}
// The class manages transitions between planning and driving stages of the game
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    //public GameState currentGameState;
    [SerializeField] private Vector2 playerStartPosition;
    [SerializeField] private Vector2 playerStartRotation;
    [SerializeField] NightData[] nights;
    public NightData currentNight;
    private int nightIndex = 0;
    public bool endOfGame = false;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            currentNight = nights[nightIndex];
        }
        else
        {
            Debug.Log("set game manager false");
            Destroy(gameObject);
        }
    }

    void Start()
    {
        
    }

    public void SwitchNight()
    {
        nightIndex += 1;
        if (nightIndex + 1 >= nights.Length)
        {
            // its not printing the debug log for some reason
            Debug.Log("All nights completed!");
            // Handle end of game logic here
            endOfGame = true;
            return;
        }
        else
        {
            currentNight = nights[nightIndex];
            //Show UI elements transitions to next night
        }
        
    }
}
