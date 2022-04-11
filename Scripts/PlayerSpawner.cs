using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject[] playerPrefabs;
    public Transform[] spawnPoints;

    private void Start()
    {
    	int randnum = Random.Range(0 , spawnPoints.Length);
    	Transform spawnPoint = spawnPoints[randnum];
    	GameObject playerToSpawn;
    	if(PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"] == null)
    	{
    		playerToSpawn = playerPrefabs[0];
    	}
    	else
    	{
    		playerToSpawn = playerPrefabs[(int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"]];
    	}

        PhotonNetwork.Instantiate(playerToSpawn.name, spawnPoint.position, Quaternion.identity);
    }
}
