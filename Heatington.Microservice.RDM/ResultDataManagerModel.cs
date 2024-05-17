// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Heatington.Controllers;
using Heatington.Helpers;

namespace Heatington.Microservice.RDM;

public class ResultDataManagerModel
{
    public static ResultDataManager.RDM? Rdm;
    private static ResultDataManagerService _service = new ResultDataManagerService();

    public static void LoadResultDataManager()
    {
        //CSV Controller
        string path = Utilities.GetAbsolutePathToAssetsDirectory();
        CsvController csvController = new CsvController(path);

        //Result Data Manager
        Rdm = new ResultDataManager.RDM(csvController);
        Console.WriteLine("YOOOO RDM STARTED");

        //Load data
        var optData = _service.GetDataFromOpt();
        Rdm.FetchOptimizationData(optData.Result);

        //optData.Result.ForEach(Console.WriteLine);
    }
}
