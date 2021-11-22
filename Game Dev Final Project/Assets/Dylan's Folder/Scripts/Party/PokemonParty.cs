using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonParty : MonoBehaviour
{
   [SerializeField] public List<Pokemon> partyList;

    private void Start()
    {
        foreach(var pokemon in partyList)
        {
            pokemon.Init();
        }
    }
}
