using EVoting6.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace EVoting6.Web.Controllers;

public class VoteController : Controller
{
    private ClientVotingService _clientVotingService;
    
    public VoteController(ClientVotingService cvs)
    {
        _clientVotingService = cvs;
    }
    
    [HttpGet]
    [Route("vote/")]
    public IActionResult Index()
    {
        return Ok(_clientVotingService.GetCandidates());
    }
    
    [HttpPost]
    [Route("vote/")]
    public IActionResult Vote([FromBody]int candidate)
    {
        var token = Request.Headers.Authorization.First();

        try
        {
            var msg = _clientVotingService.CreateMessage(token, candidate);
            _clientVotingService.SendBulletin(msg);
        }
        catch(Exception e)
        {
            return BadRequest(e);
        }

        return Ok();
    }
    
    [HttpGet]
    [Route("vote/results")]
    public IActionResult Results()
    {
        try
        {
            return Ok(_clientVotingService.GetResults());
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }
}