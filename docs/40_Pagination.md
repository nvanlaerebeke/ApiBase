# Pagination

Pagination using the query string parameters can automatically be added by adding the following parameter:

```c#
GetAll([FromQuery] PaginationQuery paginationQuery, ...)
```


This adds the following optional parameters in the paginationQuery variable if provided:

- PageNumber
- PageSize


An example of using this can be found in the Sample API project:

```c#
public async Task<IActionResult> GetAll([FromQuery] PaginationQuery paginationQuery, [FromQuery] SortQuery sortQuery) {
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

The first step is going from the `PaginationQuery` class that defines the API contract to the internal `PaginationFilter` object.  
Next is passing that along to the code for your solution that will do the paging.  
  
There is a class `PagedResult` available to make integration between your solution and the `ApiBase` easier.  
  
The `PageResponse` expects the data, total number of items available and the filter that was originally passed.
