using DG.Tweening;
using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    GameManager gameManager;

    [Header("Player Prefab")]
    public PlayerController player;
    private PlayerController playerIntance;

    private Transform spawnPosition;
    
    public int CurrentSceneIndex { get; private set; }
    
    // Start is called before the first frame update
    void Awake()
    {
        CurrentSceneIndex = SceneManager.GetActiveScene().buildIndex;
     
        Debug.Log($"Started | Current Scene: {CurrentSceneIndex}");
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        Actions.OnLevelComplete += LoadNextLevel;
        Actions.OnEnemyCollision += ResetLevel;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        Actions.OnLevelComplete -= LoadNextLevel;
        Actions.OnEnemyCollision -= ResetLevel;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }        

        SpawnPlayer();
    }

    void SpawnPlayer()
    {
        spawnPosition = GameObject.FindGameObjectWithTag("SpawnPosition").GetComponent<Transform>();
        if (playerIntance == null)
        {
            playerIntance = Instantiate(player, new Vector3(spawnPosition.position.x, spawnPosition.position.y, 0), Quaternion.identity);
        }
        else
        {
            Debug.Log("Player Prefab is not assigned");
        }
    }

    void LoadNextLevel()
    {
        Debug.Log("Loading Next Scene");
        if(CurrentSceneIndex + 1 <= SceneManager.sceneCount)
        {            
            SceneManager.LoadScene(CurrentSceneIndex + 1);
            CurrentSceneIndex += 1;
        }
        else
        {
            Debug.Log("No more scenes");
        }        
    }
    
    void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
