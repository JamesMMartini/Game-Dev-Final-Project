using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public PokemonBase playerBase;
    public PokemonBase enemyBase;

    Pokemon playerPokemon;
    Pokemon enemyPokemon;
    
    [Header ("Menus")]
    public GameObject narrationMenu;
    public GameObject mainMenu;
    public GameObject runMenu;    

    [Header ("Colors")]
    public Color selectedColor;
    public Color defaultColor;
    
    GameObject[][] buttonArray = new GameObject[2][];
    GameObject activeMenu;

    [Header ("Menu Text")]
    public string intro;
    public string mainMenuText;
    public string move;
    public string[] menuOptions;
    public string[] moves;

    int selectedRow, selectedCol;

    private void Awake()
    {
        // Initialize the pokemon
        playerPokemon = new Pokemon(playerBase, 10);
        enemyPokemon = new Pokemon(enemyBase, 5);
    }

    // Start is called before the first frame update
    void Start()
    {
        SwapMenu(narrationMenu);

        // Set up the initial text and such
        activeMenu.GetComponent<TMP_Text>().text = intro;
    }

    // Update is called once per frame
    void Update()
    {
        if (activeMenu == narrationMenu) // We're in the narration menu
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                // Switch to the main menu
                SwapMenu(mainMenu);
            }
        }
        else if (activeMenu == mainMenu) // We're in the main menu
        {

        }
        //else if(text.text == mainMenuText) // We're in the main menu
        //{
        //    if (Input.GetKeyUp(KeyCode.UpArrow))
        //    {
        //        if (selectedRow != 0)
        //        {
        //            SelectButton(selectedRow - 1, selectedCol);
        //        }
        //    }
        //    else if (Input.GetKeyUp(KeyCode.DownArrow))
        //    {
        //        if (selectedRow != 1)
        //        {
        //            SelectButton(selectedRow + 1, selectedCol);
        //        }
        //    }
        //    else if (Input.GetKeyUp(KeyCode.LeftArrow))
        //    {
        //        if (selectedCol != 0)
        //        {
        //            SelectButton(selectedRow, selectedCol - 1);
        //        }
        //    }
        //    else if (Input.GetKeyUp(KeyCode.RightArrow))
        //    {
        //        if (selectedCol != 1)
        //        {
        //            SelectButton(selectedRow, selectedCol + 1);
        //        }
        //    }
        //}
    }

    void SelectButton(int row, int col)
    {
        // Deselect the previous button
        buttonArray[selectedRow][selectedCol].GetComponent<Image>().color = defaultColor;
        buttonArray[selectedRow][selectedCol].GetComponentInChildren<TMP_Text>().color = Color.white;

        // Select the new button
        selectedRow = row;
        selectedCol = col;

        buttonArray[row][col].GetComponent<Image>().color = selectedColor;
        buttonArray[row][col].GetComponentInChildren<TMP_Text>().color = Color.black;
    }

    void SwapMenu(GameObject newMenu)
    {
        // Disable the existing menu
        if (activeMenu != null)
            activeMenu.SetActive(false);

        // Change the active menu
        activeMenu = newMenu;

        // Activate the new menu
        activeMenu.SetActive(true);

        if (activeMenu != narrationMenu) // We're not in the narration menu and need to set up a new menu
        {
            // Get all of the buttons
            //activeMenu.GetComponent<GameObject>().transform.

            //int firstArrayLength = 
        }
    }
}
