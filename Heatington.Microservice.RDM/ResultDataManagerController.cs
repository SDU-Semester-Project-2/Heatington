// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Heatington.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Heatington.Microservice.RDM;

[Route("api/[controller]")]
[ApiController]
public class ResultDataManagerController
{
    [HttpGet]
    public ActionResult<IEnumerable<FormatedResultHolder>> GetFormatedResults()
    {
        return ResultDataManagerModel.Rdm.FormatResults();
    }
}
