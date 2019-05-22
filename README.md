![BlackRain Logo](https://avatars1.githubusercontent.com/u/45467978)

BlackRain.Logging
=================

Logging is a collection of logging providers extending [ASP.NET's Logging Extensions](https://github.com/aspnet/extensions). They can be used freely with any application using ASP's logging implementation.

Install
=======

Install the library via NuGet:

```
Install-Package BlackRain.Logging
```

If you are using ASP.NET Core, simply add the following to `CreateWebHostBuilder` in `Program.cs`:

```csharp
    .ConfigureLogging(builder => builder.AddFileLogging(options =>
    {
        options.MinimumLogLevel = LogLevel.Debug;
    }))
```

License
=======

This library is licensed permissively under the Apache 2.0 license. See [LICENSE.txt](LICENSE.txt) for full license information.