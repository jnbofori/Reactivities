using Domain;
using MediatR;
using Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Application.Core;

namespace Application.Activities
{
  public class List
  {
    public class Query: IRequest<Result<List<Activity>>> {}

    public class Handler : IRequestHandler<Query, Result<List<Activity>>>
    {
      private readonly DataContext _context;

      public Handler(DataContext context) {
        _context = context;
      }

      // note: what cancellationToken does is, if user cancels request before it is completed,
      // it should stop the process
      public async Task<Result<List<Activity>>> Handle(Query request, CancellationToken cancellationToken)
      {
        return Result<List<Activity>>.Success(await _context.Activities.ToListAsync(cancellationToken));
      }
    }
  }
}