using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Data/GameData")]
public class GameData : ScriptableObject
{
    [SerializeField] Vector3 playerLocation;

    [SerializeField] PokemonParty party;

    public Vector3 PlayerLocation { get { return playerLocation; } set { playerLocation = value; } }

    public PokemonParty PokemonParty { get { return party; } set { party = value; } }
}
