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

    public void ResetData()
    {
        saveData.PlayerLocation = new Vector3(0.5f, 0.5f, 0f);

        foreach (Pokemon p in saveData.PokemonParty.partyList)
            p.HP = p.MaxHP;
    }
}
