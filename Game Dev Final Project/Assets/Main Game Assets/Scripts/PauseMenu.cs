using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class PauseMenu : MonoBehaviour
{
    GameObject[][] buttonArray = new GameObject[2][];
    int selectedRow, selectedCol;

    public Color selectedColor;
    public Color defaultColor;

    public PauseManager pauseManager;
    public GameObject SwapMenu;

    // Start is called before the first frame update
    void Start()
    {
        buttonArray = new GameObject[2][];

        // Get the length of the two different arrays
        int firstArrayLength = (transform.childCount / 2) + (transform.childCount % 2);
        int secondArrayLength = transform.childCount / 2;

        // Populate the first array
        buttonArray[0] = new GameObject[firstArrayLength];
        for (int i = 0; i < buttonArray[0].Length; i++)
            buttonArray[0][i] = transform.GetChild(i).gameObject;

        // Populate the second array
        buttonArray[1] = new GameObject[secondArrayLength];
        for (int i = 0; i < buttonArray[1].Length; i++)
            buttonArray[1][i] = transform.GetChild(firstArrayLength + i).gameObject;

        // Select the first button
        SelectButton(0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        // Make sure to run the menu controls and allow the player to change the highlighted button
        MenuControls();

        //Check to see if the player has selected a button
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ButtonPressed();
        }
    }

    void MenuControls()
    {
        // Check to see if they want to go back
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseManager.Resume();
        }

        //// Handle moving selected buttons
        //if (Input.GetAxis("Vertical") > 0)
        //{
        //    if (selectedRow != 0)
        //    {
        //        SelectButton(selectedRow - 1, selectedCol);
        //    }
        //}
        //else if (Input.GetAxis("Vertical") < 0)
        //{
        //    if (selectedRow < buttonArray.Length - 1)
        //    {
        //        SelectButton(selectedRow + 1, selectedCol);
        //    }
        //}
        //else if (Input.GetAxis("Horizontal") < 0)
        //{
        //    if (selectedCol != 0)
        //    {
        //        SelectButton(selectedRow, selectedCol - 1);
        //    }
        //}
        //else if (Input.GetAxis("Horizontal") > 0)
        //{
        //    if (selectedCol < buttonArray[selectedRow].Length - 1)
        //    {
        //        SelectButton(selectedRow, selectedCol + 1);
        //    }
        //}

        // Handle moving selected buttons
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (selectedRow != 0)
            {
                SelectButton(selectedRow - 1, selectedCol);
            }
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            if (selectedRow < buttonArray.Length - 1)
            {
                SelectButton(selectedRow + 1, selectedCol);
            }
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            if (selectedCol != 0)
            {
                SelectButton(selectedRow, selectedCol - 1);
            }
        }
        else if (Input.GetKeyDown(KeyCode.D))
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

    void ButtonPressed()
    {
        GameObject selectedButton = buttonArray[selectedRow][selectedCol];
        string selectedText = selectedButton.GetComponentInChildren<TMP_Text>().text;
        if (selectedText == "Reset Game")
        {
            GameManager gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
            gameManager.ResetData();

            Time.timeScale = 1;
            SceneManager.LoadScene("OpenWorld");
        }
        else if (selectedText == "Heal Pokemon")
        {
            GameManager gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
            foreach (Pokemon p in gameManager.party.partyList)
                p.HP = p.MaxHP;

            gameManager.SaveData();

            pauseManager.Resume();
        }
        else if (selectedText == "Resume")
        {
            pauseManager.Resume();
        }
        else if (selectedText == "Edit Pokemon")
        {
            //Save the data first
            GameManager gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
            gameManager.SaveData();
            //Lets load in the other scene
            Time.timeScale = 1;
            SceneManager.LoadScene("InventoryTest");
        }
    }
}
