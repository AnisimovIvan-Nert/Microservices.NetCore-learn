using Microservices.NetCore.LoyaltyProgram.Services.LoyaltyProgramUser;
using Microsoft.AspNetCore.Mvc;
using LoyaltyProgramUser = Microservices.NetCore.LoyaltyProgram.Model.LoyaltyProgramUser;
using Model_LoyaltyProgramUser = Microservices.NetCore.LoyaltyProgram.Model.LoyaltyProgramUser;

namespace Microservices.NetCore.LoyaltyProgram.Controllers;

[ApiController]
[Route("users")]
public class ShoppingCartController(ILoyaltyProgramUserService loyaltyProgramUserService) : ControllerBase
{
    [HttpGet]
    public ValueTask<IEnumerable<Model_LoyaltyProgramUser>>  GetAll()
    {
        return loyaltyProgramUserService.GetAll();
    }
    
    [HttpGet("{id:int}")]
    public async ValueTask<IActionResult>  Get(int id)
    {
        var user = await loyaltyProgramUserService.TryGet(id);
        if (user == null)
            return NotFound();

        return Ok(user);
    }

    [HttpPost]
    public async ValueTask<IActionResult> Create(Model_LoyaltyProgramUser user)
    {
        await loyaltyProgramUserService.Register(user);
        
        var routeValues = new { user.Id };
        return CreatedAtAction(nameof(Get), routeValues, user);
    }

    [HttpPut("{id:int}")]
    public async ValueTask<Model_LoyaltyProgramUser> Update(int id, Model_LoyaltyProgramUser user)
    {
        await loyaltyProgramUserService.Update(id, user);
        return user;
    }
}