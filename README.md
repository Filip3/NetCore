# NetCore
This sample app is intended for basic exploration of the .NET core and implementing features.

###### Description of the sample

It is a sample started from a basic empty .NET Core project. It is used to implement every feature step by step, for testing purposes, with the least amount of code generation.
The app instead of using a dependency library, like for example SimpleInjector, we implement the dependency injection of .NET Core.
[SimpleInjector](https://github.com/simpleinjector/SimpleInjector)

You can find the implementation of the dependenices in the \Startup.cs, under the ConfigureServices. Where you can see a simple implementation just for demonstration purposes.

The app uses the NLog logging platform for logging, 
[NLog](https://github.com/NLog/NLog)

You can find the configuration of the NLog in the nlog.config, where you can see the configuration and a simple logger and some rules, for demonstration purposes in implementation it in .NET Core.

###### Prerequisites

This sample requires:
 
- Visual Studio


###### To start the sample

- Clone the project
- Restore nuget packages 
- Build the sample
- Run and test the sample

###### Change log

- July 2017
- August 2017
