<h2>ServiceResult Example</h2>

This project contains an example ASP.Net Core API with three controller actions. Each action returns the same data, but uses a different mechanism for handling errors:

* `[GET] /api/Get1/` - Controller handles exceptions thrown by primary service layer
* `[GET] /api/Get2/` - Service returns naive wrapper object
* `[GET] /api/Get3/` - Service returns sophisticated wrapper object

Start at [WeatherForecastController](./ServiceResultExample/Controllers/WeatherForecastController.cs) and work your way through from `Get1()` down. Comments throughout the code explain the various error handling mechanisms and attempt to explain why the third is the most sophisticated.

Swagger page is available at `/swagger/index.html` if you want to debug through the code.