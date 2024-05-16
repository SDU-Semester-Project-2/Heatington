// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Heatington.Controllers;
using Heatington.Helpers;

namespace Heatington.Microservice.RDM;

public class ResultDataManagerModel
{
    //public static ResultDataManager.ResultDataManager? RDM;
    public static ResultDataManager.RDM? Rdm;

    public static async Task LoadResultDataManager()
    {
        //CSV Controller
        string path = Utilities.GetAbsolutePathToAssetsDirectory();
        CsvController csvController = new CsvController(path);

        //Result Data Manager
        Rdm = new ResultDataManager.RDM(csvController);
    }
}
