using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{

    //Our GameManager
    private GameManager gameManager;

    //We store a copy of the GameManager's pokemon
    private List<Pokemon> partyPokemonCopy;

    //List of UI elements for Party Display
    PartyDisplaySet[] partySlots;

    //Players cursor index
    int selectIndex;

    //Boolean to see if we are in a swapping Pokemon state
    bool swapStarted;
    //First Swap index
    int swapOneIndex, swapTwoIndex;

    private void Awake()
    {
        selectIndex = 0;

        swapStarted = false;
        swapOneIndex = -1; //-1 is out of range, so don't have to worry about it
        swapTwoIndex = -1; //-1 is out of range, so don't have to worry about it

        gameManager = FindObjectOfType<GameManager>();
        partyPokemonCopy = gameManager.GetComponent<PokemonParty>().partyList;
        Init();
        
    }

    // Update is called once per frame
    void Update()
    {

        //Here, we take in player input to navigate the menu
        if(Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            HorizontalInput();
        }
        else if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            VerticalInput();
        }

        //We need a way for players to select pokemon and swap
        if(Input.GetKeyDown(KeyCode.Return))
        {
            //Enter swapping mode
            SwapPokemon();
            //This if statement should enter us in and out of the Swap faze.
            //This happens after the swap function, so shouldn't interfere
            if (!swapStarted)
            {
                swapStarted = true;
            }
            else
            {
                swapStarted = false;
            }
        }
   

        //Testing to see if The highlight boxes will pop up
        foreach(PartyDisplaySet i in partySlots)
        {
            if( System.Array.IndexOf(partySlots, i) == selectIndex)
            {
                i.SetHighlightPokemon(true); //We highlight the pokemon
                if(swapStarted) //If we are currently selecting a swap
                {
                    i.SetSelectedPokemon(true); 
                }
            }
            else
            {
                i.SetHighlightPokemon(false); //We make sure it isn't highlighted
                if(swapStarted && System.Array.IndexOf(partySlots, i) != swapOneIndex) //If we are swapping and pokemon is not first selected
                {
                    i.SetSelectedPokemon(false);
                }
            }
        }
    }

    private void HorizontalInput()
    {
        //When the player clicks left or right, the index should move one,
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (selectIndex - 1 < 0) //We reached the beginning of the party
            {
                selectIndex = partySlots.Length - 1;
            }
            else //Good to move
            {
                selectIndex--;
            }

            Debug.Log(selectIndex);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (selectIndex + 1 > partySlots.Length - 1) //We reached end of the party
            {
                selectIndex = 0;
            }
            else
            {
                selectIndex++;
            }
            Debug.Log(selectIndex);
        }
    }

    private void VerticalInput()
    {
        //If they click up or down, the index should move two
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (selectIndex - 2 < 0) //We reached the top of party
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
                selectIndex -= 2;
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
    }


    private void SwapPokemon()
    {

        if (!swapStarted) //We need to start setting up the swap
        {
            //We need to store the index values for the final swap
            swapOneIndex = selectIndex;

            //Highlight the UI
            partySlots[swapOneIndex].SetSelectedPokemon(true);

            //Display button instructions on how to swap (Back space to cancel, enter to select second pokemon)


        }
        else if(swapStarted) // We are ending the swap
        {
            swapTwoIndex = selectIndex;

            if(swapOneIndex != swapTwoIndex) //If we haven't reselected same Pokemon
            {
                Swap(swapOneIndex, swapTwoIndex);
            }

            //Swap Pokemon in Display, PartyManager array, and in gamemanager array


        }
        else
        {
            Debug.Log("Something went wrong with the swap");
        }



        //Allow players to choose a second pokemon

        //Swap Pokemon in Display, PartyManager array, and in gamemanager array
    }

    //Swaps two elements in the GameManager Pokemon list
    void Swap(int first, int second)
    {
        //Make a variable to the managerList
        PokemonParty pokeManager = gameManager.GetComponent<PokemonParty>();
        //First, we temporarily store the first pokemon chosen
        Pokemon temp = pokeManager.partyList[first];
        pokeManager.partyList[first] = pokeManager.partyList[second];
        pokeManager.partyList[second] = temp;
    }

    public void Init()
    {
        partySlots = GetComponentsInChildren<PartyDisplaySet>();
        //We still need to initialize party pokemon into array
        foreach (Pokemon p in partyPokemonCopy)
        {
            int index = partyPokemonCopy.IndexOf(p);
            Debug.Log(index);
            partySlots[index].partyPokemon = p;
        }

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
