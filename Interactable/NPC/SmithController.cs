using System.Collections.Generic;

public class SmithController : VillagerController
{
    private List<ItemBaseController> Items { get; set; }

    public SmithController(string _name) : base(_name)
    {
        this.Health = 100;
        this.Messages = new List<string> { "Hello", "You wanna trade or create something?" };
        this.Name = _name;
        this.Role = "Smith";
    }

    public override void Interact()
    {
        Talk(); // open dialog with player
        Trade();
        Create();
    }

    private void Trade()
    {
        // trade with player
    }

    private void Create()
    {
        // create something for player
    }
}
