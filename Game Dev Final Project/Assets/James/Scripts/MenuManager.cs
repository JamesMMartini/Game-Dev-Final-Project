using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    PokemonParty pokemonParty;

    Pokemon playerPokemon;
    public Pokemon enemyPokemon;

    [Header ("Sprite Objects")]
    public GameObject playerSprite;
    public GameObject enemySprite;
    public GameObject trainerSprite;

    [Header("Stat Bars")]
    public StatBars playerStats;
    public StatBars enemyStats;
    
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
    public NarrationDialog enterPokemon;

    [Header("Object Locations/Lerp Info")]
    public Vector3 playerHealthPos;
    public Vector3 enemyHealthPos;
    public Vector3 mainBoxPos;
    public Vector3 enemyPos;
    public Vector3 playerPos;
    public Vector3 pokemonOffscreen;
    public float speed;

    [Header("Move Settings")]
    public string move;
    public string[] menuOptions;
    public string[] moves;

    [Header("Pokemon Select Screen")]
    public GameObject selectScreen;

    int selectedRow, selectedCol;
    bool menuActive;
    bool resetPokemon;

    // Start is called before the first frame update
    void Start()
    {
        pokemonParty = GameManager.gameManager.GetComponent<GameManager>().party;

        // Initialize the pokemon
        playerPokemon = pokemonParty.partyList[0];
        enemyPokemon.Init();

        // Set the pokemon sprites in the scene
        playerSprite.GetComponent<SpriteRenderer>().sprite = playerPokemon.BackSprite;
        enemySprite.GetComponent<SpriteRenderer>().sprite = enemyPokemon.FrontSprite;

        if (playerPokemon.HP <= 0)
        {
            Pokemon newPokemon = null;
            foreach (Pokemon poke in pokemonParty.partyList)
            {
                if (poke.HP > 0)
                {
                    newPokemon = poke;
                    break;
                }
            }

            if (newPokemon == null)
            {

            }
            else
            {
                playerPokemon = newPokemon;
                playerSprite.GetComponent<SpriteRenderer>().sprite = playerPokemon.BackSprite;
            }
        }

        resetPokemon = false;
        menuActive = false;
        StartCoroutine(EnterObjects());

        playerStats.UpdateBars(playerPokemon.HP, playerPokemon.MaxHP);
        enemyStats.UpdateBars(enemyPokemon.HP, enemyPokemon.MaxHP);
    }

    // Update is called once per frame
    void Update()
    {
        if (menuActive)
        {
            if (activeMenu == narrationMenu) // We're in the narration menu
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    if (currentDialog.Next.NarrationType == NarrationType.NewDialog)
                    {
                        SwapMenu(narrationMenu, currentDialog.Next);
                        //menuActive = false;
                        //StartCoroutine(AdvanceDialog(currentDialog.Next));
                    }
                    else if (currentDialog.Next.NarrationType == NarrationType.SelectPokemon)
                    {
                        // Create a new narration dialog for this
                        NarrationDialog newDialog = ScriptableObject.CreateInstance<NarrationDialog>();
                        newDialog.NarrationType = NarrationType.SelectPokemon;
                        newDialog.Next = currentDialog.Next.Next;
                        newDialog.Previous = currentDialog;
                        newDialog.Text = playerPokemon.Name + currentDialog.Next.Text;

                        // Swap the menus and the pokemon
                        SwapMenu(narrationMenu, newDialog);
                        StartCoroutine(SwapPokemon(playerPokemon));
                    }
                    else if (currentDialog.Next.NarrationType == NarrationType.MainMenu)
                    {
                        SwapMenu(mainMenu, mainMenuText);
                    }
                    else if (currentDialog.Next.NarrationType == NarrationType.RunMenu)
                    {
                        SwapMenu(runMenu, runText);
                    }
                    else if (currentDialog.Next.NarrationType == NarrationType.EnemyMove)
                    {
                        EnemyAction();
                    }
                    else if (currentDialog.Next.NarrationType == NarrationType.SelectScreen)
                    {
                        activeMenu = selectScreen;
                        selectScreen.SetActive(true);
                    }
                    else if (currentDialog.Next.NarrationType == NarrationType.EndDialog)
                    {
                        if (resetPokemon)
                        {
                            // Reset the Pokemon's health
                            foreach (Pokemon poke in pokemonParty.partyList)
                                poke.HP = poke.MaxHP;
                        }

                        GameManager.gameManager.GetComponent<GameManager>().party = pokemonParty;
                        GameManager.gameManager.GetComponent<GameManager>().SaveData();

                        SceneManager.LoadScene("OpenWorld");
                    }
                }
            }
            else if (activeMenu == selectScreen)
            {
                if (Input.GetKeyDown(KeyCode.Return)) // A pokemon was selected
                {
                    if (pokemonParty.partyList[selectScreen.GetComponentInChildren<PartyManager>().selectIndex].HP > 0) // We can choose the pokemon
                    {
                        playerPokemon = pokemonParty.partyList[selectScreen.GetComponentInChildren<PartyManager>().selectIndex];

                        // Create a new narration dialog for this
                        NarrationDialog newDialog = ScriptableObject.CreateInstance<NarrationDialog>();
                        newDialog.NarrationType = NarrationType.SelectPokemon;
                        newDialog.Next = currentDialog.Next.Next;
                        newDialog.Previous = currentDialog;
                        newDialog.Text = playerPokemon.Name + enterPokemon.Text;

                        // Swap the menus and the pokemon
                        SwapMenu(narrationMenu, newDialog);
                        StartCoroutine(SwapPokemon(playerPokemon));

                        playerStats.UpdateBars(playerPokemon.HP, playerPokemon.MaxHP);
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
    }

    IEnumerator EnterObjects()
    {
        float t = 0.0f;
        Vector3 enemyHealthStart = enemyStats.gameObject.transform.localPosition;
        Vector3 playerHealthStart = playerStats.gameObject.transform.localPosition;
        Vector3 mainBoxStart = transform.localPosition;
        Vector3 enemyStart = enemySprite.transform.localPosition;
        while (t < 1.0f)
        {
            t += Time.deltaTime * speed;
            enemyStats.gameObject.transform.localPosition = Vector3.Lerp(enemyHealthStart, enemyHealthPos, t);
            playerStats.gameObject.transform.localPosition = Vector3.Lerp(playerHealthStart, playerHealthPos, t);
            transform.localPosition = Vector3.Lerp(mainBoxStart, mainBoxPos, t);
            enemySprite.transform.localPosition = Vector3.Lerp(enemyStart, enemyPos, t);
            yield return null;
        }

        yield return new WaitForSeconds(0.25f);

        SwapMenu(narrationMenu, intro);
    }

    IEnumerator AdvanceDialog(NarrationDialog newDialog)
    {
        currentDialog = newDialog;

        activeMenu.GetComponentInChildren<TMP_Text>().text = currentDialog.Text;

        int textIndex = 0;
        activeMenu.GetComponentInChildren<TMP_Text>().text = "";
        while (textIndex < currentDialog.Text.Length)
        {
            activeMenu.GetComponentInChildren<TMP_Text>().text += currentDialog.Text[textIndex];

            yield return new WaitForSeconds(0.02f);

            textIndex++;
        }
        menuActive = true;
    }

    IEnumerator SwapPokemon(Pokemon newPokemon)
    {
        menuActive = false; // Scattering these lines around to make sure that you can't advance the screen before the pokemon finishes swapping

        // Make sure the pokemon is out of frame
        float t = 0.0f;
        Vector3 pokemonStart = playerSprite.transform.localPosition;
        while (t < 1.0f)
        {
            menuActive = false;

            t += Time.deltaTime * speed * 2;
            playerSprite.transform.localPosition = Vector3.Lerp(pokemonStart, pokemonOffscreen, t);
            yield return null;
        }

        // Move the trainer on screen
        t = 0.0f;
        Vector3 trainerStart = trainerSprite.transform.localPosition;
        while (t < 1.0f)
        {
            menuActive = false;

            t += Time.deltaTime * speed;
            trainerSprite.transform.localPosition = Vector3.Lerp(trainerStart, playerPos, t);
            yield return null;
        }

        menuActive = false;
        yield return new WaitForSeconds(1f);
        menuActive = false;

        // SWAP THE POKEMON SPRITE WHEN IMPLEMENTED
        playerSprite.GetComponent<SpriteRenderer>().sprite = playerPokemon.BackSprite;

        // Move the trainer off screen and pokemon on screen
        t = 0.0f;
        trainerStart = trainerSprite.transform.localPosition;
        pokemonStart = playerSprite.transform.localPosition;
        while (t < 1.0f)
        {
            menuActive = false;

            t += Time.deltaTime * speed * 2;
            trainerSprite.transform.localPosition = Vector3.Lerp(trainerStart, pokemonOffscreen, t);
            playerSprite.transform.localPosition = Vector3.Lerp(pokemonStart, playerPos, t);
            yield return null;
        }

        menuActive = true;
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
                NarrationDialog swapDialog = ScriptableObject.CreateInstance<NarrationDialog>();
                swapDialog.NarrationType = NarrationType.NewDialog;
                swapDialog.Text = "Change pokemon.";
                currentDialog.Next = swapDialog;
                swapDialog.Previous = currentDialog;

                NarrationDialog selectDialog = ScriptableObject.CreateInstance<NarrationDialog>();
                selectDialog.NarrationType = NarrationType.SelectScreen;
                swapDialog.Next = selectDialog;
                selectDialog.Previous = swapDialog;

                NarrationDialog enemyMove = ScriptableObject.CreateInstance<NarrationDialog>();
                enemyMove.NarrationType = NarrationType.NewDialog;
                enemyMove.Text = enemyPokemon.Name + " prepares to attack!";
                enemyMove.Previous = selectDialog;
                selectDialog.Next = enemyMove;

                NarrationDialog enemyAttacks = ScriptableObject.CreateInstance<NarrationDialog>();
                enemyAttacks.NarrationType = NarrationType.EnemyMove;
                enemyAttacks.Previous = enemyMove;
                enemyMove.Next = enemyAttacks;

                SwapMenu(narrationMenu, swapDialog);
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
                GameManager.gameManager.GetComponent<GameManager>().party = pokemonParty;
                GameManager.gameManager.GetComponent<GameManager>().SaveData();

                SceneManager.LoadScene("OpenWorld", LoadSceneMode.Single);
            }
            else if (selectedText == "No")
            {
                SwapMenu(mainMenu, mainMenuText);
            }
        }
        else if (activeMenu == fightMenu)
        {
            ExecuteAction();
        }
    }

    void ExecuteAction()
    {
        // Get the selected move name
        string moveName = buttonArray[selectedRow][selectedCol].GetComponentInChildren<TMP_Text>().text;

        // Get the move's data from the pokemon
        Move selectedMove = null;
        foreach (Move move in playerPokemon.Moves)
        {
            if (move.Base.name == moveName)
                selectedMove = move;
        }

        // Create the next narration dialog
        NarrationDialog actionDialog = ScriptableObject.CreateInstance<NarrationDialog>();
        actionDialog.NarrationType = NarrationType.NewDialog;
        actionDialog.Text = playerPokemon.Name + " uses " + selectedMove.Base.name;

        // Now we need to calculate and apply the damage to the enemy pokemon
        //float damage = (selectedMove.Base.Power - (enemyPokemon.Defense / 2)) / 2;
        float effectiveness = GetWeakness(enemyPokemon.Base.Type1, selectedMove.Base.Type);
        //damage *= effectiveness;

        enemyPokemon.HP -= CalculateDamage(selectedMove, enemyPokemon, playerPokemon, effectiveness);

        //enemyPokemon.HP -= (int)damage;

        // Create the effectiveness dialog
        NarrationDialog effectiveDialog = ScriptableObject.CreateInstance<NarrationDialog>();
        effectiveDialog.NarrationType = NarrationType.NewDialog;

        if (effectiveness == 0.5f)
            effectiveDialog.Text = "It's not very effective.";
        else if (effectiveness == 2f)
            effectiveDialog.Text = "It's very effective!";
        else
            effectiveDialog.Text = "It's mildly effective.";
        effectiveDialog.Previous = actionDialog;
        effectiveDialog.Next = mainMenuText;

        // See if the battle has ended
        if (playerPokemon.HP <= 0 || enemyPokemon.HP <= 0)
        {
            NarrationDialog endDialog = ScriptableObject.CreateInstance<NarrationDialog>();
            endDialog.NarrationType = NarrationType.NewDialog;
            effectiveDialog.Next = endDialog;
            endDialog.Previous = effectiveDialog;
            endDialog.Text = "It's over. Go home.";

            NarrationDialog closeScene = ScriptableObject.CreateInstance<NarrationDialog>();
            closeScene.NarrationType = NarrationType.EndDialog;
            closeScene.Previous = endDialog;
            endDialog.Next = closeScene;
        }
        else // Prep for the enemy's move
        {
            NarrationDialog enemyMove = ScriptableObject.CreateInstance<NarrationDialog>();
            enemyMove.NarrationType = NarrationType.NewDialog;
            enemyMove.Text = enemyPokemon.Name + " prepares to attack!";
            enemyMove.Previous = effectiveDialog;
            effectiveDialog.Next = enemyMove;

            NarrationDialog enemyAttacks = ScriptableObject.CreateInstance<NarrationDialog>();
            enemyAttacks.NarrationType = NarrationType.EnemyMove;
            enemyAttacks.Previous = enemyMove;
            enemyMove.Next = enemyAttacks;
        }

        // Set the next on the action dialog
        actionDialog.Next = effectiveDialog;

        playerStats.UpdateBars(playerPokemon.HP, playerPokemon.MaxHP);
        enemyStats.UpdateBars(enemyPokemon.HP, enemyPokemon.MaxHP);

        SwapMenu(narrationMenu, actionDialog);
    }

    void EnemyAction()
    {
        // Get a random int to select the move
        System.Random rand = new System.Random();
        int random = (int)(rand.NextDouble() * enemyPokemon.Moves.Count);

        // Get the move's data from the pokemon
        Move selectedMove = enemyPokemon.Moves[random];

        // Create the next narration dialog
        NarrationDialog actionDialog = ScriptableObject.CreateInstance<NarrationDialog>();
        actionDialog.NarrationType = NarrationType.NewDialog;
        actionDialog.Text = enemyPokemon.Name + " uses " + selectedMove.Base.name;

        // Now we need to calculate and apply the damage to the enemy pokemon
        //float damage = (selectedMove.Base.Power - (playerPokemon.Defense / 2))/2;
        float effectiveness = GetWeakness(playerPokemon.Base.Type1, selectedMove.Base.Type);
        //damage *= effectiveness;

        playerPokemon.HP -= CalculateDamage(selectedMove, playerPokemon, enemyPokemon, effectiveness);

        //playerPokemon.HP -= (int)damage;

        // Create the effectiveness dialog
        NarrationDialog effectiveDialog = ScriptableObject.CreateInstance<NarrationDialog>();
        effectiveDialog.NarrationType = NarrationType.NewDialog;

        if (effectiveness == 0.5f)
            effectiveDialog.Text = "It's not very effective.";
        else if (effectiveness == 2f)
            effectiveDialog.Text = "It's very effective!";
        else
            effectiveDialog.Text = "It's mildly effective.";
        effectiveDialog.Previous = actionDialog;
        effectiveDialog.Next = mainMenuText;

        // See if the battle has ended
        if (enemyPokemon.HP <= 0)
        {
            NarrationDialog endDialog = ScriptableObject.CreateInstance<NarrationDialog>();
            endDialog.NarrationType = NarrationType.NewDialog;
            effectiveDialog.Next = endDialog;
            endDialog.Previous = effectiveDialog;
            endDialog.Text = "It's over. Go home.";

            NarrationDialog closeScene = ScriptableObject.CreateInstance<NarrationDialog>();
            closeScene.NarrationType = NarrationType.EndDialog;
            closeScene.Previous = endDialog;
            endDialog.Next = closeScene;
        }
        else if (playerPokemon.HP <= 0) // The Pokemon has fainted and we need to swap pokemon
        {
            int aliveCount = 0;
            foreach (Pokemon poke in pokemonParty.partyList)
            {
                Debug.Log("HP: " + poke.HP);
                if (poke.HP > 0)
                    aliveCount++;
            }

            if (aliveCount > 0) // The player can keep going
            {
                NarrationDialog faintDialog = ScriptableObject.CreateInstance<NarrationDialog>();
                faintDialog.NarrationType = NarrationType.NewDialog;
                faintDialog.Text = playerPokemon.Name + " fainted! Choose a new pokemon to battle!";
                effectiveDialog.Next = faintDialog;
                faintDialog.Previous = effectiveDialog;

                NarrationDialog selectDialog = ScriptableObject.CreateInstance<NarrationDialog>();
                selectDialog.NarrationType = NarrationType.SelectScreen;
                faintDialog.Next = selectDialog;
                selectDialog.Previous = faintDialog;
                selectDialog.Next = enterPokemon.Next;
            }
            else // All the player's pokemon have fainted
            {
                NarrationDialog endDialog = ScriptableObject.CreateInstance<NarrationDialog>();
                endDialog.NarrationType = NarrationType.NewDialog;
                effectiveDialog.Next = endDialog;
                endDialog.Previous = effectiveDialog;
                endDialog.Text = "It's over. Go home.";

                NarrationDialog closeScene = ScriptableObject.CreateInstance<NarrationDialog>();
                closeScene.NarrationType = NarrationType.EndDialog;
                closeScene.Previous = endDialog;
                endDialog.Next = closeScene;

                // Ensure that we reset the pokemon's health
                resetPokemon = true;
            }
        }
        else // Go to the action menu
        {
            effectiveDialog.Next = mainMenuText;
        }

        // Set the next on the action dialog
        actionDialog.Next = effectiveDialog;

        playerStats.UpdateBars(playerPokemon.HP, playerPokemon.MaxHP);
        enemyStats.UpdateBars(enemyPokemon.HP, enemyPokemon.MaxHP);

        SwapMenu(narrationMenu, actionDialog);
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
        if (Input.GetAxis("Vertical") > 0)
        {
            if (selectedRow != 0)
            {
                SelectButton(selectedRow - 1, selectedCol);
            }
        }
        else if (Input.GetAxis("Vertical") < 0)
        {
            if (selectedRow < buttonArray.Length - 1)
            {
                SelectButton(selectedRow + 1, selectedCol);
            }
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            if (selectedCol != 0)
            {
                SelectButton(selectedRow, selectedCol - 1);
            }
        }
        else if (Input.GetAxis("Horizontal") > 0)
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
            menuActive = false;
            StartCoroutine(AdvanceDialog(currentDialog));
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

    public float GetWeakness(PokemonType pokemonType, PokemonType moveType)
    {
        float strength = 1f;

        switch (pokemonType)
        {
            case PokemonType.Fire:
                if (moveType == PokemonType.Fire || moveType == PokemonType.Grass)
                    strength = 0.5f;
                else if (moveType == PokemonType.Water)
                    strength = 2f;
                break;
            case PokemonType.Grass:
                if (moveType == PokemonType.Water || moveType == PokemonType.Grass)
                    strength = 0.5f;
                else if (moveType == PokemonType.Fire)
                    strength = 2f;
                break;
            case PokemonType.None:
                strength = 1f;
                break;
            case PokemonType.Normal:
                strength = 1f;
                break;
            case PokemonType.Water:
                if (moveType == PokemonType.Fire || moveType == PokemonType.Water)
                    strength = 0.5f;
                else if (moveType == PokemonType.Grass)
                    strength = 2f;
                break;
            default:
                strength = 1f;
                break;
        }

        return strength;
    }

    int CalculateDamage(Move selectedMove, Pokemon defendingPokemon, Pokemon attackingPokemon, float effectiveness)
    {
        float damage = (selectedMove.Base.Power * (attackingPokemon.Attack / defendingPokemon.Defense)) * (attackingPokemon.Attack / 100) + 4;
        damage *= effectiveness;

        return (int)damage;
    }
}
