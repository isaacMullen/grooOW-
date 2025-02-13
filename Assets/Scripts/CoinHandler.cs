using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinHandler : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {       
        
        if (collision.CompareTag("Player")) // More efficient than checking by name
        {
            Debug.Log("RAN OVER COIN");
            Destroy(gameObject); // Optional: Destroy coin when collected
            Actions.OnCoinCollected?.Invoke();
        }
    }
}
