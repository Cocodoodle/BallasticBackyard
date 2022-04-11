using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpitballBullet : MonoBehaviourPun
{

    public PhotonView PV;
    public int photonID;
    GameObject FirePoint;
    Rigidbody2D rb;
    private GameObject shooterPlayer;
    private PhotonView shooterPV;
    public float TimeTillDestroy = 7f;
    public float attackDamage;
    public float speed = 30f;

    public void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
   
        shooterPV = PhotonNetwork.GetPhotonView(photonID);

        GameObject shooterPlayer = shooterPV.gameObject;

        FirePoint = FindGOByName(shooterPlayer, "FirePoint");

        if (PV.IsMine)
        {
            ShowBulletMovement();
        }

        StartCoroutine(DestroyBulletOnTimer(TimeTillDestroy));
    }

    public IEnumerator DestroyBulletOnTimer(float sec)
    {
        yield return new WaitForSeconds(sec);

        if (PV.IsMine)
        {
            PhotonNetwork.Destroy(this.gameObject);
        }
    }

    public void DestroyBullet()
    {
        if (PV.IsMine)
        {
            PhotonNetwork.Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player 1" || collision.gameObject.tag == "Player 2")
        {
            DestroyBullet();
        }
    }

    private GameObject FindGOByName(GameObject parent, string childName)
    {
        for(int i = 0; i < parent.transform.childCount; i++)
        {
            if(parent.transform.GetChild(i).name == childName)
            {
                return parent.transform.GetChild(i).gameObject;
            }

            GameObject under = FindGOByName(parent.transform.GetChild(i).gameObject, childName);

            if(under != null)
            {
                return under;
            }

        }

        return null;
    }

    void ShowBulletMovement()
    {
        rb.AddForce(-FirePoint.transform.up * speed, ForceMode2D.Impulse);
    }

    public float GetAttackDamage()
    {
        return attackDamage;
    }

}
