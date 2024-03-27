namespace Heatington.AssetManager;

public class HeatingGrid
{
    public Guid Id { get; set; }
    public string Picture { get; set; }
    public string Name { get; set; }

    public HeatingGrid(string picture, string name)
    {
        Id = Guid.NewGuid();
        Picture = picture;
        Name = name;
    }
}
