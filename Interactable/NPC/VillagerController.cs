using System.Collections.Generic;

public class VillagerController : NPCBaseController
{
    public VillagerController(string _name)
    {
        this.Health = 100;
        this.Messages = new List<string> { "Hello", "What do you need?" };
        this.Name = _name;
        this.Role = "Villager";
    }

    protected override void Move()
    {
        // move only in the village
    }

    public override void Interact()
    {
        Talk(); // open dialog with player
        Trade(); // trade with player
    }
}
