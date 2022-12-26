using EVoting6.Web.Models;
using EVoting6.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace EVoting6.Web.Controllers;

public class UserController : Controller
{
    private UserService _userService;

    public UserController(UserService us)
    {
        _userService = us;
    }
    
    [HttpPost]
    [Route("voter/register")]
    public IActionResult Register([FromBody]UserRegisterModel user)
    {
        try
        {
            var (voter, token) = _userService.Register(user.Ipn);

            return Ok(new VoterModel
            {
                Token = token,
                Login = voter.Login,
                Password = voter.Password
            });
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpPost]
    [Route("voter/login")]
    public IActionResult LogIn([FromBody]VoterModel voter)
    {
        try
        {
            return Ok(_userService.LogIn(voter.Login, voter.Password));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}