using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public PokemonBase playerPokemon;
    public PokemonBase enemyPokemon;

    public GameObject contButton;
    public TMP_Text text;
    public GameObject buttonParent;
    public GameObject[] buttons;
    public Color selectedColor;
    public Color defaultColor;

    GameObject[][] buttonArray = new GameObject[2][];

    public string intro;
    public string mainMenu;
    public string move;
    public string[] menuOptions;
    public string[] moves;

    int selectedRow, selectedCol;

    private void Awake()
    {
        // Initialize the button array
        buttonArray[0] = new GameObject[2];
        buttonArray[0][0] = buttons[0];
        buttonArray[0][1] = buttons[1];

        buttonArray[1] = new GameObject[2];
        buttonArray[1][0] = buttons[2];
        buttonArray[1][1] = buttons[3];

        //Set the initial menus and things
        buttonParent.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
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
                text.text = mainMenu;
                contButton.SetActive(false);
                buttonParent.SetActive(true);

                // Set all the button texts
                buttonArray[0][0].GetComponentInChildren<TMP_Text>().text = menuOptions[0];
                buttonArray[0][1].GetComponentInChildren<TMP_Text>().text = menuOptions[1];
                buttonArray[1][0].GetComponentInChildren<TMP_Text>().text = menuOptions[2];
                buttonArray[1][1].GetComponentInChildren<TMP_Text>().text = menuOptions[3];

                SelectButton(0, 0);
            }
        }
        else if(text.text == mainMenu) // We're in the main menu
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
}
