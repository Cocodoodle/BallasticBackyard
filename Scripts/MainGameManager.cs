using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainGameManager : MonoBehaviour
{
    public TextMeshProUGUI playerOneScoreText;
    public TextMeshProUGUI playerOneText;
    public TextMeshProUGUI playerTwoScoreText;
    public TextMeshProUGUI playerTwoText;
    public Slider P_Oneslider;
    public Slider P_Twoslider;
    private int P_OneScore = 0;
    private int P_TwoScore = 0;
    public int PlayerOneHealth = 100;
    public int PlayerTwoHealth = 100;
    private string PlayerOneName;
    private string PlayerTwoName;
    public bool StopActivity = false;
    public Image VictoryImage;
    public TextMeshProUGUI WinText;
    public TextMeshProUGUI CommentText;
    public List<string> commentList = new List<string>();
    public TextMeshProUGUI POneDamageTextPrefab;
    public TextMeshProUGUI PTwoDamageTextPrefab;
    private Animator POne_animation;
    private Animator PTwo_animation;
    public Animator POneScore_animation;
    public Animator PTwoScore_animation;
    public Canvas canvas;
    public GameObject SetPOneDmgPos;
    public GameObject SetPTwoDmgPos;
    public Slider NovaSlider;
    public List<Transform> respawnPoints = new List<Transform>();
    public Image normalBulletIcon;
    public Image HypermordernBulletIcon;
    public GameObject MainCamera;
    public GameObject playerOne;
    public GameObject playerTwo;


    public void Start()
    { 
        Dictionary<int, Photon.Realtime.Player> pList = Photon.Pun.PhotonNetwork.CurrentRoom.Players;
        foreach (KeyValuePair<int, Photon.Realtime.Player> p in pList)
        {
            playerOneScoreText.text = P_OneScore.ToString();
            playerTwoScoreText.text = P_TwoScore.ToString();

            if (p.Value.IsMasterClient)
            {
                playerOneText.text = p.Value.NickName;
                PlayerOneName = p.Value.NickName;
            }
            else
            {
                playerTwoText.text = p.Value.NickName;
                PlayerTwoName = p.Value.NickName;
            } 
        }
    }

    public void PlayerOneAddScore()
    {
        P_OneScore += 1;
        playerOneScoreText.text = P_OneScore.ToString();
        POneScore_animation.SetTrigger("Blink");
    }

    public void PlayerTwoAddScore()
    {
        P_TwoScore += 1;
        playerTwoScoreText.text = P_TwoScore.ToString();
        PTwoScore_animation.SetTrigger("Blink");
    }

    public void OnPOneHeathUpdate()
    {
        P_Oneslider.value = PlayerOneHealth;
    }

    public void OnPTwoHeathUpdate()
    {
        P_Twoslider.value = PlayerTwoHealth;
    }

    public void ResetHealth()
    {
        P_Oneslider.value = 100;
        P_Twoslider.value = 100;
        PlayerOneHealth = 100;
        PlayerTwoHealth = 100;
    }

    public int GetPOneHealth()
    {
        return PlayerOneHealth;
    }

    public int GetPTwoHealth()
    {
        return PlayerTwoHealth;
    }

    public void POneTakeDamage(int damage)
    {
        PlayerOneHealth -= damage;

}

    public void PTwoTakeDamage(int damage)
    {
        PlayerTwoHealth -= damage;
    }

    public void POneVictory()
    {
        VictoryImage.gameObject.SetActive(true);
        int length = commentList.Count;
        int index = Random.Range(0, length);
        CommentText.text = commentList[index];
        WinText.text = PlayerOneName + " Wins!";
        StopActivity = true;
    }

    public void PTwoVictory()
    {
        VictoryImage.gameObject.SetActive(true);
        int length = commentList.Count;
        int index = Random.Range(0, length);
        CommentText.text = commentList[index];
        WinText.text = PlayerTwoName + " Wins!";
        StopActivity = true;
    }

    public int GetPOneScore()
    {
        return P_OneScore;
    }

    public int GetPTwoScore()
    {
        return P_TwoScore;
    }

    public IEnumerator Disconnect()
    {
        PhotonNetwork.Disconnect();

        while (PhotonNetwork.IsConnected)
        {
            yield return null;
        }

        SceneManager.LoadScene("Username");
    }

    public void disconnect()
    {
        StartCoroutine(Disconnect());
    }

    public bool GetStopActivity()
    {
        return StopActivity;
    }

    public IEnumerator POneShowDamage(int damage)
    {
        TextMeshProUGUI POneDamageText = Object.Instantiate(POneDamageTextPrefab, SetPOneDmgPos.transform.position, Quaternion.identity, canvas.transform);
        POneDamageText.text = "-" + damage.ToString();
        POne_animation = POneDamageText.GetComponent<Animator>();
        POne_animation.SetTrigger("IsTakingDamage");

        yield return new WaitForSeconds(0.22f);

        Destroy(POneDamageText);

    }

    public IEnumerator PTwoShowDamage(int damage)
    {
        TextMeshProUGUI PTwoDamageText = Object.Instantiate(PTwoDamageTextPrefab, SetPTwoDmgPos.transform.position, Quaternion.identity, canvas.transform);
        PTwoDamageText.text = "-" + damage.ToString();
        PTwo_animation = PTwoDamageText.GetComponent<Animator>();
        PTwo_animation.SetTrigger("IsTakingDamage");

        yield return new WaitForSeconds(0.22f);

        Destroy(PTwoDamageText);
    }

    public Slider GetNovaSlider()
    {
        return NovaSlider;
    }

    public IEnumerator POneHeal(int amount)
    {
        TextMeshProUGUI POneDamageText = Object.Instantiate(POneDamageTextPrefab, SetPOneDmgPos.transform.position, Quaternion.identity, canvas.transform);

        if (PlayerOneHealth + amount > 100)
        {
            PlayerOneHealth = 100;
            amount = 100 - PlayerOneHealth;
        }
        else
        {
            PlayerOneHealth += amount;
        }

        POneDamageText.color = Color.green;
        POneDamageText.text = "+" + amount.ToString();
        POne_animation = POneDamageText.GetComponent<Animator>();
        POne_animation.SetTrigger("IsTakingDamage");

        yield return new WaitForSeconds(0.22f);

        Destroy(POneDamageText);

    }
    public IEnumerator PTwoHeal(int amount)
    {
        TextMeshProUGUI PTwoDamageText = Object.Instantiate(PTwoDamageTextPrefab, SetPTwoDmgPos.transform.position, Quaternion.identity, canvas.transform);

        if(PlayerTwoHealth + amount > 100)
        {
            PlayerTwoHealth = 100;
            amount = 100 - PlayerTwoHealth;
        }
        else
        {
            PlayerTwoHealth += amount;
        }


        PTwoDamageText.color = Color.green;
        PTwoDamageText.text = "+" + amount.ToString();
        PTwo_animation = PTwoDamageText.GetComponent<Animator>();
        PTwo_animation.SetTrigger("IsTakingDamage");

        yield return new WaitForSeconds(0.22f);

        Destroy(PTwoDamageText);
    }

    public Image GetNormalIcon()
    {
        return normalBulletIcon;
    }

    public Image GetHyperIcon()
    {
        return HypermordernBulletIcon;
    }

    public GameObject GetCamera()
    {
        return MainCamera;
    }
}
