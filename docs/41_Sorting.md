# Sorting

Just as with paging, sorting is done by adding query string parameters.  

The parameters in question are:

- OrderBy: property name
- OrderDirection: ASC or DESC

An example:

```c#
public async Task<IActionResult> GetAll([FromQuery] PaginationQueryString paginationQuery, [FromQuery] SortQueryString sortQuery) {
    var forecasts = await Forecasts.GetAsync(
        Mapper.Map<PaginationFilter>(paginationQuery),
        Mapper.Map<SortFilter>(sortQuery)
    ).ConfigureAwait(false);

    return Ok(new PageResponse<ForecastResponse>(
        Mapper.Map<List<ForecastResponse>>(forecasts.Data),
        forecasts.TotalRecords,
        forecasts.PaginationFilter)
    );
}
```

The first step is going from the `SortQueryString` class that defines the API contract to the internal `SortQueryString` object.  
Next is passing that along to the code for your solution that will do the sorting.