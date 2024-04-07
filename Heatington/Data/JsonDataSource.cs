// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Heatington.Controllers;
using Heatington.Controllers.Enums;
using Heatington.Controllers.Interfaces;

namespace Heatington.Data;

public class JsonDataSource(string filePath) : IReadWriteController //UNUSED
{
    //TODO: for now both JsonController and JsonDataSource implement IReadWriteController. Remove one, after talking with team.
    private readonly IReadWriteController _fileController = new FileController(filePath);

    public async Task<T> ReadData<T>()
    {
        string data = await _fileController.ReadData<string>();
        T? result = JsonController.Deserialize<T>(data);

        return result;
    }

    public async Task<OperationStatus> WriteData<T>(T content)
    {
        string serializedData = JsonController.Serialize(content);
        return await _fileController.WriteData<string>(serializedData);
    }

    public override string? ToString()
    {
        return $"Path to file {filePath}";
    }
}
