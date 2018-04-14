# ULIS.NET
.NET port of the [Universal Language Intelligence Service JavaScript library](https://github.com/CatalystCode/Universal-Language-Intelligence-Service)

For detailed description and use cases, please refer to [this article](https://www.microsoft.com/developerblog/2017/01/14/building-luis-models-for-unsupported-languages-with-machine-translation/).

It mostly implements the same set of functionalities, but it's not a 100% copy of the original.
The main difference is that it contains a [UlisDialog](https://github.com/peterbozso/ULIS.NET/blob/master/Ulis.Net/Ulis.Net.Dialog/UlisDialog.cs) class, which you can reuse in your own C# bots.
You can also choose which translation provider you want to use, so you can go with the one that produces the better results for your language.
Currently Microsoft and Google translators are supported, but you are free to add your own implementation by using the [ITranslatorClient](https://github.com/peterbozso/ULIS.NET/blob/master/Ulis.Net/Ulis.Net.Library/ITranslatorClient.cs) interface.

Issues, suggestions and pull requests are very welcome!

## Solution structure
### Ulis.Net.Library
#### .NET Standard 2.0 class library
Contains the core logic of communicating with the chosen translation provider and then with LUIS. It's the basis of all the other projects.
Can be used in any C# code that needs to use LUIS with an officially unsupported language.
### Ulis.Net.BulkImport
#### .NET Core 2.0 console app
Useful tool for (initial) training of your LUIS model.
Configure your keys in [appsettings.json](https://github.com/peterbozso/ULIS.NET/blob/master/Ulis.Net/Ulis.Net.BulkImport/appsettings.json).
For more details, please refer to [this article](https://www.microsoft.com/developerblog/2017/01/14/building-luis-models-for-unsupported-languages-with-machine-translation/).
### Ulis.Net.Dialog
#### .NET Framework 4.7.1 class library
Contains the UlisDialog class and every other helper classes that's needed to make it work.
Note that if you want to use it in your own bot, you have to target .NET Framework 4.7.1.
### Ulis.Net.TrainBot
#### .NET Framework 4.7.1 chatbot
A simple bot using the C# SDK that can be used for ongoing training of your bot.
Configure your keys in [Web.config](https://github.com/peterbozso/ULIS.NET/blob/master/Ulis.Net/Ulis.Net.TrainBot/Web.config).
For more details, please refer to [this article](https://www.microsoft.com/developerblog/2017/01/14/building-luis-models-for-unsupported-languages-with-machine-translation/).

## Prerequisites
* [Microsoft](https://azure.microsoft.com/en-us/services/cognitive-services/translator-text-api/) or [Google](https://cloud.google.com/translate/) translator subscription key
* [LUIS](https://www.luis.ai/) app

## Installation
* [Ulis.Net.Dialog NuGet package](https://www.nuget.org/packages/Ulis.Net.Dialog/)
  When you are using this package, you are also implicitly pulling into your project the Ulis.Net.Library one.
  Since that one is a .NET Standard library, and you are trying to use it in a C# bot, which is a .NET Framework project, you'll get runtime exceptions.
  More on this issue can be found here: https://github.com/dotnet/standard/issues/481
  Luckily, there's an easy workaround: when you compile your bot using this library for the first time, you'll get a very long warning in your "Error List" window in Visual Studio.
  It will start like: "Found conflicts between different versions of the same dependent assembly. In Visual Studio, double-click this warning (or select it and press Enter) to fix the conflicts..."
  So please do as it suggests to avoid those nasty runtime exceptions. :)
* [Ulis.Net.Library NuGet package](https://www.nuget.org/packages/Ulis.Net.Library/)

## Usage
### Ulis.Net.Dialog
```csharp
using Ulis.Net.Dialog;
using Ulis.Net.Dialog.Attributes;

[MicrosoftTranslator("Your Microsoft Translator Text API key")]
[LuisModel("Your LUIS model ID", "Your LUIS subscription key")]
[Serializable]
public class RootDialog : UlisDialog<object>
{
    [LuisIntent("")]
    [LuisIntent("None")]
    public async Task None(IDialogContext context, LuisResult result)
    {
        var message = $"Sorry, I did not understand '{result.Query}'.";

        await context.PostAsync(message);

        context.Wait(this.MessageReceived);
    }
}
```
You can also check the [Ulis.Net.TrainBot project](https://github.com/peterbozso/ULIS.NET/blob/master/Ulis.Net/Ulis.Net.TrainBot/Dialogs/RootDialog.cs) to get some ideas.
### Ulis.Net.Library
See the [Ulis.Net.BulkImport project](https://github.com/peterbozso/ULIS.NET/blob/master/Ulis.Net/Ulis.Net.BulkImport/Program.cs) for usage samples.
