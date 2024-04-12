using Heatington.AssetManager;

namespace Heatington.Tests.Models
{
    public class ProductionUnitTest
    {
        [Fact]
        public void Constructor_Works_Properly()
        {
            ProductionUnit unit = new ProductionUnit("Example Unit", "example/path", 20, 25, 30, 35, 40);

            Assert.Equal("Example Unit", unit.Name);
            Assert.Equal("example/path", unit.PicturePath);
            Assert.Equal(20, unit.MaxHeat);
            Assert.Equal(25, unit.ProductionCost);
            Assert.Equal(30, unit.MaxElectricity);
            Assert.Equal(35, unit.GasConsumption);
            Assert.Equal(40, unit.Co2Emission);
            Assert.Equal(0, unit.OperationPoint);
        }

        [Fact]
        public void OperationPoint_ValidRange()
        {
            ProductionUnit unit = new ProductionUnit("Example Unit", "example/path", 20, 25, 30, 35, 40);

            unit.OperationPoint = 0.5;
            Assert.Equal(0.5, unit.OperationPoint);
        }

        [Fact]
        public void OperationPoint_OutOfRange()
        {
            ProductionUnit unit = new ProductionUnit("Example Unit", "example/path", 20, 25, 30, 35, 40);

            Assert.Throws<ArgumentOutOfRangeException>(() => unit.OperationPoint = 1.1);
            Assert.Throws<ArgumentOutOfRangeException>(() => unit.OperationPoint = -0.1);
        }
    }
}
