using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class MultiplayerMovement : MonoBehaviour
{
    
    private PhotonView PV;
    public float speed = 5f;
    public Rigidbody2D rb;
    private GameObject camObj;
    Vector2 movement;
    Vector2 mousePos;
    Vector3 PlayerPos;
    public SpriteRenderer sprite;
    public float magnitude;
    public float duration;
    private GameObject GameManager;
    private TextMeshProUGUI bulletText;
    private Image normalBulletIcon;
    private Image HypermordernBulletIcon;
    public GameObject NovaSputter;
    BulletManager bulletManager;
    public List<GameObject> Straws = new List<GameObject>();
    private float timer =  0f;
    public float MaxTimer = 1f;
    private Slider refillBar;
    private bool shouldUpdate = false;
    public float shootTimeMax = 0.2f;
    private float shootTime;
    public bool barEffects;
    public Arrow arrowScript;


    public void Awake()
    {
        PV = GetComponent<PhotonView>();

        this.gameObject.name = "Player " + PV.Owner.NickName;

        GameManager = GameObject.Find("GameManager");

        bulletText = GameObject.Find("BulletText").GetComponent<TextMeshProUGUI>();

        refillBar = GameManager.GetComponent<MainGameManager>().GetNovaSlider();

        normalBulletIcon = GameManager.GetComponent<MainGameManager>().GetNormalIcon();
        HypermordernBulletIcon = GameManager.GetComponent<MainGameManager>().GetHyperIcon();

        if (PV.Owner.IsMasterClient)
        {
            this.gameObject.tag = "Player 1";
            this.gameObject.GetComponent<RandomDrops>().enabled = true;
            GameManager.GetComponent<MainGameManager>().playerOne = this.gameObject;
        }
        else
        {
            this.gameObject.tag = "Player 2";
            GameManager.GetComponent<MainGameManager>().playerTwo = this.gameObject;
        }
    }

    void Start()
    {
      arrowScript.enabled = true;
      GameObject camRef = GameManager.GetComponent<MainGameManager>().GetCamera();
      camObj = GameObject.Instantiate(camRef, camRef.transform.position, Quaternion.identity);
      camObj.GetComponent<CameraPV>().target = this.gameObject;

      UpdateBulletText(); 


      if (!PV.IsMine)
      {
         Destroy(camObj);
         Destroy(camRef);
      }

      camObj.AddComponent<AudioListener>();

      sprite.sortingOrder = 3;

      bulletManager = new BulletManager();

      bulletManager.hyperModernBullets = 5;
      bulletManager.normalBullets = 20;

    }

    void Update()
    {
        if (!PV.IsMine)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            StartCoroutine(camObj.GetComponent<CameraShake>().Shake(duration, magnitude));
            ShootWithWeapon();
        }

        if(NovaSputter.activeSelf && bulletManager.hyperModernBullets <= 0)
        {
            Novabar();
        }

        UpdateBulletText();

        if(bulletManager.normalBullets > 0)
        {
            ShootRepoblower();
        }

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        mousePos = camObj.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, 4f));

        if (NovaSputter.activeSelf)
        {
            normalBulletIcon.enabled = false;
            HypermordernBulletIcon.enabled = true;
        }
        else
        {
            normalBulletIcon.enabled = true;
            HypermordernBulletIcon.enabled = false;
        }
       
    }

    void FixedUpdate()
    {
    	if(PV.IsMine)
    	{
            Move();
        }
        
    }

    void Move()
    {

        if(GameManager.GetComponent<MainGameManager>().GetStopActivity() == false)
        {
            rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
        }

        Vector2 lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 270;
        rb.rotation = angle;
    }

    void ShootWithWeapon()
    {
        if(Straws[0].activeSelf)
        {
            if (PV.IsMine)
            {
                if (bulletManager.normalBullets > 0)
                {
                    Straws[0].GetComponent<Strawstol>().Shoot();
                    bulletManager.normalBullets -= 1;
                }
            }
        }

        else if (Straws[1].activeSelf)
        {
            if (PV.IsMine)
            {
                if (bulletManager.normalBullets > 0)
                {
                    Straws[1].GetComponent<Strawstol>().Shoot();
                    bulletManager.normalBullets -= 1;
                }
            }
        }

        else if(Straws[3].activeSelf)
        {
            if (PV.IsMine)
            {
                if (bulletManager.hyperModernBullets > 0)
                {
                    Straws[3].GetComponent<Strawstol>().Shoot();
                    bulletManager.hyperModernBullets -= 1;
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "SpitballPickup")
        {
            bulletManager.normalBullets += 5;
        }
    }

    public void UpdateBulletText()
    {
        if (PV.IsMine)
        {
            if (!NovaSputter.activeSelf)
            {
                bulletText.text = bulletManager.normalBullets.ToString();
            }
            else
            {
                bulletText.text = bulletManager.hyperModernBullets.ToString();
            }
        }
    }

    public void Novabar()
    {
        if (Input.GetMouseButton(0))
        {
            shouldUpdate = false;

            this.gameObject.GetComponent<PlayerHealth>().vulnerability *= 1.5f;
            barEffects = true;

            timer += Time.deltaTime;

            refillBar.gameObject.SetActive(true);
            refillBar.value = timer / MaxTimer;

            if (timer >= MaxTimer)
            {
                timer = 0f;

                if (barEffects)
                {
                    this.gameObject.GetComponent<PlayerHealth>().vulnerability = this.gameObject.GetComponent<PlayerHealth>().vulnerability * (1f / 1.5f);
                    barEffects = false;
                }

                refillBar.value = 0f;
                refillBar.gameObject.SetActive(false); ;
                bulletManager.hyperModernBullets = 5;
            }
        }

        else
        {
            if (shouldUpdate)
            {
                timer -= Time.deltaTime;
                refillBar.value = timer / MaxTimer;

                if (timer <= 0f)
                {
                    if (barEffects)
                    {
                        this.gameObject.GetComponent<PlayerHealth>().vulnerability = this.gameObject.GetComponent<PlayerHealth>().vulnerability * (1f / 1.5f);
                        barEffects = false;
                    }

                    timer = 0f;
                    refillBar.value = 0f;
                    refillBar.gameObject.SetActive(false);
                    shouldUpdate = false;
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            shouldUpdate = true;

            if (barEffects)
            {
                this.gameObject.GetComponent<PlayerHealth>().vulnerability = this.gameObject.GetComponent<PlayerHealth>().vulnerability * (1f / 1.5f);
                barEffects = false;
            }
        }

     }

    public void ShootRepoblower()
    {
        if (Straws[2].activeSelf)
        {
            if (Input.GetMouseButton(0))
            {
                shootTime += Time.deltaTime;

                if(shootTime > shootTimeMax)
                {
                    shootTime = 0f;
                    Straws[2].GetComponent<Strawstol>().Shoot();
                    bulletManager.normalBullets -= 1;
                }
            }
        }
    }
}
