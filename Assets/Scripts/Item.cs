public class Item
{
    public string Name;
    public string Description;
    public System.Action Effect;
    public Item(string name, string description, System.Action effect)
    {
        Name = name;
        Description = description;
        Effect = effect;
    }
    public void Use()
    {
        Effect?.Invoke();
    }
}
