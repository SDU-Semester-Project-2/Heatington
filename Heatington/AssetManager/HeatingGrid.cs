namespace Heatington.AssetManager;

public class HeatingGrid
{
    public Guid Id { get; set; }
    public string Picture { get; set; } //path to picture of the grid
    public string Name { get; set; }

    public HeatingGrid(string picture, string name)
    {
        Id = Guid.NewGuid();
        Picture = picture;
        Name = name;
    }
}
