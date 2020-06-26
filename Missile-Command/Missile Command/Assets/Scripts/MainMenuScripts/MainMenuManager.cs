using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class MainMenuManager : MonoBehaviour
{
    //Instancias de Outras Classes Criadas
    private MyGameManager gameManager;

    AudioSource audio;
    //Moedas
    private int coins;
    [SerializeField] private TextMeshProUGUI coinsText;
    //Configurações
    [SerializeField] private GameObject configScreen;
    //High Score
    [SerializeField] private GameObject         highScoreCanvas;
    [SerializeField] private TextMeshProUGUI[]  scores, names;

    //Shopping
    [SerializeField] private GameObject         shoppingCanvas, speedPanel, areaPanel, housePanel ;
    [SerializeField] private Image              speedUpdImage, areaUpdImage, houseUpdImage;
    [SerializeField] private TextMeshProUGUI    levelSpeed, levelArea, levelHouse;

    //Cursor
    [SerializeField] private Texture2D[]    cursorTexture;
    private Vector2                         cursorHotspot;
    private int                             cursorId = 0;
    [SerializeField] TMP_Dropdown           selectCursor;

    //

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindObjectOfType<MyGameManager>();
        
        audio = GetComponent<AudioSource>();
        coins = gameManager.getCoins();
        updateCoinsText();

        cursorId = gameManager.GetCursorId();
        SetNewCursor();
        
        selectCursor.value = cursorId;

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)){
            audio.Play();
        }
    }
    //MOEDAS
    private void updateCoinsText(){
        coins = gameManager.getCoins();
        coinsText.text = "Moedas: " + coins;
    }
    /* 
        BOTAO
        START
        GAME
    */
    public void startGame(){
        SceneManager.LoadScene("Game");
    }

    /*
        BOTAO
        DE
        CONFIGURAÇÃO
    */
    public void alteraVolume(float newVolume){
        gameManager.setVolume(newVolume);
    }
    /* 
        BOTAO
        HIGH
        SCORE
    */
    public void showHighScores(){
        setAllOff();
        highScoreCanvas.gameObject.SetActive(true);
        displayHighScores();
    }
    private void displayHighScores(){
        for(int i=gameManager.getSizeHighScoreList()-1;i>=0;--i){
            names[i].text =  gameManager.getName(i);
            scores[i].text = gameManager.getScore(i).ToString();
        }
    }
    /*
        BOTAO
        LOJA
    */
    public void openShoppingScreen(){
        setAllOff();
        showHouseUpd();
        showSpeedUpd();
        //showAreaUpd();

        shoppingCanvas.SetActive(true);
    }
    /*
        COMPRAS
        DE
        MELHORIAS
    */
    // Casas
    public void houseUpdBought(){
        bool canBuy = gameManager.canBuyHouse();
        if(canBuy){
            gameManager.houseBought();
            updateCoinsText();
            showHouseUpd();
        }
    }
    public void speedUpdBought(){
        bool canBuy = gameManager.canBuySpeed();
        if(canBuy){
            gameManager.speedBought();
            updateCoinsText();
            showSpeedUpd();
        }
    }
    public void areaUpdBought(){
        bool canBuy = gameManager.canBuyArea();
        if(canBuy){
            gameManager.areaBought();
            updateCoinsText();
            showAreaUpd();
        }
    }

    private void showHouseUpd(){
        if(gameManager.canShowHouses()){
            levelHouse.text = "Upgrade "+(gameManager.getHousesId() + 1) + "\n";
            levelHouse.text += "Preço: "+ gameManager.getHousesPrice();
            houseUpdImage.sprite = gameManager.GetRedHouseSprite(false);
            housePanel.SetActive(true);
        }
        else{
            housePanel.SetActive(false);
        }
    }
    private void showSpeedUpd(){
        if(gameManager.canShowSpeed()){
            levelSpeed.text = "Upgrade "+(gameManager.getSpeedId() + 1) + "\n";
            levelSpeed.text += "Preço: "+ gameManager.getSpeedPrice();
            speedUpdImage.sprite = gameManager.GetSpeedSprite();
            speedPanel.SetActive(true);
        }
        else{
            speedPanel.SetActive(false);
        }
    }
    private void showAreaUpd(){
        if(gameManager.canShowArea()){
            levelArea.text = "Upgrade "+(gameManager.getAreaId() + 1) + "\n";
            levelArea.text += "Preço: "+ gameManager.getAreaPrice();
            areaUpdImage.sprite = gameManager.GetAreaSprite();
            areaPanel.SetActive(true);
        }
        else{
            areaPanel.SetActive(false);
        }
    }
    

    // CURSOR
    public void SetCursorId(int newId){
        gameManager.SetCursorId(newId);
        if(newId >=0 && newId <6){
            cursorId = newId;
            SetNewCursor();
        }
        else{
            Debug.Log("ID " + newId + " fora do range :(");
        }
    }
    private void SetNewCursor(){
        cursorHotspot = new Vector2(cursorTexture[cursorId].width/2f, cursorTexture[cursorId].height/2f);
        Cursor.SetCursor(cursorTexture[cursorId], cursorHotspot, CursorMode.Auto);
    }

    //Others
    private void setAllOff(){
        shoppingCanvas.SetActive(false);
        configScreen.SetActive(false);
        highScoreCanvas.SetActive(false);
    }
}
