# ViewModel Composition Benchmarks

* Benchmark code will be linked here

All tests are run using 3 different types of application:

* regular ASP.Net Core WebAPI (MVC)
* ASP.Net Core WebAPI (MVC) + Composition
* Stand-alone Composition Gateway

Tests are run 100.000 times in a loop. Average execution times varies a little across runs. Tests are run using ASP.Net End-2-End Testing infrastructure so to avoid real HTTP traffic.

## Test #1: Single item composition, single appender

All applications return the exact same paylod, the first one through a regular WebAPI Controller, the second one using 1 single ViewModel Appender bound to a regular WebAPI Controller, the third using 1 single ViewModel Appender hosted in the Composition Gateway.

```
ASP.Net Core WebAPI (MVC) Baseline application:          2643 (ticks)
UI Composition on top of ASP.Net Core WebAPI (MVC):      2625 (ticks)
Gateway Composition as stand alone reverse proxy:        1819 (ticks)
