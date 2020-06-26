using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missil_Inimigo : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private GameObject missilePrefab;
    GameObject[] defenders;
    private GameController gameController;
    Vector3 target;
    private float randomTimer;
    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.FindObjectOfType<GameController>();
        speed = gameController.enemyMissileSpeed;

        defenders = GameObject.FindGameObjectsWithTag("Defenders");
        target = defenders[Random.Range(0,defenders.Length)].transform.position;
        target.y-=0.1f;

        randomTimer = Random.Range(.1f,gameController.getEndTimerRange());
        Invoke("splitMissile",randomTimer);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, target, speed*Time.deltaTime);
        if(transform.position == target){
            gameController.enemyMissileDestroyed(false);
            missileExplode();
        }
    }
    private void OnTriggerEnter2D(Collider2D collider){
        if(collider.tag == "Defenders"){
            //false -> Missil inimigo **não** foi destruído por um míssil do jogador
            missileExplode();
            gameController.enemyMissileDestroyed(false);
            if(collider.GetComponent<Town>()!= null){
                gameController.MissileLauncherHit();
            }
            else{
                Destroy(collider.gameObject);
                gameController.HouseHit();

            }
            //Debug.Log("Missil Inimigo atingiu um Defensor");
        }
        else if(collider.tag == "Explosions"){
            //true -> Missil inimigo foi destruído por um míssil do jogador
            gameController.enemyMissileDestroyed(true);
            missileExplode();
        }
    }
    private void splitMissile(){
        float Yvalue = Camera.main.ViewportToWorldPoint(new Vector3(0,.25f,0)).y;
        if(transform.position.y >= Yvalue){
            gameController.addEnemyMissile();
            Instantiate(missilePrefab, transform.position,Quaternion.identity);
        }
    }
    private void missileExplode(){
        Instantiate(explosionPrefab,transform.position,Quaternion.identity);
        Destroy(gameObject);
    }
}
