using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;

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
    public GameObject fightMenu;

    [Header ("Colors")]
    public Color selectedColor;
    public Color defaultColor;
    
    GameObject[][] buttonArray = new GameObject[2][];
    GameObject activeMenu;
    NarrationDialog currentDialog;

    [Header("Menu Text")]
    public NarrationDialog intro;
    public NarrationDialog mainMenuText;
    public NarrationDialog runText;
    public NarrationDialog fightText;

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
        SwapMenu(narrationMenu, intro);

        // Set up the initial text and such
        activeMenu.GetComponentInChildren<TMP_Text>().text = intro.Text;
    }

    // Update is called once per frame
    void Update()
    {
        if (activeMenu == narrationMenu) // We're in the narration menu
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (currentDialog.Next.NarrationType == NarrationType.NewDialog)
                {

                }
                else if (currentDialog.Next.NarrationType == NarrationType.MainMenu)
                {
                    SwapMenu(mainMenu, mainMenuText);
                }
                else if (currentDialog.Next.NarrationType == NarrationType.RunMenu)
                {
                    SwapMenu(runMenu, runText);
                }
            }
        }
        else // We're in another menu
        {
            // Make sure to run the menu controls and allow the player to change the highlighted button
            MenuControls();

            //Check to see if the player has selected a button
            if (Input.GetKeyDown(KeyCode.Return))
            {
                ButtonPressed();
            }
        }
    }

    void AdvanceDialog(NarrationDialog newDialog)
    {
        currentDialog = newDialog;

        activeMenu.GetComponentInChildren<TMP_Text>().text = currentDialog.Text;
    }

    void ButtonPressed()
    {
        if (activeMenu == mainMenu)
        {
            GameObject selectedButton = buttonArray[selectedRow][selectedCol];
            string selectedText = selectedButton.GetComponentInChildren<TMP_Text>().text;
            if (selectedText == "Fight")
            {
                SwapMenu(fightMenu, fightText);
            }
            else if (selectedText == "Bag")
            {

            }
            else if (selectedText == "Pokemon")
            {

            }
            else if (selectedText == "Run")
            {
                SwapMenu(runMenu, runText);
            }
        }
        else if (activeMenu == runMenu)
        {
            GameObject selectedButton = buttonArray[selectedRow][selectedCol];
            string selectedText = selectedButton.GetComponentInChildren<TMP_Text>().text;
            if (selectedText == "Yes")
            {
                // Right now we unload this scene and load the open world scene, but in the long-term
                // it might be better to load this scene as an additive scene and then just unload this scene
                // so we don't need to worry about passing so much data back and forth
                SceneManager.LoadScene("OpenWorld", LoadSceneMode.Single);
            }
            else if (selectedText == "No")
            {
                SwapMenu(mainMenu, mainMenuText);
            }
        }
    }

    void MenuControls()
    {
        // Check to see if they want to go back
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentDialog.Previous != null)
            {
                if (currentDialog.Previous == mainMenuText)
                    SwapMenu(mainMenu, currentDialog.Previous);
            }
        }

        // Handle moving selected buttons
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (selectedRow != 0)
            {
                SelectButton(selectedRow - 1, selectedCol);
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (selectedRow < buttonArray.Length - 1)
            {
                SelectButton(selectedRow + 1, selectedCol);
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (selectedCol != 0)
            {
                SelectButton(selectedRow, selectedCol - 1);
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (selectedCol < buttonArray[selectedRow].Length - 1)
            {
                SelectButton(selectedRow, selectedCol + 1);
            }
        }
    }

    void SelectButton(int row, int col)
    {
        // Deselect the previous button
        try
        {
            buttonArray[selectedRow][selectedCol].GetComponent<Image>().color = defaultColor;
            buttonArray[selectedRow][selectedCol].GetComponentInChildren<TMP_Text>().color = Color.white;
        }
        catch (Exception ex)
        {
            // We don't need to do anything
        }

        // Select the new button
        selectedRow = row;
        selectedCol = col;

        buttonArray[row][col].GetComponent<Image>().color = selectedColor;
        buttonArray[row][col].GetComponentInChildren<TMP_Text>().color = Color.black;
    }

    void SwapMenu(GameObject newMenu, NarrationDialog newDialog)
    {
        // Disable the existing menu
        if (activeMenu != null)
            activeMenu.SetActive(false);

        // Change the active menu
        activeMenu = newMenu;

        // Activate the new menu
        activeMenu.SetActive(true);

        // Set the new text
        currentDialog = newDialog;
        activeMenu.transform.Find("Text").GetComponent<TMP_Text>().text = currentDialog.Text;

        // Deselect the previous button
        try
        {
            buttonArray[selectedRow][selectedCol].GetComponent<Image>().color = defaultColor;
            buttonArray[selectedRow][selectedCol].GetComponentInChildren<TMP_Text>().color = Color.white;
        }
        catch (Exception ex)
        {
            // We don't need to do anything
        }

        if (activeMenu == narrationMenu)
        {

        }
        else if (activeMenu == fightMenu)
        {
            // Get all of the buttons
            GameObject buttonGroup = null;
            foreach (Transform child in activeMenu.transform)
            {
                if (child.tag == "UI Button Group")
                    buttonGroup = child.gameObject;
            }

            // Deactivate all of the button so we can only enable the ones necessary
            foreach (Transform button in buttonGroup.transform)
                button.gameObject.SetActive(false);

            // Enable a button for every move the pokemon has and add the move's name
            for (int i = 0; i < playerPokemon.Moves.Count; i++)
            {
                GameObject button = buttonGroup.transform.GetChild(i).gameObject;
                button.SetActive(true);

                button.GetComponentInChildren<TMP_Text>().text = playerPokemon.Moves[i].Base.MoveName;
            }

            // Set up the button array
            if (playerPokemon.Moves.Count <= 2)
            {
                // Set up the array
                buttonArray = new GameObject[1][];
                buttonArray[0] = new GameObject[playerPokemon.Moves.Count];

                // Populate the array
                for (int i = 0; i < playerPokemon.Moves.Count; i++)
                {
                    buttonArray[0][i] = buttonGroup.transform.GetChild(i).gameObject;
                }
            }
            else
            {
                // Set up the array
                buttonArray = new GameObject[2][];
                buttonArray[0] = new GameObject[2];
                buttonArray[1] = new GameObject[playerPokemon.Moves.Count - 2];

                // Populate the array
                for (int i = 0; i < buttonArray.Length; i++)
                {
                    for (int j = 0; j < buttonArray[i].Length; j++)
                    {
                        buttonArray[i][j] = buttonGroup.transform.GetChild(i + j).gameObject;
                    }
                }
            }

            // Select the first button
            SelectButton(0, 0);
        }
        else // We're not in the narration menu or a fight menu and need to set up a new menu
        {
            // Get all of the buttons
            GameObject buttonGroup = null;
            foreach (Transform child in activeMenu.transform)
            {
                if (child.tag == "UI Button Group")
                    buttonGroup = child.gameObject;
            }

            buttonArray = new GameObject[2][];

            // Get the length of the two different arrays
            int firstArrayLength = (buttonGroup.transform.childCount / 2) + (buttonGroup.transform.childCount % 2);
            int secondArrayLength = buttonGroup.transform.childCount / 2;

            // Populate the first array
            buttonArray[0] = new GameObject[firstArrayLength];
            for (int i = 0; i < buttonArray[0].Length; i++)
                buttonArray[0][i] = buttonGroup.transform.GetChild(i).gameObject;

            // Populate the second array
            buttonArray[1] = new GameObject[secondArrayLength];
            for (int i = 0; i < buttonArray[1].Length; i++)
                buttonArray[1][i] = buttonGroup.transform.GetChild(firstArrayLength + i).gameObject;

            // Select the first button
            SelectButton(0, 0);
        }
    }
}
