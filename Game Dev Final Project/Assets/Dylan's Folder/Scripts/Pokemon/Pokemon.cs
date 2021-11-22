using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pokemon
{
    [SerializeField] PokemonBase _base;
    [SerializeField] int level;

    public PokemonBase Base
    {
        get
        {
            return _base;
        }
    }

    public int Level
    {
        get { return level; }
    }

    public int HP { get; set; }

    public List<Move> Moves { get; set; }

    public void Init()
    {
        HP = _base.MaxHP;

        //Generate Moves
        Moves = new List<Move>();
        foreach(var move in _base.LearnableMoves)
        {
            if(move.Level <= level)
            {
                Moves.Add(new Move(move.Base));
            }

            if(Moves.Count >= 4)
            {
                break;
            }
        }
    }


    
    public Sprite FrontSprite
    {
        get { return _base.FrontSprite; }
    }

    public Sprite BackSprite
    {
        get { return _base.BackSprite; }
    }

    public string Name
    {
        get { return _base.name; }
    }

    public int Attack
    {
        //Apparently the formula to calculate XXXX in Pokemon
        get { return Mathf.FloorToInt((_base.Attack * level) / 100f) + 5; }
    }

    public int Defense
    {
        //Apparently the formula to calculate XXXX in Pokemon
        get { return Mathf.FloorToInt((_base.Defense * level) / 100f) + 5; }
    }

    public int SpAttack
    {
        //Apparently the formula to calculate XXXX in Pokemon
        get { return Mathf.FloorToInt((_base.SpDefense * level) / 100f) + 5; }
    }

    public int SpDefense
    {
        //Apparently the formula to calculate XXXX in Pokemon
        get { return Mathf.FloorToInt((_base.SpDefense * level) / 100f) + 5; }
    }

    public int Speed
    {
        //Apparently the formula to calculate XXXX in Pokemon
        get { return Mathf.FloorToInt((_base.Speed * level) / 100f) + 5; }
    }

    //The only different equation
    public int MaxHP
    {
        //Apparently the formula to calculate XXXX in Pokemon
        get { return Mathf.FloorToInt((_base.MaxHP * level) / 100f) + 10; }
    }
}
