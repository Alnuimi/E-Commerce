using System;
using System.Security.Claims;
using API.DTOs;
using Core.Entities;
using Microsoft.AspNetCore.Authorization;
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
    [HttpGet("notfound")]
    public IActionResult GetNotFoundError()
    {
       return NotFound();
    }
     [HttpPost("validationerror")]
    public IActionResult GetValidationError(CreateProductDto product)
    {
        return Ok();
      //return BadRequest();
    }

    [Authorize]
    [HttpGet("secret")]

    public IActionResult GetSecret()
    {
        var user = User.FindFirst(ClaimTypes.Name)?.Value;
        var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return Ok("Hello "+user +" with the id of "+id);
    }
}
