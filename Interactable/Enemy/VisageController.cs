using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisageController : EnemyBaseController
{
    public VisageController(string _name) : base(_name)
    {
        this.Name = _name;
        this.Damage = 15;
        this.Health = 200;
    }

    protected override void Move()
    {
         // Fly
    }

    public override void Interact()
    {
        base.Attack();
    }
}
