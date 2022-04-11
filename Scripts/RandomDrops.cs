using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RandomDrops : MonoBehaviour
{
    public GameObject medkitPrefab;
    public GameObject bulletPrefab;
    public int mapHeight;
    public int mapWidth;
    public float MedkitSpawnTime;
    private float MedKitElapedTime = 0.0f;
    public float bulletSpawnTime;
    private float bulletElapedTime = 0.0f;
    public int maxItems = 30;
    private int ItemCount;

    void Update()
    {
        MedKitElapedTime += Time.deltaTime;
        bulletElapedTime += Time.deltaTime;

        if (MedKitElapedTime > MedkitSpawnTime && ItemCount <= maxItems)
        {
            MedKitElapedTime = 0.0f;
            SpawnMedkit();
        }

        if (bulletElapedTime > bulletSpawnTime && ItemCount <= maxItems)
        {
            bulletElapedTime = 0.0f;
            SpawnSpitballs();
        }

    }

    public void SpawnMedkit()
    {
        Vector3 spawnPos = new Vector3(Random.Range(mapWidth / 2, (-mapWidth) / 2), Random.Range(mapHeight / 2, (-mapHeight) / 2), 0);
        GameObject medkit = PhotonNetwork.Instantiate(medkitPrefab.name, spawnPos, Quaternion.identity);
        ItemCount += 1;
    }  

    public void SpawnSpitballs()
    {
        Vector3 spawnPos = new Vector3(Random.Range(mapWidth / 2, (-mapWidth) / 2), Random.Range(mapHeight / 2, (-mapHeight) / 2), 0);
        GameObject Spitballs = PhotonNetwork.Instantiate(bulletPrefab.name, spawnPos, Quaternion.identity);
        ItemCount += 1;
    }


}