using System;
using API.DTOs;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class BuyygController:BaseApiController
{
    [HttpGet("unauthorized")]
    public IActionResult GetUnauthorized()
    {
        return Unauthorized();
    }
    [HttpGet("badrequest")]
    public IActionResult GetBadRequest()
    {
        return BadRequest("Not good Request");
    }

     [HttpGet("internalerror")]
    public IActionResult GetInternalError()
    {
       throw new Exception("This is test expecetion");
    }

     [HttpGet("validationerror")]
    public IActionResult GetValidationError(CreateProductDto product)
    {
        return Ok();
      //return BadRequest();
    }
}
