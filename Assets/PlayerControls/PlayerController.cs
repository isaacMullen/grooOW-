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
    public AudioClip collectSound;

    [Header("Ground Check Settings")]
    public float groundCheckDistance = 1f;
    public LayerMask groundLayer;
    private bool isGrounded;

    [Header("UI Settings")]
    public int score = 0;
    int total = 0;
    public string ScoreString => score.ToString();
    public TextMeshProUGUI text;

    private GameObject parent;
    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        parent = GameObject.FindGameObjectWithTag("PickupParent");
        total = parent.transform.childCount;

        //starting the main background music
        audioSource.Play();
    }

    private void OnEnable()
    {
        Actions.OnMovePerformed += HandlePlayerMove;
        Actions.OnJumpPerformed += HandlePlayerJump;
        Actions.OnCoinCollected += UpdateScore;
    }

    private void OnDisable()
    {
        Actions.OnMovePerformed -= HandlePlayerMove;
        Actions.OnJumpPerformed -= HandlePlayerJump;
        Actions.OnCoinCollected -= UpdateScore;
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

    private void UpdateScore()
    {
        audioSource.PlayOneShot(collectSound);

        score += 1;

        text.SetText($"{ScoreString}/{total}");
    }
}
        