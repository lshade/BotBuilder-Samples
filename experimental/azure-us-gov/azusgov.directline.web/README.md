# About

This is an ASP.NET Core Web Application (Model-View-Controller) that embeds a Web App Bot running in an Azure US Government subscription using Direct Line API 3.0.

This is different than embedding a bot running in Azure Cloud because:
1. The Direct Line API endpoint in the `HomeController` must use https://directline.botframework.azure.us 
1. The domain for the implementation in JavaScript (found in `Index.cshtml`) must be "https://directline.botframework.azure.us/v3/directline"

# Instructions

To run the solution with your bot, change the value of the `"DirectLineSecret"` in `appsettings.json` to a secret key from the Direct Line channel configuration on your bot in Azure US Gov.

# Components

1. Secret value stored in `appsettings.json`
1. `services.AddSingleton(new HttpClient());` in `Startup.cs`
1. `HomeController.Index()` calls the Direct Line API to get the token and send it to the Index View
1. `Index.cshtml` uses the `userId` and `token` from the controller to render the bot using JavaScript