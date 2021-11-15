using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PokemonDisplay : MonoBehaviour
{
    //This is the Pokemon we're displaying
    public PokemonBase currentPokemonBase;

    public Pokemon currentPokemon;

    //Storing UI Gameobjects
    public SpriteRenderer myfrontImage;
    public SpriteRenderer mybackImage;

    public TextMeshPro myName;
    public TextMeshPro myLV;
    public TextMeshPro myHP;
    public TextMeshPro mySpeed;
    public TextMeshPro myAttack;
    public TextMeshPro myDefense;
    public TextMeshPro mySpAttack;
    public TextMeshPro mySpDefense;

    // Start is called before the first frame update
    void Start()
    {
        currentPokemon = new Pokemon(currentPokemonBase, 12);
    }

    // Update is called once per frame
    void Update()
    {
        //Update Sprites
        myfrontImage.sprite = currentPokemonBase.FrontSprite;
        mybackImage.sprite = currentPokemonBase.BackSprite;
        
        //We will set all the values in Update so we can switch out the Pokemon;
        myName.text = currentPokemon.Name;
        myLV.text = "LV. " + currentPokemon.Level;
        mySpeed.text = "Speed: "+ currentPokemon.Speed;
        myHP.text = "HP: " + currentPokemon.MaxHP;
        myAttack.text = "Attack: " + currentPokemon.Attack;
        myDefense.text = "Defense: " + currentPokemon.Defense;
        mySpAttack.text = "SP Attack: " + currentPokemon.SpAttack;
        myDefense.text = "SP Defense: " + currentPokemon.SpAttack;

    }
}
