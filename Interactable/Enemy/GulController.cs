using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GulController : EnemyBaseController
{
    public GulController(string _name) : base(_name)
    {
        this.Name = _name;
        this.Damage = 3;
        this.Health = 100;
    }

    protected override void Move()
    {
        base.Move(); // Move
    }

    public override void Interact()
    {
        base.Attack(); // Attack
    }
}
