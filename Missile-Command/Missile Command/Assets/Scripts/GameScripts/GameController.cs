using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class GameController : MonoBehaviour
{
    private float frequency;
    private bool endForced = false;
    //Moedas
    [SerializeField] private TextMeshProUGUI gameCoinsText;
    [SerializeField] private TextMeshProUGUI totalGameCoinsText;

    private int coinsPerHouse = 0;
    private int gameCoins=0;
    
    //Inicio do Nivel;
    [SerializeField] private GameObject town;
    [SerializeField] private GameObject[] green_houses, red_houses;
    [SerializeField] private GameObject[] green_housesPrefab, red_housesPrefab;

    //Durante o Nível
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI coinsThisRound;
    [SerializeField] private TextMeshProUGUI missilesText;
    [SerializeField] private TextMeshProUGUI missilesInLauncherText;
    public int score = 0;
    public int level = 1;
    public int houseCounter = 0;
    public int currentMissilesLoadedInLauncher = 0;
    public int playerMissilesLeft = 30;
    
    //Inimigo
    SpawnerMissil_Inimigo           enemySpawner;
    public float                    enemyMissileSpeed = 2;
    [SerializeField] private float  enemyMissileSpeedMultiplier = 1.25f;
    public int                      enemyMissilesLeftInRound = 0;
    private int                     enemyMissilesThisRound = 20;
    [SerializeField] private int    enemyMissileDestroyedPoints = 25;
    private float                   endTimerRange = 100f;

    //Fim de Nivel
    [SerializeField] private GameObject      endOfRoundPanel;
    [SerializeField] private TextMeshProUGUI title; 
    [SerializeField] private TextMeshProUGUI leftOverMissileBonusText;
    [SerializeField] private TextMeshProUGUI leftOverHouseBonusText;
    [SerializeField] private TextMeshProUGUI totalBonusText;

    [SerializeField] private int    missileEndOfRoundPoints = 5;
    [SerializeField] private int    houseEndOfRoundPoints = 100;
    private bool                    isRoundOver = false;
    
    private bool                    waitForEnemy = true;
    //Fim de Jogo
    private bool isNewHighScore=false;
    private bool isGameOver = false;
    private MyGameManager gameManager;
    [SerializeField] private GameObject endOfGamePanel;
    [SerializeField] private TextMeshProUGUI maybeNewHighScore, nameText;
    [SerializeField] private TMP_InputField inputName;

    // Start is called before the first frame update
    void Start()
    {
        currentMissilesLoadedInLauncher = 10;
        playerMissilesLeft -= 10;

        gameManager = GameObject.FindObjectOfType<MyGameManager>();
        setHouses();
        coinsPerHouse = gameManager.getCoinsPerHouse();
        //Debug.Log(coinsPerHouse);

        enemySpawner = GameObject.FindObjectOfType<SpawnerMissil_Inimigo>();
        houseCounter = GameObject.FindObjectsOfType<House>().Length;
        updateLevelText();
        updateScoreText();
        updateMissilesLeftText();
        updateMissilesLeftInLauncherText();
        startRound();
        frequency = Time.fixedTime + 2.0f;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        //Debug.Log("b");
        if(Time.fixedTime >= frequency && !isRoundOver){
            if(endForced){
                isRoundOver = true;
                isNewHighScore = gameManager.isThisNewHighScore(score);
                StartCoroutine(endOfGame());
            }
            else{
                enemyMissilesLeftInRound = GameObject.FindObjectsOfType<Missil_Inimigo>().Length;
                houseCounter = GameObject.FindObjectsOfType<House>().Length ;
                if(waitForEnemy){
                    waitForEnemy = enemyMissilesLeftInRound == 0;
                    return;
                }
                Debug.Log(houseCounter);
                //Debug.Log(enemyMissilesLeftInRound);
                isRoundOver = enemyMissilesLeftInRound == 0;
                if(isRoundOver){
                    if(houseCounter <=0 ){
                        isNewHighScore = gameManager.isThisNewHighScore(score);
                        StartCoroutine(endOfGame());
                    }
                    else{
                        waitForEnemy = true;
                        StartCoroutine(endOfRound());
                    }
                }
            }
            frequency = Time.fixedTime + 2.0f;
        }
    }
    private void setHouses(){
        int id = gameManager.getHousesId()-1;
        float offsetGreen = 0f, offsetRed = 0f;
        Vector3 myV = new Vector3(0,0,0);
        if(id==1){
            offsetGreen = 0.17f;
            offsetRed = 0.19f;
        }
        else if(id==2){
            offsetGreen = 0.26f;
            offsetRed = 0.26f;
        }
        for(int i=0;i<green_houses.Length;++i){
            myV.x = green_houses[i].transform.position.x;
            myV.y = green_houses[i].transform.position.y    +   offsetGreen;
            Instantiate(green_housesPrefab[id],myV,Quaternion.identity);
            myV.x = red_houses[i].transform.position.x;
            myV.y = red_houses[i].transform.position.y      +   offsetRed;
            Instantiate(red_housesPrefab[id], myV,Quaternion.identity);
        }
    
    }
    //Começar Nível
    private void startRound(){
        enemySpawner.missilesToSpawnThisRound = enemyMissilesThisRound;
        enemyMissilesLeftInRound = enemyMissilesThisRound;
        enemySpawner.startSpawn();
    }
    //Utilizar Míssel do Jogador
    public bool canUseMissile(){
        if(isRoundOver) return false;
        return (currentMissilesLoadedInLauncher>0 && !isRoundOver);
    }
    public void useMissile(){
        --currentMissilesLoadedInLauncher;
        if(playerMissilesLeft>0 && currentMissilesLoadedInLauncher==0){
            currentMissilesLoadedInLauncher = Math.Min(10,playerMissilesLeft);
            playerMissilesLeft-=currentMissilesLoadedInLauncher;
            updateMissilesLeftText();
        }
        updateMissilesLeftInLauncherText();
    }

    //Míssel inimigo
    public void addEnemyMissile(){
        ++enemyMissilesLeftInRound;
    }
    public void enemyMissileDestroyed(bool destroyedByPlayerMissile){
        //Debug.Log(enemyMissilesLeftInRound);
        
        enemyMissilesLeftInRound = GameObject.FindObjectsOfType<Missil_Inimigo>().Length - 1;
        //--enemyMissilesLeftInRound;
        //Debug.Log(enemyMissilesLeftInRound);
        if(destroyedByPlayerMissile){
            addMissileDestroyedPoints();
        }
        //yield return new WaitForSeconds(0.1f);
    }
    public void addMissileDestroyedPoints(){
        score+=enemyMissileDestroyedPoints;
        updateScoreText();
    }
    private void decreaseEndTimerRange(){
        endTimerRange*=0.93f;
    }
    public float getEndTimerRange(){
        return endTimerRange;
    }

    //Casa destruída ou Torre atacada
    public void MissileLauncherHit(){
        if(playerMissilesLeft>0){
            currentMissilesLoadedInLauncher = Math.Min(10,playerMissilesLeft);
            playerMissilesLeft-=currentMissilesLoadedInLauncher;
            updateMissilesLeftText();
            updateMissilesLeftInLauncherText();
        }
        else {
            currentMissilesLoadedInLauncher = 0;
            updateMissilesLeftInLauncherText();
        }
    }
    public void HouseHit(){
        houseCounter = GameObject.FindObjectsOfType<House>().Length - 1;
        //Debug.Log(houseCounter);
    }
    
    //Fim do Nível
    public IEnumerator endOfRound(){
        yield return new WaitForSeconds(.5f);
        decreaseEndTimerRange();
        House[] houses = GameObject.FindObjectsOfType<House>();
        int leftOverHouseBonus = houses.Length* houseEndOfRoundPoints;
        int leftOverMissileBonus = (playerMissilesLeft+currentMissilesLoadedInLauncher) * missileEndOfRoundPoints;
        int totalBonus = leftOverHouseBonus + leftOverMissileBonus;
        int roundCoins = coinsPerHouse * houses.Length;
        gameCoins+=roundCoins;
        
        title.text                      =  "Nível " + level +" concluído!";
        leftOverHouseBonusText.text     =  "Bônus Extra por Casas Restantes:     " + leftOverHouseBonus;
        leftOverMissileBonusText.text   =  "Bônus Extra por Mísseis Restantes:   " + leftOverMissileBonus;
        totalBonusText.text="Bônus Total:                                        " + totalBonus;
        totalBonusText.text             += "  *  "+Math.Min(6,(level+1)/2)+"  =  " + totalBonus;
        coinsThisRound.text            =  "Moedas adquiridas neste round:        " + coinsPerHouse;
        coinsThisRound.text             += "  *  "+houses.Length          +"  =  " + roundCoins;

        totalBonus*=Math.Min(6,(level+1)/2); // (nivel, multiplicador) -> ((1,2),1), ((3,4),2), ((5,6),3), ... , (>12,6);
        score+=totalBonus;
        updateScoreText();
        updateCoinsText();
        endOfRoundPanel.SetActive(true);
    }
    public void startNextRound(){
        endOfRoundPanel.SetActive(false);
        isRoundOver = false;
        ++level;
        currentMissilesLoadedInLauncher = 10;
        playerMissilesLeft = 30 - 10;
        enemyMissileSpeed *= enemyMissileSpeedMultiplier;
        updateMissilesLeftText();
        updateMissilesLeftInLauncherText();
        updateLevelText();
        //Aumentar velocidade do míssil do inimigo aqui
        startRound();
    }

    //Atualiza Texto na Tela
    void updateMissilesLeftText(){
        missilesText.text = "Misseis: " + playerMissilesLeft;
    }
    void updateMissilesLeftInLauncherText(){
        missilesInLauncherText.text = "Misseis na Torre: " + currentMissilesLoadedInLauncher;
    }
    void updateScoreText(){
        scoreText.text = "Pontuação: " + score;
    }
    void updateLevelText(){
        levelText.text = "Nivel: " + level;
    }
    void updateCoinsText(){
        gameCoinsText.text = "Moedas adquiridas: " + gameCoins;
    }

    //Fim de Jogo
    public IEnumerator endOfGame(){
        currentMissilesLoadedInLauncher=0;
        playerMissilesLeft=0;
        yield return new WaitForSeconds(.5f);
        totalGameCoinsText.text = "Moedas Totais Adquiridas: " + gameCoins;
        gameManager.addCoins(gameCoins);
        if(isNewHighScore){
            maybeNewHighScore.text = "HIGH SCORE!!!\nParabéns, você entrou para o placar :)";
            nameText.gameObject.SetActive(true);
            inputName.gameObject.SetActive(true);
        }
        else{
            maybeNewHighScore.text = "Que pena, você não entrou para o placar\nContinue Tentando :)";
        }
        endOfGamePanel.SetActive(true);
    }
    public void submitNewScore(){
        nameText.gameObject.SetActive(false);
        inputName.gameObject.SetActive(false);
        if(isNewHighScore){
            gameManager.SetNewHighScore(score, inputName.text);
        }
        SceneManager.LoadScene("MainMenu");
    } 
    public void forceEnd(){
        Debug.Log("end");
        endForced = true;
    }
}
