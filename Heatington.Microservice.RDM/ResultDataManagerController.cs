// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Heatington.Models;
using Microsoft.AspNetCore.Mvc;

namespace Heatington.Microservice.RDM;

[Route("api/[controller]")]
[ApiController]
public class ResultDataManagerController
{
    [HttpGet]
    public ActionResult<IEnumerable<FormatedResultHolder>> GetFormatedResults()
    {
        return ResultDataManagerModel.Rdm.

        /*List<FormatedResultHolder> troie = new List<FormatedResultHolder>();

        troie.Add(new FormatedResultHolder(DateTime.Now, DateTime.Now, 23,23, null,23));

        return troie;*/
    }
}
