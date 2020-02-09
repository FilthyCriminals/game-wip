using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor
{
    //So far there isn't a need for Item/Armor health
    
    /// <summary>
    /// These two might not be needed, depends on if we actually want the armor to increase these factors 
    /// </summary>
    public short Damage { get => Damage; set => Damage = value; }
    public short AttackRating{ get => AttackRating; set => AttackRating = value; }


    public string Description { get => Description; set => Description = value; }
    public string Name { get => Name; set => Name = value; }
    public string SpecialAbility { get => SpecialAbility; set => SpecialAbility = value; } //Could be anything: bonuses, damage type resistance, 
}
