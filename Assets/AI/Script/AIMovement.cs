using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIMovement : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _agent;

    [SerializeField] private FieldOfView FOV;

    private Vector3 firstPos;

    private float tmpRad;

    private float tmpAngle;
    // Start is called before the first frame update
    void Start()
    {
        firstPos = transform.position;
        tmpAngle = FOV.angle;
        tmpRad = FOV.radius;
    }

    // Update is called once per frame
    void Update()
    {
        if (FOV.canSeePlayer == true)
        {
            FOV.radius = 20;
            FOV.angle = 360;
            _agent.destination = FOV.playerRef.transform.position;
        }
        else
        {
            FOV.radius = tmpRad;
            FOV.angle = tmpAngle;
            
            _agent.destination = firstPos;
        }
    }
}
