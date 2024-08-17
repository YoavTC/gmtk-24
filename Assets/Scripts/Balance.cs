using System;
using UnityEngine;

public class Balance : MonoBehaviour
{
    [SerializeField] private float targetRotation;
    [SerializeField] private float balanceForce;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    void FixedUpdate()
    {
        rb.MoveRotation(Mathf.LerpAngle(rb.rotation, targetRotation, Time.fixedDeltaTime * balanceForce));
    }
}
