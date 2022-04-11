using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class CameraAssign : MonoBehaviour
{
    public Camera cameraPrefab;

    public void Awake()
    {
        Dictionary<int, Photon.Realtime.Player> pList = Photon.Pun.PhotonNetwork.CurrentRoom.Players;
        foreach (KeyValuePair<int, Photon.Realtime.Player> p in pList)
        {
            Camera PlayerCamera = Instantiate(cameraPrefab, new Vector3(0, 0, -50), Quaternion.identity);
            PlayerCamera.name = p.Value.NickName + " Camera";
        }
        

    }

    
}
