using Application.Core;
using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Activities
{
  public class Edit
  {
    public class Command : IRequest<Result<Unit>> {
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
      private readonly IMapper _mapper;
      public Handler(DataContext context, IMapper mapper) {
        _mapper = mapper;
        _context = context;
      }

      public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
      {
        var activity = await _context.Activities.FindAsync(request.activity.Id);

        if (activity == null) return null;

        _mapper.Map(request.activity, activity);

        var result = await _context.SaveChangesAsync() > 0;

        if (!result) return Result<Unit>.Failure("Failed to edit activity");

        return Result<Unit>.Success(Unit.Value);
      }
    }
  }
}