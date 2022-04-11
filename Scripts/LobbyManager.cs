using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public TMP_InputField RoomInput;
    public GameObject lobbyPanel;
    public GameObject RoomPanel;
	public TextMeshProUGUI roomName;

    public RoomItem roomItemPrefab;
    List<RoomItem> roomItemList = new List<RoomItem>();
    public Transform contantObj;

    public float timeBetweenUpdates = 1.5f;
    float nextUpdateTime;

    public List<PlayerItem> PlayerItemList = new List<PlayerItem>();
    public PlayerItem PlayerItemPrefab;
    private PlayerItem PlayerItemScript;
    public Transform PlayerItemParent;

    public GameObject playerButton;
    public Image playerAvatar;
    public bool IsPlayerSelected;
    public Sprite None;
    public GameObject[] playerChar;
    RoomOptions ro;
    public Image fade;
    public Canvas main;


    private void Start()
	{
       PhotonNetwork.JoinLobby();
    }

  
    public void OnClickCreate()
    {
    	if(RoomInput.text.Length > 0)
    	{
            ro = new RoomOptions() { MaxPlayers = 2, BroadcastPropsChangeToAll = true, IsOpen = true, IsVisible = true };
    		PhotonNetwork.CreateRoom(RoomInput.text, ro);
    	}
    }

    public override void OnJoinedRoom()
    {
    	lobbyPanel.SetActive(false);
        RoomPanel.SetActive(true);
        roomName.text = "Room Name: " + PhotonNetwork.CurrentRoom.Name;
        UpdatePlayerList();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if(Time.time >= nextUpdateTime)
        {
            UpdateRoomList(roomList);
            nextUpdateTime = Time.time + timeBetweenUpdates;
        }
    }

    void UpdateRoomList(List<RoomInfo> list)
    {
        foreach(RoomItem item in roomItemList)
        {
            Destroy(item.gameObject);
        }
        roomItemList.Clear();

        foreach(RoomInfo room in list)
        {
            if (room.RemovedFromList)
            {
                return;
            }
            RoomItem newRoom = Instantiate(roomItemPrefab, contantObj);
            newRoom.SetRoomName(room.Name);
            roomItemList.Add(newRoom);
        }
    } 

    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    public void OnClickLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

     public override void OnLeftRoom()
    {
        RoomPanel.SetActive(false);
        lobbyPanel.SetActive(true);
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    void UpdatePlayerList()
    {
        foreach(PlayerItem item in PlayerItemList)
        {
            Destroy(item.gameObject);
        }
        PlayerItemList.Clear();

        if(PhotonNetwork.CurrentRoom == null)
        {
            return;
        }

        foreach(KeyValuePair<int,Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            PlayerItem newPlayerItem = Instantiate(PlayerItemPrefab, PlayerItemParent);
            newPlayerItem.SetPlayerInfo(player.Value);
            if(player.Value == PhotonNetwork.LocalPlayer)
            {
                newPlayerItem.ApplyLocalChanges();
            }
            PlayerItemList.Add(newPlayerItem);
        }
    }


    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerList();
    }
    

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerList();
    }

    private void Update()
    {

        if(PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount >= 2 )
        {
            playerButton.SetActive(true);
        }
        else
        {
            playerButton.SetActive(false);
        }
    }

    public void OnCLickPlayButton()
    {
        if (!(PhotonNetwork.IsMasterClient))
        {
            ro.IsOpen = false;
            ro.IsVisible = false;
            main.enabled = false;
            fade.enabled = true;
            fade.gameObject.GetComponent<Animator>().SetTrigger("Fade");
        }
        else
        {
            main.enabled = false;
            fade.gameObject.GetComponent<Animator>().SetTrigger("Fade");
        }

        ro.IsOpen = false;
        ro.IsVisible = false;

        StartCoroutine(WaitForFade());

        PhotonNetwork.LoadLevel("Game");
    }

    public IEnumerator WaitForFade()
    {
        yield return new WaitForSeconds(3);
    }


}    