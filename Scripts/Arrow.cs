using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Arrow : MonoBehaviour
{
    public PhotonView PV;
    private bool isShowing = false;
    private GameObject target;
    public Rigidbody2D rb;
    public SpriteRenderer sprite;
    public Color NormalColor;
    public Color TransparentColor;
    public GameObject GameManager;
    private Color color;
    public Transform player;
    public float ang = 230f;

    void Update()
    {
        transform.position = player.position;
        GameManager = GameObject.Find("GameManager");
        MainGameManager mainGameManager = GameManager.GetComponent<MainGameManager>();

        if (PV.Owner.IsMasterClient)
        {
            target = mainGameManager.playerTwo;
        }
        else
        {
            target = mainGameManager.playerOne;
            print(target);
        }

        if (!PV.IsMine)
        {
            return;
        }

        if (!isShowing && Input.GetKeyDown(KeyCode.Tab))
        {
            print("fifi");
            isShowing = true;
            sprite.color = NormalColor;
        }
        else if (isShowing && Input.GetKeyDown(KeyCode.Tab))
        {
            isShowing = false;
            sprite.color = TransparentColor;
        }


        if(target != null)
        {
            Vector3 dir = target.GetComponent<Rigidbody2D>().position - rb.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - ang;
            rb.rotation = angle;
        }
    }

}
