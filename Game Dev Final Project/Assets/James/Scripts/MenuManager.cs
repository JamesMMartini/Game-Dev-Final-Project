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

    GameObject activeMenu;

    [Header ("Menus")]
    public GameObject narrationMenu;
    public GameObject mainMenu;
    public GameObject runMenu;    

    [Header ("Colors")]
    public Color selectedColor;
    public Color defaultColor;
    
    GameObject[][] buttonArray = new GameObject[2][];

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

        text.text = intro;
        contButton.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (contButton.activeSelf) // We're looking at narration text
        {
            if (Input.GetKeyUp(KeyCode.Return))
            {
                // Switch to the main menu
                text.text = mainMenuText;
                contButton.SetActive(false);

                // Set all the button texts
                buttonArray[0][0].GetComponentInChildren<TMP_Text>().text = menuOptions[0];
                buttonArray[0][1].GetComponentInChildren<TMP_Text>().text = menuOptions[1];
                buttonArray[1][0].GetComponentInChildren<TMP_Text>().text = menuOptions[2];
                buttonArray[1][1].GetComponentInChildren<TMP_Text>().text = menuOptions[3];

                SelectButton(0, 0);
            }
        }
        else if(text.text == mainMenuText) // We're in the main menu
        {
            if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                if (selectedRow != 0)
                {
                    SelectButton(selectedRow - 1, selectedCol);
                }
            }
            else if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                if (selectedRow != 1)
                {
                    SelectButton(selectedRow + 1, selectedCol);
                }
            }
            else if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                if (selectedCol != 0)
                {
                    SelectButton(selectedRow, selectedCol - 1);
                }
            }
            else if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                if (selectedCol != 1)
                {
                    SelectButton(selectedRow, selectedCol + 1);
                }
            }
        }
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

    }
}
