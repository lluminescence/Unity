using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateController : LootBoxController
{
    private List<ItemBaseController> Items { get; set; }

    public CrateController(string _name)
    {
        this.Name = _name;
        this.Type = "LootBox";
        this.Description = "Box with loot";
        Items = new List<ItemBaseController> { new PoisonController("Poised"), new HealController("Treatment") };
    }

    public override void Interact()
    {
        base.Interact(); //Open box with loot
    }
}
