using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{

    PartyDisplaySet[] partySlots;

    int selectIndex;

    
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
            selectIndex--;
            Debug.Log(selectIndex);
        }
        else if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            selectIndex++;
            Debug.Log(selectIndex);
        }

        //If they click up or down, the index should move two
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {

        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {

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
    }
}
