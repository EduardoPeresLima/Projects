using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGameManager : MonoBehaviour
{
    //Moedas
    private int playerCoins=0;

    //Melhorias
    private int coinsPerHouse = 20;
    private int deltaCoinsPerHouse = 20;
    [SerializeField] Sprite[] greenHouseSprite, redHouseSprite;
    private int[] housesPrices;
    private int housesId = 1;


    private float inicialSpeedPlayer = 5f;
    private float deltaSpeedPlayer = 1f;
    [SerializeField] Sprite[] speedSprite;
    private int[] speedPrices;
    private int speedId = 0;


    private float inicialRadius = 0f;
    private float deltaRadius = 0.5f;//Aumentanto o raio, aumenta a área 
    [SerializeField] Sprite[] areaSprite;
    private int[] areaPrices;
    private int areaId = 0;

    //Cursor
    private int cursorId = 0;
    MainMenuManager menuManager; 

    //Pontuação
    public class highScoreEntry{
        public int score;
        public string name;
    }
    private List<highScoreEntry> entryList; 
    private int sizeHighScoreList= 0;
    private int maxSizeHighScore = 5;
    
    //Audio
    AudioSource audio;

    //Padrão de Projeto Singleton
    public static MyGameManager instance;
    private void Awake() {
        if(instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else{
            Destroy(gameObject);
        }   
    }
    
    // Start is called before the first frame update
    void Start()
    {
        menuManager = GameObject.FindObjectOfType<MainMenuManager>();
        audio = gameObject.GetComponent<AudioSource>();
        entryList = new List<highScoreEntry>();
        
        housesPrices = new int[redHouseSprite.Length];
        speedPrices = new int[speedSprite.Length];
        areaPrices = new int[areaSprite.Length];
        for(int i=0;i<redHouseSprite.Length;++i){
            housesPrices[i] = 400 * (i+1);
        }
        for(int i=0;i<speedSprite.Length;++i){
            speedPrices[i] = 250 * (i+1);
        }
        for(int i=0;i<areaSprite.Length;++i){
            areaPrices[i] = 100 * (i+1);
        }
        /*
        entryList = new List<highScoreEntry>(){
            new highScoreEntry{score = 200, name = "A"},
            new highScoreEntry{score = 100, name = "B"},
            new highScoreEntry{score = 100, name = "C"},
            new highScoreEntry{score = 300, name = "D"},
            new highScoreEntry{score = 150, name = "E"}
        };
        */
    }
    /*
        CONTROLE DAS
        VARIAVEIS INICIAIS
        MELHORADAS
    */
    public int getCoinsPerHouse(){
        return coinsPerHouse;
    }
    public float getInicialSpeedPlayer(){
        return inicialSpeedPlayer;
    }
    public float getInicialRadius(){
        return inicialRadius;
    }

    /*
        CONTROLE
        DA
        PONTUAÇÃO
    */
    public int getSizeHighScoreList(){
        return sizeHighScoreList;
    }
    public string getName(int i){
        return entryList[i].name;
    }
    public int getScore(int i){
        return entryList[i].score;
    }
    public bool isThisNewHighScore(int newScore){
        Debug.Log(sizeHighScoreList + "  ->  " + maxSizeHighScore);
        return true;
        if(sizeHighScoreList<maxSizeHighScore)return true;
        else{
            return newScore > entryList[sizeHighScoreList-1].score;
        }
    }
    public void SetNewHighScore(int newScore, string newName){
        if(sizeHighScoreList<maxSizeHighScore){
            entryList.Add(new highScoreEntry(){score = -1, name = ""});
            ++sizeHighScoreList;
        }
        int id = -1;
        for(int i=sizeHighScoreList-1;i>=1;i--){
            entryList[i] = entryList[i-1];
            if(newScore<=entryList[i].score){
                id=i;
                break;
            }
        }
        if(id==-1)id=0;
        entryList[id] = new highScoreEntry(){score = newScore, name = newName};
    }

    /*
        CONTROLE
        DO
        CURSOR
    */
    public void SetCursorId(int newId){
        if(newId >=0 && newId <6){
            cursorId = newId;
        }
        else{
            Debug.Log("ID " + newId + " fora do range :(");
        }
    }
    public int GetCursorId(){
        return cursorId;
    }

    /*
        CONTROLE
        DE
        MELHORIAS
    */

    public bool canBuyHouse(){
        return housesPrices[housesId]<=playerCoins;
    }
    public bool canBuySpeed(){
        return speedPrices[speedId]<=playerCoins;
    }
    public bool canBuyArea(){
        return areaPrices[areaId]<=playerCoins;
    }

    public Sprite GetGreenHouseSprite(bool forTheGame){
        if(forTheGame) return greenHouseSprite[housesId-1];
        return greenHouseSprite[housesId];
    }
    public Sprite GetRedHouseSprite(bool forTheGame){
        if(forTheGame) return redHouseSprite[housesId-1];
        return redHouseSprite[housesId];
    }
    public Sprite GetSpeedSprite(){
        return speedSprite[speedId];
    }
    public Sprite GetAreaSprite(){
        return areaSprite[areaId];
    }

    public void houseBought(){
        playerCoins         -= housesPrices[housesId];
        coinsPerHouse       += deltaCoinsPerHouse;
        
        ++housesId;
    }
    public void speedBought(){
        playerCoins         -= speedPrices[speedId];
        inicialSpeedPlayer  += deltaSpeedPlayer;
        
        ++speedId;
    }
    public void areaBought(){
        playerCoins         -= areaPrices[areaId];
        inicialRadius       += deltaRadius;

        ++areaId;
    }

    public int getHousesId(){
        return housesId;
    }
    public int getSpeedId(){
        return speedId;
    }
    public int getAreaId(){
        return areaId;
    }

    public int getHousesPrice(){
        return housesPrices[housesId];
    }
    public int getSpeedPrice(){
        return speedPrices[speedId];
    }
    public int getAreaPrice(){
        return areaPrices[areaId];
    }

    public bool canShowHouses(){
        return housesId < redHouseSprite.Length;
    }
    public bool canShowSpeed(){
        return speedId < speedSprite.Length;
    }
    public bool canShowArea(){
        return areaId < areaSprite.Length;
    }
    /*
        CONTROLE
        DE
        MOEDAS
    */

    public int getCoins(){
        return playerCoins;
    }
    public void addCoins(int deltaCoins){
        Debug.Log(playerCoins + " + " + deltaCoins + " = " + (playerCoins+deltaCoins));
        if(deltaCoins>=0)playerCoins+=deltaCoins;
    }

    public void setVolume(float newVolume){
        audio.volume = newVolume;
    }
}
