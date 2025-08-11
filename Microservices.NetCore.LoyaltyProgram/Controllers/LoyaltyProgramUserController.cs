using Microservices.NetCore.LoyaltyProgram.Model;
using Microservices.NetCore.LoyaltyProgram.Services.User;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.NetCore.LoyaltyProgram.Controllers;

[ApiController]
[Route("users")]
public class LoyaltyProgramUserController(ILoyaltyProgramUserService loyaltyProgramUserService) : ControllerBase
{
    [HttpGet]
    public ValueTask<IEnumerable<LoyaltyProgramUser>> GetAll()
    {
        return loyaltyProgramUserService.GetAll();
    }
    
    [HttpGet("{id:int}")]
    public async ValueTask<IActionResult> Get(int id)
    {
        var user = await loyaltyProgramUserService.TryGet(id);
        return user == null
            ? NotFound()
            : Ok(user);
    }

    [HttpPost]
    public async ValueTask<IActionResult> Create(LoyaltyProgramUser user)
    {
        var id = await loyaltyProgramUserService.Register(user);
        user.Id = id;
        
        var routeValues = new { id };
        return CreatedAtAction(nameof(Get), routeValues, user);
    }

    [HttpPut("{id:int}")]
    public async ValueTask<IActionResult> Update(int id, LoyaltyProgramUser user)
    {
        var result = await loyaltyProgramUserService.Update(id, user);
        return result 
            ? Ok(user) 
            : BadRequest();
    }
}