using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Medkit : MonoBehaviour
{
    public PhotonView PV;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PV.RPC("DisplayifWork", RpcTarget.All, "Picked Up");
        DestroyItem();
    }

    public void DestroyItem()
    {
        if (PV.IsMine)
        {
            PhotonNetwork.Destroy(this.gameObject);
        }
    }

    [PunRPC]
    public void DisplayifWork(string str)
    {
        Debug.Log(str);
    }
}