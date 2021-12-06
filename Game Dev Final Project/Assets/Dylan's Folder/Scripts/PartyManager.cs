using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{

    private GameManager gameManger; 

    PartyDisplaySet[] partySlots;

    int selectIndex;

    private void Awake()
    {
        gameManger = FindObjectOfType<GameManager>();
        Init();
        
    }

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        //We still need to initialize party pokemon into array

        //When the player clicks left or right, the index should move one,
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if(selectIndex - 1 < 0) //We reached the beginning of the party
            {
                selectIndex = partySlots.Length - 1;
            }
            else //Good to move
            {
                selectIndex--;
            }

            Debug.Log(selectIndex);
        }
        else if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            if(selectIndex + 1 > partySlots.Length - 1) //We reached end of the party
            {
                selectIndex = 0;
            }
            else
            {
                selectIndex++;
            }
            Debug.Log(selectIndex);
        }

        //If they click up or down, the index should move two
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if(selectIndex - 2 < 0) //We reached the top of party
            {
                if (selectIndex % 2 == 0) //If it's even
                {
                    selectIndex = partySlots.Length - 2; //Bottom of Left Row
                }
                else
                {
                    selectIndex = partySlots.Length - 1; //Bottom of right Row
                }
            }
            else //Good to go
            {
                selectIndex-= 2;
            }
            Debug.Log(selectIndex);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (selectIndex + 2 > partySlots.Length - 1) //We reached the bottom
            {
                if (selectIndex % 2 == 0) //If it's even
                {
                    selectIndex = 0; //Bottom of Left Row
                }
                else
                {
                    selectIndex = 1; //Bottom of right Row
                }
            }
            else //Good to go
            {
                selectIndex += 2;
            }
            Debug.Log(selectIndex);
        }

        //Testing to see if The highlight boxes will pop up
        foreach(PartyDisplaySet i in partySlots)
        {
            if( System.Array.IndexOf(partySlots, i) == selectIndex)
            {
                i.setHighlightPokemon(true); //We highlight the pokemon
            }
            else
            {
                i.setHighlightPokemon(false); //We make sure it isn't highlighted
            }
        }
    }

    public void Init()
    {
        partySlots = GetComponentsInChildren<PartyDisplaySet>();
        
        //We can use this index to put the main party pokemon into the array
        //int transferIndex = 0;
        //partySlots = GetComponentsInChildren<PartyDisplaySet>();
        //foreach(PartyDisplaySet pokeDisplay in partySlots)
        //{
        //    pokeDisplay.partyPokemon = gameManger.GetComponent<PokemonParty>().partyList[transferIndex];
        //    transferIndex++;
        //}
    }
}
