using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items 
{
   

    public short Damage { get => Damage; set => Damage = value; }
    public short AttackRating { get => AttackRating; set => AttackRating = value; }

    


    public string Description { get => Description; set => Description = value; }
    public string Name { get => Name; set => Name = value; }
    public string SpecialAbility { get => SpecialAbility; set => SpecialAbility = value; }// Fire Damage, poison, AoE Etc. Could happen on a crit based on a D20(D&D) or over 10 System(Pathfinder 2E)

    //So far there isn't a need for Item/Armor health



}
