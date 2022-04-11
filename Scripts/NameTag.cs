using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class NameTag : MonoBehaviour
{

    private PhotonView PV;
    public TextMeshProUGUI Text;


    void Start()
    {
        PV = GetComponent<PhotonView>();

        if(PV.IsMine)
        {
            return;
        }

        nameTag();
    }

    private void nameTag()
    {
        Text.text = PV.Owner.NickName;
    }
}
