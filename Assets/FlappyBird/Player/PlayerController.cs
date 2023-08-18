using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static event Action OnJump, OnPlayerDie, OnPlayerCollectPoint;

    [SerializeField] Rigidbody2D rigidBody;
    [SerializeField] Animator animator;

    [SerializeField] float jumpHeight = 5f;
    [SerializeField] float jumpAngle = 20f, fallAngle = 210f;
    [MinMaxSlider(0f, 360f)]
    [SerializeField] Vector2 angleLimit = new Vector2(30f, 315f);

    private Vector3 startPosition;
    private bool isPlaying;

    [MinMaxSlider(0f, 10f)]
    [SerializeField] Vector2 dieForceSlider;

    private void Awake()
    {
        GameEventController.OnGameStart += OnGameStart;
        GameEventController.OnGameRestart += OnGameRestart;

        rigidBody.gravityScale = 0f;
        startPosition = transform.position;
    }
    private void OnGameStart()
    {
        rigidBody.gravityScale = 1.3f;
        isPlaying = true;

        Jump();
    }
    private void OnGameRestart()
    {
        transform.SetPositionAndRotation(startPosition, Quaternion.identity);
        rigidBody.gravityScale = 0f;
        rigidBody.velocity = Vector2.zero;
        rigidBody.angularVelocity = 0f;
        animator.speed = 1f;
    }
    private void Update()
    {
        PlayInProgress();
    }
    private void PlayInProgress()
    {
        if (!isPlaying) return;  
        
        Jump();
        Fall();
        LimitZRotation();       
    }
    public void Jump()
    {
        //Need new input
        if (!Input.GetMouseButtonDown(0)) return;

        rigidBody.velocity = Vector2.up * jumpHeight;
        rigidBody.AddTorque(jumpAngle);
        OnJump?.Invoke();
    }
    private void Fall()
    {
        if (rigidBody.velocity.y < 0f) 
            rigidBody.angularVelocity = -fallAngle;
    }
    private void LimitZRotation()
    {
        Vector3 tempVect = transform.localEulerAngles;
        float zAngle = tempVect.z;

        if(zAngle > 0f && zAngle < 180f) zAngle = Mathf.Min(zAngle, angleLimit.x);
        if (zAngle < 360f && zAngle > 180f) zAngle = Mathf.Max(zAngle, angleLimit.y);

        tempVect.z = zAngle;
        transform.localEulerAngles = tempVect;
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isPlaying) return;
        if (collision.gameObject.layer != LayerMask.NameToLayer("Obstacle")) return;

        Die();
    }
    private void Die()
    {
        isPlaying = false;
        animator.speed = 0f;
        Vector2 dieForce = new Vector2(UnityEngine.Random.Range(dieForceSlider.x, dieForceSlider.y), UnityEngine.Random.Range(dieForceSlider.x, dieForceSlider.y));
        float dieTorque = UnityEngine.Random.Range(dieForceSlider.x, dieForceSlider.y) / 4f;
        rigidBody.AddForce(dieForce, ForceMode2D.Impulse);
        rigidBody.AddTorque(dieTorque, ForceMode2D.Impulse);
        OnPlayerDie?.Invoke();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isPlaying) return;
        if (collision.gameObject.layer != LayerMask.NameToLayer("PointCollider")) return;

        CollectPoint(collision);
    }
    private static void CollectPoint(Collider2D collision)
    {
        collision.gameObject.SetActive(false);
        OnPlayerCollectPoint?.Invoke();
    }
}