using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Gov_Bot_Auth_DL_Secure_Site_MVC_Core.Models;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text;

namespace Gov_Bot_Auth_DL_Secure_Site_MVC_Core.Controllers
{
    public class HomeController : Controller
    {
        private const string AzureUsGovDirectLineUrl = "https://directline.botframework.azure.us/v3/directline/tokens/generate";

        public IConfiguration Configuration { get; set; }
        public HttpClient Client { get; set; }

        public HomeController(IConfiguration configuration, HttpClient httpClient)
        {
            Configuration = configuration;
            Client = httpClient;
        }

        public IActionResult Index()
        {
            var directLineToken = string.Empty;

            // Generate unique user ID
            var userId = $"dl_{Guid.NewGuid()}";

            // Add Direct Line secret as the Bearer token in the Authorization header
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                Configuration["directLineSecret"]);

            // Add Enhanced Authentication
            var userIdObject = new { User = new { Id = userId } };
            var stringContent = JsonConvert.SerializeObject(userIdObject);

            // Send request for token
            var response = Client.PostAsync(
                AzureUsGovDirectLineUrl,
                new StringContent(stringContent, Encoding.UTF8, "application/json")).Result;

            // Get token
            if (response.IsSuccessStatusCode)
            {
                var responseString = response.Content.ReadAsStringAsync().Result;
                var directLineResponse = JsonConvert.DeserializeObject<DirectLineConversation>(responseString);
                directLineToken = directLineResponse.Token;
            }

            // Set config with token value and user ID
            var config = new WebChatConfig()
            {
                Token = directLineToken,
                UserId = userId
            };

            return View(config);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
