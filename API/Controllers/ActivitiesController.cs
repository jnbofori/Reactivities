using Domain;
using Microsoft.AspNetCore.Mvc;
using Persistence;
using Microsoft.EntityFrameworkCore;
using MediatR;
using Application.Activities;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
  [AllowAnonymous]
  public class ActivitiesController: BaseApiController
  {
    // note: IAction result specifies http response (i think)
    [HttpGet]
    public async Task<IActionResult> GetActivities() {
      // note: this sends query to mediator handler
      return HandleResult(await Mediator.Send(new List.Query()));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetActivity(Guid id) {
      var result = await Mediator.Send(new Details.Query{ Id = id });

      return HandleResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateActivity(Activity activity) {
      return HandleResult(await Mediator.Send(new Create.Command{ activity = activity }) );
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> EditActivity(Guid id, Activity activity) {
      activity.Id = id;
      return HandleResult(await Mediator.Send(new Edit.Command{ activity = activity }) );
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteActivity(Guid id) {
      return HandleResult(await Mediator.Send(new Delete.Command{ Id = id }));
    }
  }
}