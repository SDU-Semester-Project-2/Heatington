using Heatington.Controllers.Serializers;

namespace Heatington.Tests.Serializers;

public class TestOneConstructor : IEquatable<TestOneConstructor>
{
    public int A { get; set; }
    public string B { get; set; }
    public DateTime C { get; set; }
    public TestOneConstructor(int A, string B, DateTime C)
    {
        this.A = A;
        this.B = B;
        this.C = C;
    }

    public override bool Equals(object? obj) => this.Equals(obj as TestOneConstructor);
    public bool Equals(TestOneConstructor? obj)
    {
        if (this is null && obj is null)
        {
            return true;
        }
        return obj is not null && this is not null && this.A == obj.A && this.B == obj.B && this.C == obj.C;
    }

    public override int GetHashCode() => (A, B, C).GetHashCode();
}

public class TestMultipleConstructorsWithSingleSelected : TestOneConstructor
{
    public TestMultipleConstructorsWithSingleSelected(int A, string B, DateTime C) : base(A, B, C) { }

    [CsvConstructor]
    public TestMultipleConstructorsWithSingleSelected(int A, string B) : base(A, B, new()) { }

    public TestMultipleConstructorsWithSingleSelected(int A) : base(A, "default", new()) { }
}

public class TestMultipleConstructorsWithNoneSelected : TestOneConstructor
{
    public TestMultipleConstructorsWithNoneSelected(int A, string B, DateTime C) : base(A, B, C) { }

    public TestMultipleConstructorsWithNoneSelected(int A, string B) : base(A, B, new()) { }

    public TestMultipleConstructorsWithNoneSelected(int A) : base(A, "default", new()) { }
}

public class TestMultipleConstructorsWithMultipleSelected : TestOneConstructor
{
    [CsvConstructor]
    public TestMultipleConstructorsWithMultipleSelected(int A, string B, DateTime C) : base(A, B, C) { }

    [CsvConstructor]
    public TestMultipleConstructorsWithMultipleSelected(int A, string B) : base(A, B, new()) { }

    public TestMultipleConstructorsWithMultipleSelected(int A) : base(A, "default", new()) { }
}

public class CsvDataTests
{
    [Fact]
    public void ValidCsvWithoutHeader_CsvData_ConstructsCorrectCsvData()
    {
        //Arrange
        List<string[]> table = new List<string[]> {
            new string[] {"abcd", "efgh", "ijkl"},
            new string[] {"mnop", "qrs", "tu"}
            };
        //Act
        CsvData result = new CsvData(table);
        //Assert
        Assert.Null(result.Header);
        Assert.Equal(table, result.Table);
    }

    [Fact]
    public void ValidCsvWithHeader_CsvData_ConstructsCorrectCsvData()
    {
        //Arrange
        List<string[]> table = new List<string[]> {
            new string[] {"abcd", "efgh", "ijkl"},
            new string[] {"mnop", "qrs", "tu"}
            };
        string[] header = new string[] { "First", "Second", "Third" };
        //Act
        CsvData result = new CsvData(table, header);
        //Assert
        Assert.Equal(header, result.Header);
        Assert.Equal(table, result.Table);
    }

    [Fact]
    public void ValidCsvWithOnlytHeader_CsvData_ConstructsCorrectCsvData()
    {
        //Arrange
        List<string[]> table = new List<string[]> { };
        string[] header = new string[] { "First", "Second", "Third" };
        //Act
        CsvData result = new CsvData(table, header);
        //Assert
        Assert.Equal(header, result.Header);
        Assert.Equal(table, result.Table);
    }

    [Fact]
    public void InconsistentNumberOfFieldsInCsvTable_CsvData_ThrowsException()
    {
        //Arrange
        List<string[]> table = new List<string[]> {
            new string[] {"abcd", "efgh", "ijkl"},
            new string[] {"mnop", "qrs", "tu", "extra"},
            new string[] {"vw", "x", ""}
            };
        string[] header = new string[] { "First", "Second", "Third" };
        //Act
        //Assert
        Assert.Throws<Exception>(() => new CsvData(table, header));
    }

    [Fact]
    public void NumberOfFieldsInHeaderInconsistentWithTable_CsvData_ThrowsException()
    {
        //Arrange
        List<string[]> table = new List<string[]> {
            new string[] {"abcd", "efgh", "ijkl"},
            new string[] {"mnop", "qrs", "tu"},
            new string[] {"vw", "x", ""}
            };
        string[] header = new string[] { "First", "Second", "Third", "Extra" };
        //Act
        //Assert
        Assert.Throws<Exception>(() => new CsvData(table, header));
    }

    [Fact]
    public void ListOfGenericTypeWithoutSpecifiedHeader_Create_ConstructsCorrectCsvData()
    {
        //Arrange
        List<TestOneConstructor> table = new List<TestOneConstructor> {
            new(5, "test", new DateTime(2024, 1, 20, 13, 45, 10)),
            new(-123, "Lorem", new DateTime(2024, 3, 14, 15, 42, 45)),
            new(987, "ipsum", new DateTime(2024, 5, 4, 5, 0, 0))
        };
        List<string[]> expectedTable = new List<string[]> {
            new string[] {"5", "test", "20.01.2024 13.45.10"},
            new string[] {"-123", "Lorem", "14.03.2024 15.42.45"},
            new string[] {"987", "ipsum", "04.05.2024 05.00.00"}
        };
        string[] expectedHeader = new string[] { "A", "B", "C" };
        //Act
        CsvData result = CsvData.Create<TestOneConstructor>(table);
        //Assert
        Assert.Equal(expectedHeader, result.Header);
        Assert.Equal(expectedTable, result.Table);
    }

    [Fact]
    public void ListOfGenericTypeWithSpecifiedHeader_Create_ConstructsCorrectCsvData()
    {
        //Arrange
        List<TestOneConstructor> table = new List<TestOneConstructor> {
            new(5, "test", new DateTime(2024, 1, 20, 13, 45, 10)),
            new(-123, "Lorem", new DateTime(2024, 3, 14, 15, 42, 45)),
            new(987, "ipsum", new DateTime(2024, 5, 4, 5, 0, 0))
        };
        List<string[]> expectedTable = new List<string[]> {
            new string[] {"5", "test", "20.01.2024 13.45.10"},
            new string[] {"-123", "Lorem", "14.03.2024 15.42.45"},
            new string[] {"987", "ipsum", "04.05.2024 05.00.00"}
        };
        string[] header = new string[] { "Some", "thing", "Else" };
        //Act
        CsvData result = CsvData.Create<TestOneConstructor>(table, header);
        //Assert
        Assert.Equal(header, result.Header);
        Assert.Equal(expectedTable, result.Table);
    }

    [Fact]
    public void CsvDataWithoutHeaderTOnlyOneConstructor_ConvertRecords_CreatesTheCorrectListT()
    {
        //Arrange
        CsvData data = new CsvData(
            new List<string[]> {
                new string[] {"5", "test", "20.01.2024 13.45.10"},
                new string[] {"-123", "Lorem", "14.03.2024 15.42.45"},
                new string[] {"987", "ipsum", "04.05.2024 05.00.00"}
        });
        List<TestOneConstructor> expectedList = new List<TestOneConstructor> {
            new(5, "test", new DateTime(2024, 1, 20, 13, 45, 10)),
            new(-123, "Lorem", new DateTime(2024, 3, 14, 15, 42, 45)),
            new(987, "ipsum", new DateTime(2024, 5, 4, 5, 0, 0))
        };
        //Act
        List<TestOneConstructor> result = data.ConvertRecords<TestOneConstructor>();
        //Assert
        Assert.Equal(expectedList, result);
    }

    [Fact]
    public void CsvDataWithHeaderTOnlyOneConstructor_ConvertRecords_CreatesTheCorrectListT()
    {
        //Arrange
        CsvData data = new CsvData(
            new List<string[]> {
                new string[] {"20.01.2024 13.45.10", "5", "test"},
                new string[] {"14.03.2024 15.42.45", "-123", "Lorem"},
                new string[] {"04.05.2024 05.00.00", "987", "ipsum"}
            },
            new string[] { "C", "A", "B" }
        );
        List<TestOneConstructor> expectedList = new List<TestOneConstructor> {
            new(5, "test", new DateTime(2024, 1, 20, 13, 45, 10)),
            new(-123, "Lorem", new DateTime(2024, 3, 14, 15, 42, 45)),
            new(987, "ipsum", new DateTime(2024, 5, 4, 5, 0, 0))
        };
        //Act
        List<TestOneConstructor> result = data.ConvertRecords<TestOneConstructor>();
        //Assert
        Assert.Equal(expectedList, result);
    }

    [Fact]
    public void CsvDataWithHeaderTMutlipleConstructorsSingleSelected_ConvertRecords_CreatesTheCorrectListT()
    {
        //Arrange
        CsvData data = new CsvData(
            new List<string[]> {
                new string[] {"5", "test"},
                new string[] {"-123", "Lorem"},
                new string[] {"987", "ipsum"}
            },
            new string[] { "A", "B" }
        );
        List<TestMultipleConstructorsWithSingleSelected> expectedList = new List<TestMultipleConstructorsWithSingleSelected> {
            new(5, "test"),
            new(-123, "Lorem"),
            new(987, "ipsum")
        };
        //Act
        List<TestMultipleConstructorsWithSingleSelected> result = data.ConvertRecords<TestMultipleConstructorsWithSingleSelected>();
        //Assert
        Assert.Equal(expectedList, result);
    }

    [Fact]
    public void CsvDataWithHeaderTMutlipleConstructorsNoneSelected_ConvertRecords_ThrowsException()
    {
        //Arrange
        CsvData data = new CsvData(
            new List<string[]> {
                new string[] {"5", "test"},
                new string[] {"-123", "Lorem"},
                new string[] {"987", "ipsum"}
            },
            new string[] { "A", "B" }
        );
        List<TestMultipleConstructorsWithNoneSelected> expectedList = new List<TestMultipleConstructorsWithNoneSelected> {
            new(5, "test"),
            new(-123, "Lorem"),
            new(987, "ipsum")
        };
        //Act

        //Assert
        Assert.Throws<Exception>(() => data.ConvertRecords<TestMultipleConstructorsWithNoneSelected>());
    }

    [Fact]
    public void CsvDataWithHeaderTMutlipleConstructorsMultipleSelected_ConvertRecords_ThrowsException()
    {
        //Arrange
        CsvData data = new CsvData(
            new List<string[]> {
                new string[] {"5", "test"},
                new string[] {"-123", "Lorem"},
                new string[] {"987", "ipsum"}
            },
            new string[] { "A", "B" }
        );
        List<TestMultipleConstructorsWithMultipleSelected> expectedList = new List<TestMultipleConstructorsWithMultipleSelected> {
            new(5, "test"),
            new(-123, "Lorem"),
            new(987, "ipsum")
        };
        //Act

        //Assert
        Assert.Throws<Exception>(() => data.ConvertRecords<TestMultipleConstructorsWithMultipleSelected>());
    }
}
