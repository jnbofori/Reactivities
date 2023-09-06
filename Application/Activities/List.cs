using Domain;
using MediatR;
using Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace Application.Activities
{
  public class List
  {
    public class Query : IRequest<Result<List<ActivityDto>>> { }

    public class Handler : IRequestHandler<Query, Result<List<ActivityDto>>>
    {
      private readonly DataContext _context;
      private readonly IMapper _mapper;

      public Handler(DataContext context, IMapper mapper)
      {
        _mapper = mapper;
        _context = context;
      }

      // note: what cancellationToken does is, if user cancels request before it is completed,
      // it should stop the process
      public async Task<Result<List<ActivityDto>>> Handle(Query request, CancellationToken cancellationToken)
      {
        var activities = await _context.Activities
          .ProjectTo<ActivityDto>(_mapper.ConfigurationProvider)
          .ToListAsync(cancellationToken);
        // note: query above creates an infinite loop
        // Activities have Attendees which also relate to AppUsers and AppUsers have Activities 
        // and so on and so forth. Hence the autoMapper to map to new object/object type
        return Result<List<ActivityDto>>.Success(activities);
      }
    }
  }
}