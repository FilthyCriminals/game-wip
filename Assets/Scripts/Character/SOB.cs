using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SOB : Skill //this is inherited even if it doesnt show it
{

    public short HP { get => HP; set => HP = value; }
//public short Damage { get => Damage; set => Damage = value; }
    public short Strength { get => Strength; set => Strength = value; }
    public short Dexterity { get => Dexterity; set => Dexterity = value; }
    public short Constitution { get => Constitution; set => Constitution = value; }
    public short Intelligence { get => Intelligence; set => Intelligence = value; }
    public short Wisdom { get => Wisdom; set => Wisdom = value; }

    public short AttackRating { get => AttackRating; set => AttackRating = value; }

    public char Gender { get => Gender; set => Gender = value; }
   // public string Name { get => Name; set => Name = value; } 




    public SOB()
    {


    }



}
