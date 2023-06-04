using BunnyHouse.Core;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Handles bunny movement
/// </summary>
public class BunnyScript : MonoBehaviour
{
    public static BunnyScript I;
    [SerializeField] float MaxJumpDistance;
    [SerializeField] Animator BunnyAnimation;
    public float JumpDistance;
    public float JumpDeltaY;
    public float TargetLocation;
    public float MaxYPos;
    public float MinYPos;
    float time;
    public bool isHidden = false;

    SpriteRenderer sr;

    private void Awake()
    {
        I = this;
        sr = GetComponent<SpriteRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {
        TargetLocation = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (DataSystem.GameData.Bunny.Name == "" || isHidden)
        {
            sr.enabled = false;
        }
        else
        {
            sr.enabled = true;
        }
        if (!Singleton.isUIOverride())
        {
            if (Input.GetMouseButtonDown(0))
            {
                time = 0f;
            }
            if (Input.GetMouseButton(0))
            {
                time += Time.deltaTime;
            }
            if (Input.GetMouseButtonUp(0))
            {
                if (time < 0.1f)
                {
                    BunnyAnimation.SetBool("Jumping", true);
                    Vector3 TargetVector = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    TargetLocation = TargetVector.x;
                    float distance = (TargetLocation - transform.position.x);
                    float jumpsRequired = Mathf.Max(Mathf.Ceil(Mathf.Abs(distance / MaxJumpDistance)), 1f);
                    JumpDeltaY = (Mathf.Clamp(TargetVector.y + 2f, MinYPos, MaxYPos) - transform.position.y) / jumpsRequired;
                    JumpDistance = distance / jumpsRequired;
                }
            }
        }
    }
}
