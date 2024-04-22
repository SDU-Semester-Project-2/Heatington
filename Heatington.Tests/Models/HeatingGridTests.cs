using Heatington.Models;

namespace Heatington.Tests.Models
{
    public class HeatingGridTests
    {
        [Fact]
        public void Constructor_Sets_Properties_Correctly()
        {
            string picturePath = "example/path/picture";
            string name = "Test Grid";

            HeatingGrid heatingGrid = new HeatingGrid(picturePath, name);

            Assert.Equal(picturePath, heatingGrid.PicturePath);
            Assert.Equal(name, heatingGrid.Name);
        }
    }
}
