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

    //Display variables
    public TextMeshProUGUI textName;
    public TextMeshProUGUI textHP;
    public TextMeshProUGUI textLV;
    public Image imagePoke;
    public Image imageHP;

    //We intialize all the variables for the display
    private void Awake()
    {
        //partyPokemon.Init();
        //textName.text = partyPokemon.Name;
        //textHP.text = partyPokemon.HP + "/ " + partyPokemon.MaxHP;
        //textLV.text = "LV: " + partyPokemon.Level;

        //imagePoke.sprite = partyPokemon.FrontSprite;

        ////We are setting the HP bar to be a fraction of the full HP 
        //imageHP.fillAmount = ((float)partyPokemon.HP/ (float)partyPokemon.MaxHP); 

    }
    // Start is called before the first frame update
    void Start()
    {
        partyPokemon.Init();
        textName.text = partyPokemon.Name;
        textHP.text = partyPokemon.HP + "/ " + partyPokemon.MaxHP;
        textLV.text = "LV: " + partyPokemon.Level;

        imagePoke.sprite = partyPokemon.FrontSprite;

        //We are setting the HP bar to be a fraction of the full HP 
        imageHP.fillAmount = ((float)partyPokemon.HP / (float)partyPokemon.MaxHP);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //We turn on or off the highlight depending on the bool recieved
    public void setHighlightPokemon(bool isActive)
    {
        highlight.SetActive(isActive);
    }


}
