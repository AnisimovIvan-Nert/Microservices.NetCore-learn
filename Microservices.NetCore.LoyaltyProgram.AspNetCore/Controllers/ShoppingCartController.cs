using Microservices.NetCore.LoyaltyProgram.Shared.Users;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.NetCore.LoyaltyProgram.AspNetCore.Controllers;

[ApiController]
[Route("users")]
public class ShoppingCartController(IUsersService usersService) : ControllerBase
{
    [HttpGet]
    public ValueTask<IEnumerable<LoyaltyProgramUser>>  GetAll()
    {
        return usersService.GetAll();
    }
    
    [HttpGet("{id:int}")]
    public async ValueTask<IActionResult>  Get(int id)
    {
        var user = await usersService.TryGet(id);
        if (user == null)
            return NotFound();

        return Ok(user);
    }

    [HttpPost]
    public async ValueTask<IActionResult> Create(LoyaltyProgramUser user)
    {
        await usersService.Register(user);
        
        var routeValues = new { user.Id };
        return CreatedAtAction(nameof(Get), routeValues, user);
    }

    [HttpPut("{id:int}")]
    public async ValueTask<LoyaltyProgramUser> Update(int id, LoyaltyProgramUser user)
    {
        await usersService.Update(id, user);
        return user;
    }
}