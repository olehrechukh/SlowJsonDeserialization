# Project to demo the bug with low performance in DeserializeAsyncEnumerable.
Github issue: https://github.com/dotnet/runtime/issues/100582
# Getting Started

Clone the repo:

``` bash
git clone https://github.com/olehrechukh/SlowJsonDeserialization
```

Build the app:
``` bash
cd SlowJsonDeserialization
dotnet build -c Release
```

Launch the server:
``` 
dotnet run -c Release --project .\SlowJsonDeserialization.Api\SlowJsonDeserialization.Api.csproj
```

Launch the client (open a new terminal in the root project)
``` 
dotnet run -c Release --project .\SlowJsonDeserialization.Client\SlowJsonDeserialization.Client.csproj
```