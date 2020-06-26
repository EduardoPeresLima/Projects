using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    private GameController gameController;

    [SerializeField] GameObject missile;
    [SerializeField] GameObject missileLauncher;
    
    [SerializeField] private Texture2D[] cursorTexture;
    private MyGameManager gameManager;
    private Vector2 cursorHotspot;
    private int id=0;
    
    AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {   
        audio = GetComponent<AudioSource>();
        gameManager =  GameObject.FindObjectOfType<MyGameManager>();
        id = gameManager.GetCursorId();

        gameController = GameObject.FindObjectOfType<GameController>();
        cursorHotspot = new Vector2(cursorTexture[id].width/2f, cursorTexture[id].height/2f);
        Cursor.SetCursor(cursorTexture[id], cursorHotspot, CursorMode.Auto);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && gameController.canUseMissile()){
            audio.Play();
            Instantiate(missile,missileLauncher.transform.position,Quaternion.identity);
            gameController.useMissile();
        }
    }
}
