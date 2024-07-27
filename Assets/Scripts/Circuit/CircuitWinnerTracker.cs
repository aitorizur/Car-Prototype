using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CircuitWinnerTracker : MonoBehaviour
{
    [SerializeField] private int lapsToWin = 2;
    [SerializeField] private TextMeshProUGUI winnerText = null;
    [SerializeField] private TextMeshProUGUI maxCircuitFollowersText = null;

    [SerializeField] private List<CircuitFollower> circuitFollowers = new List<CircuitFollower>();

    private bool raceEnded = false;

    private void Awake()
    {
        SetInstanceVariables();
        SetNumberOfFollowers();
    }

    private void SetInstanceVariables()
    {
        foreach (CircuitFollower currentCircuitFollower in circuitFollowers)
        {
            currentCircuitFollower.OnLapEvent += OnFollowerLap;
        }
    }

    private void SetNumberOfFollowers()
    {
        maxCircuitFollowersText.text = "/ " + circuitFollowers.Count;
    }

    private void Update()
    {
        UpdatePositions();
    }

    private void UpdatePositions()
    {
        List<CircuitFollower> circuitFollowersToCheck = new List<CircuitFollower>();
        circuitFollowersToCheck.Add(circuitFollowers[0]);

        for (int i = 1; i < circuitFollowers.Count; i++)
        {
            int positionToInsert = 0;
            foreach (CircuitFollower currentCircuitFollower in circuitFollowersToCheck)
            {
                if (IsFirstAhead(circuitFollowers[i], currentCircuitFollower))
                {
                    positionToInsert = i;
                    break;
                }
                else if (i == circuitFollowersToCheck.Count - 1)
                {
                    positionToInsert = i;
                }
            }

            circuitFollowersToCheck.Insert(positionToInsert, circuitFollowers[i]);
        }

        int position = 1;
        foreach (CircuitFollower currentCircuitFollower in circuitFollowersToCheck)
        {
            currentCircuitFollower.CircuitPosition = position;
            ++position;
        }
    }

    private bool IsFirstAhead(CircuitFollower firstCircuitFollower, CircuitFollower secondCircuitFollower)
    {
        if (firstCircuitFollower.Laps > secondCircuitFollower.Laps)
        {
            return true;
        }
        else if (firstCircuitFollower.Laps == secondCircuitFollower.Laps)
        {
            if (firstCircuitFollower.NextWayPoint > secondCircuitFollower.NextWayPoint)
            {
                return true;
            }
            else if (firstCircuitFollower.NextWayPoint == secondCircuitFollower.NextWayPoint)
            {
                if (firstCircuitFollower.DistanceToNextWaypoint < secondCircuitFollower.DistanceToNextWaypoint)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        else
        { 
            return false;
        }
    }

    private void OnFollowerLap(int lap, GameObject car)
    {
        if (lap >= lapsToWin && raceEnded == false)
        {
            raceEnded = true;
            winnerText.gameObject.SetActive(true);
            winnerText.text = car.name + " is the Winner!";
        }
    }

    public void AddCircuitFollower(CircuitFollower givenFollower)
    {
        circuitFollowers.Add(givenFollower);
        givenFollower.OnLapEvent += OnFollowerLap;
    }
}
