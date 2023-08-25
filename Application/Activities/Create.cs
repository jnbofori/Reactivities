using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Persistence;

namespace Application.Activities
{
  public class Create
  {
    public class Command: IRequest {
      public Activity activity { get; set; }
    }

    public class Handler : IRequestHandler<Command> {
      private readonly DataContext _context;
      public Handler(DataContext context) {
        _context = context;
      }

      public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
      {
        // note: this method doesn't access the db at this point. We're just adding the activity in memory 
        _context.Activities.Add(request.activity);

        // note: this is where data is actually saved in db
        await _context.SaveChangesAsync();

        return Unit.Value;
      }
    }
  }
}