using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Realtime;

public class Strawstol : MonoBehaviourPun
{
    public PhotonView PV;
    public GameObject bulletPrefab;
    public GameObject FirePoint;
    public float AttackPower = 10;
    private float angle;
    public GameObject Nova;
    public GameObject player;
    public PlayerHealth playerHealth;
    public SpriteRenderer spriteRenderer;

    public void Update()
    {
        Color color = spriteRenderer.color;

        if (playerHealth.GetInactiveBool())
        {
            color.a = 0f;
        }
        else
        {
            color.a = 255f;
        }

    }

    public void Shoot()
    {
       GameObject bullet = PhotonNetwork.Instantiate(bulletPrefab.name, FirePoint.transform.position, Quaternion.identity);
       bullet.GetComponent<SpitballBullet>().photonID = PV.ViewID;
       bullet.GetComponent<SpitballBullet>().attackDamage = AttackPower;

        if (Nova.activeSelf)
        {
            bullet.GetComponent<Rigidbody2D>().rotation =  player.GetComponent<Rigidbody2D>().rotation + 90;
        }
    }
}
