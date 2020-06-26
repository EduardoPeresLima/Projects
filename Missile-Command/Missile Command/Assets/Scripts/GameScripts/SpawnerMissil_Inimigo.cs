using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerMissil_Inimigo : MonoBehaviour
{
    [SerializeField] private GameObject enemymissilePrefab;
    [SerializeField] private float Ypadding = 0.5f;
    private float minX, maxX;
    public int missilesToSpawnThisRound = 10;
    public float delayEntreMisseis = .5f;
    private float Yvalue, randomX;
    // Start is called before the first frame update
    void Awake() {
        minX = Camera.main.ViewportToWorldPoint(new Vector3(0,1,0)).x;
        maxX = Camera.main.ViewportToWorldPoint(new Vector3(1,1,0)).x;
        Yvalue = Camera.main.ViewportToWorldPoint(new Vector3(0,1,0)).y;
        //StartCoroutine(SpawnMissiles());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void startSpawn(){
        StartCoroutine(SpawnMissiles());
    }
    public IEnumerator SpawnMissiles(){
        while(missilesToSpawnThisRound>0){
            randomX =  Random.Range(minX, maxX);
            Instantiate(enemymissilePrefab, new Vector3(randomX, Yvalue + Ypadding, 0),Quaternion.identity);
            --missilesToSpawnThisRound;
            yield return new WaitForSeconds(delayEntreMisseis);
        }
    }
}
