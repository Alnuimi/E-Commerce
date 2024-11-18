using System;
using API.RequestHelper;
using Core.Entities;
using Core.Interface;
using Microsoft.AspNetCore.Mvc;
namespace API.Controllers;


[ApiController]
[Route("api/[controller]")]
public class BaseApiController:ControllerBase
{
    protected async Task<ActionResult> CreatePagedResultAsync<T>(IGenericRepository<T> repository
    ,ISpecification<T> specification,int pageIndex,int pageSize) where T :BaseEntity 
    {
        var items = await repository.GetListAsync(specification);
        var count = await repository.CountAsync(specification);
         Pagination<T> pagination =new Pagination<T>(pageIndex,pageSize,count,items);
         return Ok(pagination);
    }

}
