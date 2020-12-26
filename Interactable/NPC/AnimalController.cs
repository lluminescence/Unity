using System.Collections.Generic;

public class AnimalController : NPCBaseController
{
    public AnimalController(string _name)
    {
        this.Health = 100;
        this.Messages = new List<string> {"Says something in animal language"};
        this.Name = _name;
        this.Role = "Animal";
    }

    public override void Interact()
    {
        Talk(); // open dialog with player
    }

    protected override void Move()
    {
        // move only in the village
    }
}
