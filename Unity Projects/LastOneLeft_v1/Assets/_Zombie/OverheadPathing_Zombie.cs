using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]

public class OverheadPathing_Zombie : MonoBehaviour
{
    [Header("CONFIG")]
    [SerializeField] float normalSpeed = 3;
    [SerializeField] float stunSpeed = 0;
    [SerializeField] float stumbleSpeed = 1.5f;
    [SerializeField] float fallenMoveSpeed = 0;
    [SerializeField] float enragedSpeed = 5;
    //[SerializeField] float crawlMoveSpeed = 0.5f;

    [Header("DEBUG")]
    public NavMeshAgent myAgent;
    public Status_Zombie statusScript;
    public Transform target;

    // Start is called before the first frame update
    void Awake()
    {
        statusScript = GetComponentInParent<Status_Zombie>();

        //fetch and configure agent component
        myAgent = GetComponent<NavMeshAgent>();
        myAgent.updateRotation = false;
        myAgent.updateUpAxis = false;

        //enable agent after configuration is complete to avoid errors
        myAgent.enabled = true;

    }

    private void Start()
    {
        target = FindClosestWindowTransform();

    }

    // Update is called once per frame
    void Update()
    {
        //set destination to the position of the target every frame
        myAgent.SetDestination(target.position);
        UpdateAgentSpeedBasedOnStatus();

    }

    private void UpdateAgentSpeedBasedOnStatus()
    {
        float currentSpeed = 0;
        switch (statusScript.standingState)
        {
            case ZmbStandingStateEnum.NoStatus:
                currentSpeed = normalSpeed;
                break;

            case ZmbStandingStateEnum.Stunned:
                currentSpeed = stunSpeed;
                break;

            case ZmbStandingStateEnum.Stumbling:
                currentSpeed = stumbleSpeed;
                break;

            case ZmbStandingStateEnum.FallenForward:
                currentSpeed = fallenMoveSpeed;
                break;

            case ZmbStandingStateEnum.FallenBackward:
                currentSpeed = fallenMoveSpeed;
                break;

            case ZmbStandingStateEnum.Enraged:
                currentSpeed = enragedSpeed;
                break;
        }

        myAgent.speed = currentSpeed;
    }

    /// <summary>
    /// returns the transform of the closest overhead window to the zombie
    /// </summary>
    /// <returns></returns>
    Transform FindClosestWindowTransform()
    {
        GameObject[] OverheadWindowList = GameObject.FindGameObjectsWithTag("Overhead Window");

        Transform closestWindowTransform = null;
        float ?closestWindowDistance = null;

        //find the closest window to the zombie
        foreach (GameObject window in OverheadWindowList)
        {
            float currentWindowDistance = Vector3.Distance(window.transform.position, transform.position);

            if (closestWindowDistance == null)
            {
                closestWindowDistance = currentWindowDistance;
                closestWindowTransform = window.transform;
            }
            else if (currentWindowDistance < closestWindowDistance)
            {
                closestWindowDistance = currentWindowDistance;
                closestWindowTransform = window.transform;
            }
        }

        return closestWindowTransform;

    }

}
