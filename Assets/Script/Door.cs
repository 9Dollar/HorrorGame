using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    private float targetYRot;

    public float smooth;
    public bool autoClose;

    private Transform _player;

    private float defaultYRot = 0f;
    private float timer = 0f;

    public Transform pivot;

    private bool isOpen;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        defaultYRot = transform.eulerAngles.y;
    }

    private void Update()
    {
        pivot.rotation = Quaternion.Lerp(pivot.rotation, Quaternion.Euler(0f, defaultYRot + targetYRot, 0f), smooth * Time.deltaTime);

        timer -= Time.deltaTime;

        if (timer <= 0f && isOpen && autoClose)
        {
            ToggleDoor(_player.position);
        }
    }

    public void ToggleDoor(Vector3 pos)
    {
        isOpen = !isOpen;

        if (isOpen)
        {
            Vector3 dir = (pos - transform.position);
            targetYRot = -Mathf.Sign(Vector3.Dot(transform.right, dir)) * 90f;
            timer = 5f;
        }
        else
        {
            targetYRot = 0f;
        }
    }

    public void Open(Vector3 pos)
    {
        if (!isOpen)
        {
            ToggleDoor(pos);
        }
    }
    public void Close(Vector3 pos)
    {
        if (isOpen)
        {
            ToggleDoor(pos);
        }
    }

    public void Interact()
    {
        ToggleDoor(_player.position);
    }

    public string GetDescription()
    {
        if (isOpen) return "Close The Door";
        return "Open The Door";
    }
}
