using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Realtime;

public class Spitball : MonoBehaviourPunCallbacks
{
    public PhotonView PV;
    public List<GameObject> items = new List<GameObject>();
    private GameObject previousWeapon = null;

    void Start()
    {
        PV = this.GetComponent<PhotonView>();

        GiveRandomWeapon();

    }

    public void GiveRandomWeapon()
    {
        if (PV.IsMine)
        {
            if (previousWeapon != null)
            {
                previousWeapon.SetActive(false);
            }

            int ItemIndex = Random.Range(0, items.Count);
            GameObject weapon = items[ItemIndex];
            weapon.SetActive(true);
            previousWeapon = weapon;
            Hashtable hash = new Hashtable();
            hash.Add("ItemIndex", ItemIndex);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (!PV.IsMine && targetPlayer == PV.Owner)
        {
            items[(int)changedProps["ItemIndex"]].SetActive(true);
        }
    }

}
