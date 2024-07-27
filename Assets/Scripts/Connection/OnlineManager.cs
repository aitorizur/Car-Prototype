using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class OnlineManager : MonoBehaviour
{
    [SerializeField] private Circuit onlineCircuit;
    [SerializeField] private CircuitWinnerTracker onlineWinnerTracker;

    public string playerName = "Federico";
    public GameObject playerGO;
    private Dictionary<string, OnlineCar> onlineCars = new Dictionary<string, OnlineCar>();
    public OnlineCar onlineCarPrefab;
    private SocketManager socket;

    public float timeToSendData = 0.33f;
    private float timeToSendDataAux = 0f;

    private void Awake()
    {
        socket = GetComponent<SocketManager>();
    }

    private void LateUpdate()
    {
        timeToSendDataAux -= Time.deltaTime;
        if (timeToSendDataAux <= 0f)
        {
            SendUpdate();

            timeToSendDataAux = timeToSendData;
        }
    }

    private void SendPosition()
    {
        string currentPosition = "uPOS|" + playerName + "|" + playerGO.transform.position.ToString();
        socket.Send(currentPosition);
    }
    private void SendRotation()
    {
        string currentPosition = "uROT|" + playerName + "|" + playerGO.transform.rotation.ToString();
        socket.Send(currentPosition);
    }

    void SendUpdate()
    {
        SendPosition();
        SendRotation();
    }

    public void ParseMessage(string str)
    {
        string[] strSplit = str.Split('|');
        switch (strSplit[0])
        {
            case "JOIN":
                NewPlayerJoins(strSplit[1]);
                break;
            case "uPOS":
                PositionReceived(strSplit[1], strSplit[2]);
                break;
            case "uROT":
                RotationReceived(strSplit[1], strSplit[2]);
                break;
        }
    }

    public void ParseMessages(string str)
    {
        string[] strSplit = str.Split('$');

        foreach (string s in strSplit)
        {
            ParseMessage(s);
        }
    }

    private void NewPlayerJoins(string str)
    {
        //The string is the player identifier
        var clone = Instantiate(onlineCarPrefab);
        CircuitFollower cloneCircuitFllower = clone.GetComponent<CircuitFollower>();
        cloneCircuitFllower.circuit = onlineCircuit;
        onlineWinnerTracker.AddCircuitFollower(cloneCircuitFllower);
        onlineCars[str] = clone;
    }

    private void PositionReceived(string str1, string str2)
    {
        //Receives player identifier(1) and player position(2)
        onlineCars[str1].targetPosition = StringToVector3(str2);
    }

    private void RotationReceived(string str1, string str2)
    {
        //Receives player identifier(1) and player rotation(2)
        onlineCars[str1].targetRotation = StringToQuaternion(str2);
    }


    public static Vector3 StringToVector3(string sVector)
    {
        // Remove the parentheses
        if (sVector.StartsWith("(") && sVector.EndsWith(")"))
        {
            sVector = sVector.Substring(1, sVector.Length - 2);
        }

        // split the items
        string[] sArray = sVector.Split(',');

        // store as a Vector3
        Vector3 result = new Vector3(
            float.Parse(sArray[0], CultureInfo.InvariantCulture),
            float.Parse(sArray[1], CultureInfo.InvariantCulture),
            float.Parse(sArray[2], CultureInfo.InvariantCulture));

        return result;
    }

    public static Quaternion StringToQuaternion(string sVector)
    {
        // Remove the parentheses
        if (sVector.StartsWith("(") && sVector.EndsWith(")"))
        {
            sVector = sVector.Substring(1, sVector.Length - 2);
        }

        // split the items
        string[] sArray = sVector.Split(',');

        // store as a Vector3
        Quaternion result = new Quaternion(
            float.Parse(sArray[0], CultureInfo.InvariantCulture),
            float.Parse(sArray[1], CultureInfo.InvariantCulture),
            float.Parse(sArray[2], CultureInfo.InvariantCulture),
            float.Parse(sArray[3], CultureInfo.InvariantCulture));

        return result;
    }
}

