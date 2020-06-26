using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosaoMissilAutoDestruir : MonoBehaviour
{
    [SerializeField] float tempoAteDestruir = 1f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject,tempoAteDestruir);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
