using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinHandler : MonoBehaviour
{
    private int score = 0;
    private TextMeshProUGUI scoreTextMesh;
    public int TotalCoins { get; private set; }
    public string ScoreString => score.ToString();

    private GameObject parent;

    [Header("Sound Effect Settings")]  
    public AudioClip collectSound;
    private AudioSource audioSource;

    private void OnEnable()
    {
        Actions.OnCoinCollected += CollectCoin;
    }

    private void OnDisable()
    {
        Actions.OnCoinCollected -= CollectCoin;
    }

    void Start()
    {
        audioSource = GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>();

        parent = GameObject.FindGameObjectWithTag("PickupParent");
        TotalCoins = parent.transform.childCount;
        Debug.Log($"Child Count: {parent.transform.childCount}");

        //dynamically finds the coin text
        GameObject scoreObject = GameObject.FindGameObjectWithTag("ScoreText");

        if (scoreObject != null)
        {
            scoreTextMesh = scoreObject.GetComponent<TextMeshProUGUI>();
        }
        else
        {
            Debug.LogError("Object with Tag: 'ScoreText' not found!");
        }

        UpdateScore(scoreTextMesh);       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {                               
        if (collision.CompareTag("Player")) // More efficient than checking by name
        {
            Debug.Log("RAN OVER COIN");               
            Actions.OnCoinCollected?.Invoke();
            gameObject.SetActive(false);
        }
    }

    private void CollectCoin()
    {
        
        audioSource.PlayOneShot(collectSound);

        score += 1;
        Debug.Log($"Score: {score}");        

        UpdateScore(scoreTextMesh);
        VictoryCheck();
    }

    private void UpdateScore(TextMeshProUGUI text)
    {
        text.text = $"{ScoreString}/{TotalCoins}";
    }

    private bool VictoryCheck()
    {
        if (score >= TotalCoins)
        {
            Actions.OnLevelComplete?.Invoke();
            return true;
        }
        else return false;
    }

    
}
