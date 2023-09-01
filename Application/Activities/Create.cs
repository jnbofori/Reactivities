using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Domain;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Activities
{
  public class Create
  {
    public class Command: IRequest<Result<Unit>> {
      public Activity activity { get; set; }
    }

    public class CommandValidator: AbstractValidator<Command> {
      public CommandValidator()
      {
        RuleFor(x => x.activity).SetValidator(new ActivityValidator());
      }
    }

    public class Handler : IRequestHandler<Command, Result<Unit>> {
      private readonly DataContext _context;
      public Handler(DataContext context) {
        _context = context;
      }

      public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
      {
        // note: this method doesn't access the db at this point. We're just adding the activity in memory 
        _context.Activities.Add(request.activity);

        // note: this is where data is actually saved in db
        var result = await _context.SaveChangesAsync() > 0;

        if (!result) return Result<Unit>.Failure("Failed to create activity");
        
        return Result<Unit>.Success(Unit.Value);
      }
    }
  }
}