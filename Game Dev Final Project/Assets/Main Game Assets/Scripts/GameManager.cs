using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameData saveData;

    public PokemonParty party;

    public static GameObject gameManager;

    private void Awake()
    {
        if (gameManager != null && gameManager != this)
        {
            Destroy(gameObject);
        }
        else
        {
            gameManager = gameObject;
        }

        DontDestroyOnLoad(gameManager);
    }

    public void SaveData()
    {
        saveData.PokemonParty = party;

        GameObject player = GameObject.Find("Player");
        if (player != null)
            saveData.PlayerLocation = player.transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
