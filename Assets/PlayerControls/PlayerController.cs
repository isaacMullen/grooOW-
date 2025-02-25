using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms.Impl;
using static InputActions;

public class PlayerController : MonoBehaviour
{
    private Vector3 moveVector = Vector2.zero;
    private Rigidbody2D rb;    
    private AudioSource audioSource;   

    [Header("Movement Settings")]
    public float moveSpeed;
    public float jumpForce;
    
    [Header("Animation Settings")]
    public float squishDuration = 0.2f;
    public float squishStrength = 0.2f;
    private bool animPlayed;

    [Header("Sound Effect Settings")]
    public AudioClip jumpSound;
    //public AudioClip collectSound;

    [Header("Ground Check Settings")]
    public float groundCheckDistance = 1f;
    public LayerMask groundLayer;
    private bool isGrounded;
  
    //private int score = 0;    
    //private TextMeshProUGUI scoreTextMesh;
    //public int TotalCoins { get; private set; }
    //public string ScoreString => score.ToString();

    private GameObject parent;
    private Vector3 originalScale;

    void Start()
    {        
        //components that live on the player
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();

        //for my animation
        originalScale = transform.localScale;

        //storing a reference to the coins container
        //parent = GameObject.FindGameObjectWithTag("PickupParent");        
        //TotalCoins = parent.transform.childCount;

        ////dynamically finds the coin text
        //GameObject scoreObject = GameObject.FindGameObjectWithTag("ScoreText");
        
        //if (scoreObject != null)
        //{
        //    scoreTextMesh = scoreObject.GetComponent<TextMeshProUGUI>();
        //}
        //else
        //{
        //    Debug.LogError("Object with Tag: 'ScoreText' not found!");
        //}

        //will set initial score
        //UpdateScore(scoreTextMesh);
        //starting the main background music
        audioSource.Play();
    }
    
    private void OnEnable()
    {
        Actions.OnMovePerformed += HandlePlayerMove;
        Actions.OnJumpPerformed += HandlePlayerJump;
        //Actions.OnCoinCollected += CollectCoin;
    }

    private void OnDisable()
    {
        Actions.OnMovePerformed -= HandlePlayerMove;
        Actions.OnJumpPerformed -= HandlePlayerJump;
        //Actions.OnCoinCollected -= CollectCoin;
    }

    private void HandlePlayerMove(Vector2 direction)
    {
        UpdateMovePlayer(direction);
    }

    private void UpdateMovePlayer(Vector2 direction)
    {
        moveVector = direction;
    }

    private void HandlePlayerJump()
    {
        if(isGrounded)
        UpdatePlayerJump();
    }

    private void UpdatePlayerJump()
    {
        audioSource.PlayOneShot(jumpSound);
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    private void Update()
    {
        CheckGround();
        if (!isGrounded) { animPlayed = false; }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveVector.x * moveSpeed, rb.velocity.y);
    }

    void TriggerSquish()
    {
        // Apply a quick squish effect: shrink on Y axis, enlarge on X axis
        transform.DOScale(new Vector3(originalScale.x + squishStrength, originalScale.y - squishStrength, originalScale.z), squishDuration)
            .SetEase(Ease.InOutQuad)  // Optional easing for smoothness
            .OnKill(() =>
                transform.DOScale(originalScale, squishDuration).SetEase(Ease.InOutQuad)  // Return to original scale
            );

        animPlayed = true;
    }

    void CheckGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer); 
        
        isGrounded = hit.collider != null;

        Debug.DrawRay(transform.position, Vector2.down * groundCheckDistance, isGrounded ? Color.green : Color.red);

        if (isGrounded && animPlayed == false)
        {           
            TriggerSquish();
        }        
    }

    //private void CollectCoin()
    //{
    //    VictoryCheck();
    //    audioSource.PlayOneShot(collectSound);

    //    score += 1;

    //    UpdateScore(scoreTextMesh);
    //} 
    
    //private void UpdateScore(TextMeshProUGUI text)
    //{
    //    text.text = $"{ScoreString}/{TotalCoins}";
    //}

    //private bool VictoryCheck()
    //{
    //    if (score >= TotalCoins)
    //    {
    //        return true;
    //    }
    //    else return false;
    //}
}
        