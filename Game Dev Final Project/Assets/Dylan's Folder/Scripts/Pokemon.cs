using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pokemon
{
    PokemonBase _base;
    int level;

    public List<Move> Moves { get; set; }

    public Pokemon(PokemonBase pBase, int pLevel)
    {
        _base = pBase;
        level = pLevel;

        Moves = new List<Move>();
        foreach(var move in _base.LearnableMoves)
        {
            if(move.Level <= level)
            {
                Moves.Add(new Move(move.Base));
            }
        }
    }
    
    public string Name
    {
        get { return _base.name; }
    }


    public int Level
    {
        get { return level; }
    }

    public Sprite FrontSprite
    {
        get { return FrontSprite; }
    }

    public Sprite BackSprite
    {
        get { return BackSprite; }
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
