using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    private PhotonView PV;
    private GameObject gameManager;
    private bool isPrintedOut = false;
    public float resistance = 1f;
    public float vulnerability = 1f;
    private List<Transform> RespwanPoints = new List<Transform>();
    public int healAmount = 10;
    public Color damageColor;
    public GameObject playerDeathEffectPrefab;
    public bool isInInactiveState = false;
    public SpriteRenderer spriteRenderer; 

    private void Awake()
    {
        PV = this.GetComponent<PhotonView>();
        gameManager = GameObject.Find("GameManager");
    }

    public void Start()
    {
        RespwanPoints = gameManager.GetComponent<MainGameManager>().respawnPoints;
    }


    void Update()
    {
        if(isPrintedOut == true)
        {
            return;
        }

        if (!PV.IsMine)
        {
            return;
        }

        if (PV.Owner.IsMasterClient)
        {
            if (gameManager.GetComponent<MainGameManager>().GetPOneScore() >= 5)
            {
                PV.RPC("DisplayPOneVictory", RpcTarget.All);
            }
        }
        else
        {
            if (gameManager.GetComponent<MainGameManager>().GetPTwoScore() >= 5)
            {
                PV.RPC("DisplayPTwoVictory", RpcTarget.All);
            }
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            PV.RPC("TakeDamage", RpcTarget.All, 25);
            PV.RPC("UpdateHealthBar", RpcTarget.All);
        }

        if (PV.Owner.IsMasterClient)
        {
           if(gameManager.GetComponent<MainGameManager>().GetPOneHealth() <= 0)
            {
                PV.RPC("ResetHealth", RpcTarget.All);
                PV.RPC("UpdateScore", RpcTarget.All);
                this.gameObject.GetComponent<Spitball>().GiveRandomWeapon();
            }
        }
        else
        {
            if (gameManager.GetComponent<MainGameManager>().GetPTwoHealth() <= 0)
            {
                PV.RPC("ResetHealth", RpcTarget.All);
                PV.RPC("UpdateScore", RpcTarget.All);
                this.gameObject.GetComponent<Spitball>().GiveRandomWeapon();
            }
        }
    }

    [PunRPC]
    void UpdateScore()
    {
        if (this.gameObject.tag == "Player 1")
        {
            gameManager.GetComponent<MainGameManager>().PlayerTwoAddScore();

        }
        else if (this.gameObject.tag == "Player 2")
        {
            gameManager.GetComponent<MainGameManager>().PlayerOneAddScore();

        }

    }

    [PunRPC]
    void UpdateHealthBar()
    {
        if(this.gameObject.tag == "Player 1")
        {
            gameManager.GetComponent<MainGameManager>().OnPOneHeathUpdate();
        }
        else if (this.gameObject.tag == "Player 2")
        {
            gameManager.GetComponent<MainGameManager>().OnPTwoHeathUpdate();
        }
    }

    [PunRPC]
    void ResetHealth()
    {
        gameManager.GetComponent<MainGameManager>().ResetHealth();
        GameObject.Instantiate(playerDeathEffectPrefab, transform.position, Quaternion.identity);

        isInInactiveState = true;
        Color color = spriteRenderer.color;
        color.a = 0f;

        StartCoroutine(Wait(3));

        isInInactiveState = false;
        color.a = 255f;

        MainGameManager mainGameManager = gameManager.GetComponent<MainGameManager>();

        if (PV.Owner.IsMasterClient)
        {
            this.transform.position = FindFarthestSpawnPoint(mainGameManager.playerTwo.transform).position;
        }
        else
        {
            this.transform.position = FindFarthestSpawnPoint(mainGameManager.playerTwo.transform).position;
        }
    }

    [PunRPC]
    void TakeDamage(int damage)
    {
        if (this.gameObject.tag == "Player 1")
        {
            gameManager.GetComponent<MainGameManager>().POneTakeDamage(damage);
            StartCoroutine(gameManager.GetComponent<MainGameManager>().POneShowDamage(damage));
            StartCoroutine(ColorChange());
        }
        else if (this.gameObject.tag == "Player 2")
        {
            gameManager.GetComponent<MainGameManager>().PTwoTakeDamage(damage);
            StartCoroutine(gameManager.GetComponent<MainGameManager>().PTwoShowDamage(damage));
            StartCoroutine(ColorChange());
        }
    }

    [PunRPC]
    void DisplayPOneVictory()
    {
        gameManager.GetComponent<MainGameManager>().POneVictory();
        isPrintedOut = true;
    }

    [PunRPC]
    void DisplayPTwoVictory()
    {
        gameManager.GetComponent<MainGameManager>().PTwoVictory();
        isPrintedOut = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "bullet")
        {
            int dmg = (int)Mathf.Round((collision.gameObject.GetComponent<SpitballBullet>().GetAttackDamage() / resistance) * vulnerability);

            if(dmg <= 0)
            {
                dmg = 1;
            }

            PV.RPC("TakeDamage", RpcTarget.All, dmg);
            PV.RPC("UpdateHealthBar", RpcTarget.All);
            
        }
        else if (collision.gameObject.tag == "medkit")
        {
            PV.RPC("Heal", RpcTarget.All, healAmount);
            PV.RPC("UpdateHealthBar", RpcTarget.All);
        }
    }

    [PunRPC]
    void Heal(int amount)
    {
        if (this.gameObject.tag == "Player 1")
        {
            StartCoroutine(gameManager.GetComponent<MainGameManager>().POneHeal(amount));
        }
        else if (this.gameObject.tag == "Player 2")
        {
            StartCoroutine(gameManager.GetComponent<MainGameManager>().PTwoHeal(amount));
        }
    }

    IEnumerator ColorChange()
    {
        Debug.Log("hee");
        SpriteRenderer spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.color = damageColor;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = new Color(255, 255, 255);
    }

    IEnumerator Wait(int sec)
    {
        yield return new WaitForSeconds(sec);
    }

    public bool GetInactiveBool()
    {
        return isInInactiveState;
    }
    
    public Transform FindFarthestSpawnPoint(Transform player)
    {
        Transform currentFarthestPoint = null;
        float currentFarthestDistance = 0f;

        foreach (Transform point in RespwanPoints)
        {
            if(Vector3.Distance(player.position, point.position) > currentFarthestDistance)
            {
                currentFarthestDistance = Vector3.Distance(player.position, point.position);
                currentFarthestPoint = point;
            }
        }

        return currentFarthestPoint;
    }

}

