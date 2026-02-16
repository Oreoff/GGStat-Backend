using GGStat_Backend.Data;
using MediatR;

namespace GGStat_Backend.controllers;
public record GetCountryTopQuery() : IRequest<List<CountryTop>>;
public class GetCountryTopHandler : IRequestHandler<GetCountryTopQuery, List<CountryTop>>
{
    private readonly IReadStore _readStore;

    public GetCountryTopHandler(IReadStore readStore)
    {
        _readStore = readStore;
    }

    public Task<List<CountryTop>> Handle(GetCountryTopQuery request, CancellationToken ct)
    {
        var result = _readStore.CountryTops
            .ToList();

        return Task.FromResult(result);
    }
}