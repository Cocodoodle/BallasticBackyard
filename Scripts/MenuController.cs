using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class MenuController : MonoBehaviourPunCallbacks
{
  [SerializeField] private GameObject UsernameMenu;
  
  [SerializeField] private TMP_InputField UsernameInput;
  [SerializeField] private GameObject StartButton;


    public void SetUsername()
    {
      if(UsernameInput.text.Length > 0)
      {
        StartButton.SetActive(true);
      }
      else
      {
        StartButton.SetActive(false);
      }

    }

    public void OnClickConnect()
    {
      PhotonNetwork.NickName = UsernameInput.text;
      PhotonNetwork.ConnectUsingSettings();
      PhotonNetwork.AutomaticallySyncScene = true;

    } 

    public override void OnConnectedToMaster()
    {
        SceneManager.LoadScene("Lobby");
    }

}
