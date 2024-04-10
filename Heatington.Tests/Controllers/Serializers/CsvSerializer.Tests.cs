using Heatington.Controllers.Serializers;

namespace Heatington.Tests.Serializers;

public class CsvSerializerTests
{
    public static IEnumerable<object[]> GetTests()
    {
        yield return new object[] { String.Join("\n", 
            "abc"
            ),
            new CsvData(new List<string[]>(){
                new string[] {"abc"},
            }
        )};
        yield return new object[] { String.Join("\n", 
            "a,b,c",
            "d,e,f"),
            new CsvData(new List<string[]>(){
                new string[] {"a", "b", "c"},
                new string[] {"d", "e", "f"}
            }
        )};
        yield return new object[] { String.Join("\n", 
            "a,b,c",
            "d,e,f",
            ""),
            new CsvData(new List<string[]>(){
                new string[] {"a", "b", "c"},
                new string[] {"d", "e", "f"}
            }
        )};
        yield return new object[] { String.Join("\n", 
            "a,,c",
            "d,e,f"),
            new CsvData(new List<string[]>(){
                new string[] {"a", "", "c"},
                new string[] {"d", "e", "f"}
            }
        )};
        yield return new object[] { String.Join("\n", 
            "a,,c",
            "d,e,"),
            new CsvData(new List<string[]>(){
                new string[] {"a", "", "c"},
                new string[] {"d", "e", ""}
            }
        )};
        yield return new object[] { String.Join("\n", 
            "a,,c",
            "d,e,",
            ""),
            new CsvData(new List<string[]>(){
                new string[] {"a", "", "c"},
                new string[] {"d", "e", ""}
            }
        )};
        yield return new object[] { String.Join("\n", 
            "a,bcd,\"ef\"",
            "\"gh\", ijkl  , mno "
            ),
            new CsvData(new List<string[]>(){
                new string[] {"a", "bcd", "ef"},
                new string[] {"gh", " ijkl  ", " mno "}
            }
        )};
        yield return new object[] { String.Join("\n", 
            "a,bcd,\"ef\"",
            "\"gh\", ijkl  , mno ",
            "456,\"*/\\,()\nhello=%`\", pol"
            ),
            new CsvData(new List<string[]>(){
                new string[] {"a", "bcd", "ef"},
                new string[] {"gh", " ijkl  ", " mno "},
                new string[] {"456", "*/\\,()\nhello=%`", " pol"}
            }
        )};
        yield return new object[] { String.Join("\n", 
            "a,bcd,\"ef\"",
            "\"gh\", ijkl  , mno ",
            "456,\"normal\", pol",
            "\"\"\"\"\"sometext\"\"othertext\"\"\"\"\",\"\",all"
            ),
            new CsvData(new List<string[]>(){
                new string[] {"a", "bcd", "ef"},
                new string[] {"gh", " ijkl  ", " mno "},
                new string[] {"456", "normal", " pol"},
                new string[] {"\"\"sometext\"othertext\"\"", "", "all"}
            }
        )};
        yield return new object[] { String.Join("\n", 
            "a,bcd,\"ef\"",
            "\"gh\", ijkl  , mno ",
            "456,\"*/\\,()\nhello=%`\", pol",
            "\"\"\"\"\"sometext\"\"othertext,\"\"\n\"\"\",\"\",all",
            ""
            ),
            new CsvData(new List<string[]>(){
                new string[] {"a", "bcd", "ef"},
                new string[] {"gh", " ijkl  ", " mno "},
                new string[] {"456", "*/\\,()\nhello=%`", " pol"},
                new string[] {"\"\"sometext\"othertext,\"\n\"", "", "all"}
            }
        )};
    }

    [Theory]
    [MemberData(nameof(GetTests))]
    public void ValidCsvWithoutHeader_Deserialize_ReturnsCorrectCsvData(string rawData, CsvData expected)
    {
        //Arrange
        bool includesHeader = false;
        //Act
        CsvData result = CsvSerializer.Deserialize(rawData, includesHeader);
        //Assert
        Assert.Equal(expected.Header, result.Header);
        Assert.Equal(expected.Table, result.Table);
    }

    [Fact]
    public void EmptyCsvWithoutHeader_Deserialize_ReturnsCorrectCsvData()
    {
        //Arrange
        string rawCsv = ""; 
        bool includesHeader = false;
        CsvData expected = new CsvData(new List<string[]> {} );
        //Act
        CsvData result = CsvSerializer.Deserialize(rawCsv, includesHeader);
        //Assert
        Assert.Equal(expected.Header, result.Header);
        Assert.Equal(expected.Table, result.Table);
    }

    public static IEnumerable<object[]> GetCsvTestsWithHeader()
    {
        yield return new object[] { String.Join("\n", 
            "abc"
            ),
            new CsvData(new List<string[]>(){
            },
            new string[] {"abc"}
        )};
        yield return new object[] { String.Join("\n", 
            "a,b,c",
            "d,e,f"),
            new CsvData(new List<string[]>(){
                new string[] {"d", "e", "f"}
            },
            new string[] {"a", "b", "c"}
        )};
        yield return new object[] { String.Join("\n", 
            "a,b,c",
            "d,e,f",
            ""),
            new CsvData(new List<string[]>(){
                new string[] {"d", "e", "f"}
            },
            new string[] {"a", "b", "c"}
        )};
        yield return new object[] { String.Join("\n", 
            "a,,c",
            "d,e,f"),
            new CsvData(new List<string[]>(){
                new string[] {"d", "e", "f"}
            },
            new string[] {"a", "", "c"}
        )};
        yield return new object[] { String.Join("\n", 
            "a,,c",
            "d,e,"),
            new CsvData(new List<string[]>(){
                new string[] {"d", "e", ""}
            },
            new string[] {"a", "", "c"}
        )};
        yield return new object[] { String.Join("\n", 
            "a,,c",
            "d,e,",
            ""),
            new CsvData(new List<string[]>(){
                new string[] {"d", "e", ""}
            },
            new string[] {"a", "", "c"}
        )};
        yield return new object[] { String.Join("\n", 
            "a,bcd,\"ef\"",
            "\"gh\", ijkl  , mno "
            ),
            new CsvData(new List<string[]>(){
                new string[] {"gh", " ijkl  ", " mno "}
            },
            new string[] {"a", "bcd", "ef"}
        )};
        yield return new object[] { String.Join("\n", 
            "a,bcd,\"ef\"",
            "\"gh\", ijkl  , mno ",
            "456,\"*/\\,()\nhello=%`\", pol"
            ),
            new CsvData(new List<string[]>(){
                new string[] {"gh", " ijkl  ", " mno "},
                new string[] {"456", "*/\\,()\nhello=%`", " pol"}
            },
            new string[] {"a", "bcd", "ef"}
        )};
        yield return new object[] { String.Join("\n", 
            "a,bcd,\"ef\"",
            "\"gh\", ijkl  , mno ",
            "456,\"normal\", pol",
            "\"\"\"\"\"sometext\"\"othertext\"\"\"\"\",\"\",all"
            ),
            new CsvData(new List<string[]>(){
                new string[] {"gh", " ijkl  ", " mno "},
                new string[] {"456", "normal", " pol"},
                new string[] {"\"\"sometext\"othertext\"\"", "", "all"}
            },
            new string[] {"a", "bcd", "ef"}
        )};
        yield return new object[] { String.Join("\n", 
            "a,bcd,\"ef\"",
            "\"gh\", ijkl  , mno ",
            "456,\"*/\\,()\nhello=%`\", pol",
            "\"\"\"\"\"sometext\"\"othertext,\"\"\n\"\"\",\"\",all",
            ""
            ),
            new CsvData(new List<string[]>(){
                new string[] {"gh", " ijkl  ", " mno "},
                new string[] {"456", "*/\\,()\nhello=%`", " pol"},
                new string[] {"\"\"sometext\"othertext,\"\n\"", "", "all"}
            },
            new string[] {"a", "bcd", "ef"}
        )};
    }

    [Theory]
    [MemberData(nameof(GetCsvTestsWithHeader))]
    public void ValidCsvWithHeader_Deserialize_ReturnsCorrectCsvData(string rawData, CsvData expected)
    {
        //Arrange
        bool includesHeader = true;
        //Act
        CsvData result = CsvSerializer.Deserialize(rawData, includesHeader);
        //Assert
        Assert.Equal(expected.Header, result.Header);
        Assert.Equal(expected.Table, result.Table);
    }

    [Theory]
    [InlineData(
        "abc,d\"ef,ghi\n"+
        "jklm,nopqr,st"
    )]
    [InlineData(
        "abc,def,ghi\n"+
        "jklm,\"no\"pqr\",st"
    )]
    [InlineData(
        "abc,def,ghi\n"+
        "jklm,\"no\"\"\"pqr\",st"
    )]
    [InlineData(
        "abc,def,ghi\n"+
        "jklm,\"nopqr,st"
    )]
    public void InvalidCsv_Deserialize_ThrowsException(string rawData)
    {
        //Arrange
        bool includesHeader = false;
        //Act
    
        //Assert
        Assert.Throws<Exception>(() => CsvSerializer.Deserialize(rawData, includesHeader));
    }

    [Fact]
    public void EmptyCsvWithHeader_Deserialize_ReturnsCorrectCsvData()
    {
        //Arrange
        string rawCsv = ""; 
        bool includesHeader = true;
        CsvData expected = new CsvData(new List<string[]> {} );
        //Act
        CsvData result = CsvSerializer.Deserialize(rawCsv, includesHeader);
        //Assert
        Assert.Equal(expected.Header, result.Header);
        Assert.Equal(expected.Table, result.Table);
    }

    public static IEnumerable<object[]> SerializeTests()
    {
        yield return new object[] {
            new CsvData(new List<string[]>(){new string[] {"abc"}}
            ), 
            String.Join("\n", 
            "abc"
            )
        };
        yield return new object[] {
            new CsvData(new List<string[]>(){},
            new string[] {"abc"}
            ), 
            String.Join("\n", 
            "abc\n"
            )
        };
        yield return new object[] { 
            new CsvData(new List<string[]>(){
                new string[] {"a", "b", "c"},
                new string[] {"d", "e", "f"}
            }),
            String.Join("\n", 
            "a,b,c",
            "d,e,f")
        };
        yield return new object[] { 
            new CsvData(new List<string[]>(){
                new string[] {"a", "b", "c"},
                new string[] {"d", "e", "f"}
            }),
            String.Join("\n", 
            "a,b,c",
            "d,e,f")
        };

        yield return new object[] {
            new CsvData(new List<string[]>(){
                new string[] {"a", "", "c"},
                new string[] {"d", "e", "f"}
            }), 
            String.Join("\n", 
            "a,,c",
            "d,e,f")
        };
        yield return new object[] {
            new CsvData(new List<string[]>(){
                new string[] {"a", "", "c"},
                new string[] {"d", "e", ""}
            }),
            String.Join("\n", 
            "a,,c",
            "d,e,"
            )
        
        };
        yield return new object[] {
            new CsvData(new List<string[]>(){
                new string[] {"a", "bcd", "ef"},
                new string[] {"gh", " ijkl  ", " mno "},
                new string[] {"456", "*/\\,()\nhello=%`", " pol"}
            }),
            String.Join("\n", 
            "a,bcd,ef",
            "gh, ijkl  , mno ",
            "456,\"*/\\,()\nhello=%`\", pol"
            ),
        };
        yield return new object[] {
            new CsvData(new List<string[]>(){
                new string[] {"a", "bcd", "ef"},
                new string[] {"gh", " ijkl  ", " mno "},
                new string[] {"456", "normal", " pol"},
                new string[] {"\"\"sometext\"othertext,\"\n\"", "", "all"}
            }),
             String.Join("\n", 
            "a,bcd,ef",
            "gh, ijkl  , mno ",
            "456,normal, pol",
            "\"\"\"\"\"sometext\"\"othertext,\"\"\n\"\"\",,all"

            ),

        };
    }

    [Theory]
    [MemberData(nameof(SerializeTests))]
    public void EmptyCsvWithHeader_Serialize_ReturnsCorrectString(CsvData csv, string expected)
    {
        //Arrange

        //Act
        string result = CsvSerializer.Serialize(csv);
        //Assert
        Assert.Equal(expected, result);
    }
}
