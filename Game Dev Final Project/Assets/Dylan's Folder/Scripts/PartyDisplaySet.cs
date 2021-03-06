using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PartyDisplaySet : MonoBehaviour
{
    //This script will put a Pokemon's info into the Pokemon UI Block
    //Base Values
    //Do we need these if we have the pokemon?

    //This pokemon is set up by the party manager
    public Pokemon partyPokemon;

    //We turn this objecto on and off if gameObject is selected
    public GameObject highlight;

    //We will highlight this green if gameObject is being swapped
    public GameObject selectingHighlight;

    //This is a block for if the gameobject is empty
    public GameObject emptyBlock;

    //Display variables
    public TextMeshProUGUI textName;
    public TextMeshProUGUI textHP;
    public TextMeshProUGUI textLV;
    public Image imagePoke;
    public Image imageHP;

    //We intialize all the variables for the display
    //Can't put this in
    private void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //We turn on or off the highlight depending on the bool recieved
    public void SetHighlightPokemon(bool isActive)
    {
        highlight.SetActive(isActive);
    }
    
    //We turn on or off the green select highlight depending on the bool recieved
    public void SetSelectedPokemon(bool isActive)
    {
        selectingHighlight.SetActive(isActive);
    }


    //We can use this to quickly turn on or off a Empty Pokemon Display Block
    public void TurnOffDisplay (bool isActive)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(isActive);
        }

        emptyBlock.SetActive(!isActive);

    }

    public void Init()
    {
        if (partyPokemon.Base != null) //If we have a pokemon to slot
        {

            //partyPokemon.Init();
            textName.text = partyPokemon.Name;
            textHP.text = partyPokemon.HP + "/ " + partyPokemon.MaxHP;
            textLV.text = "LV: " + partyPokemon.Level;

            imagePoke.sprite = partyPokemon.FrontSprite;

            //We are setting the HP bar to be a fraction of the full HP 
            imageHP.fillAmount = ((float)partyPokemon.HP / (float)partyPokemon.MaxHP);
        }
        else //Party Slot is empty
        {
            TurnOffDisplay(false);
        }
    }

}
