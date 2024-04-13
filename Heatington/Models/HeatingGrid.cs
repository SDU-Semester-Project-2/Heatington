namespace Heatington.Models;

public class HeatingGrid
{
    public Guid Id { get; set; }
    public string PicturePath { get; set; }
    public string Name { get; set; }

    public HeatingGrid(string picturePath, string name)
    {
        Id = Guid.NewGuid();
        PicturePath = picturePath;
        Name = name;
    }
}
