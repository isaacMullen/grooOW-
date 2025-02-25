using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Color color = new Color(0.5f, 0.4f, 0.7f, 1f);
    // Light muted purple

    private Vector3 originalScale;

    [Header("Patrol Settings")]
    public Transform pointA;
    public Transform pointB;
    public float speed;

    [Header("Animation Settings")]
    public float squishDuration = 0.2f;
    public float squishStrength = 0.2f;
    private bool animPlaying;

    private void Start()
    {      
        GetComponent<MeshRenderer>().material.color = color;
        originalScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        float t = Mathf.PingPong(Time.time * speed, 1);

        transform.position = Vector3.Lerp(pointA.position, pointB.position, t);

        if(!animPlaying)
        {
            TriggerSquish();
        }        
    }

    void TriggerSquish()
    {
        //flag to stop update from calling it each frame
        animPlaying = true;
        // Apply a quick squish effect: shrink on Y axis, enlarge on X axis
        transform.DOScale(new Vector3(originalScale.x + squishStrength, originalScale.y - squishStrength, originalScale.z), squishDuration)
        .SetEase(Ease.InOutQuad)
        .OnComplete(() =>
        {
            transform.DOScale(originalScale, squishDuration)
                .SetEase(Ease.InOutQuad)
                .OnComplete(() => animPlaying = false); // Reset flag only after full cycle
        });
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player")) // More efficient than checking by name
        {
            Debug.Log("HIT PLAYER");
            Actions.OnEnemyCollision?.Invoke();
        }
    }
}
