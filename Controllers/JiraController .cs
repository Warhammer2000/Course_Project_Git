using CourseProjectItems.Data;
using CourseProjectItems.Models;
using CourseProjectItems.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;


namespace CourseProjectItems.Controllers
{
  public class JiraController : Controller
    {
        private readonly string _jiraBaseUrl = "https://warhammerdev.atlassian.net/";
        private readonly string _jiraApiToken = "ATATT3xFfGF0_xHI1KJzFIp2LhVxMLOgcZemoMyh90T78I58E1sb5fhSWnqlV3g48mT_oAV5UxTJuC04W6nAMtmsOWSSVITOjpIS2j48klu8aLUJAZi00RgFLs6cTSGuF6YQsVZeFpBbmUfdoEVeTTkRcOC8Xm3RqnW2tQQJ3H0_SuEK_V-mtAI=97BC97D2";
        private readonly string _jiraProjectKey = "IN";
        public string _returnUrl = "/";
        public string issueUrl;
        private readonly UserManager<ApplicationUser> _userManager;

        public JiraController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTicket(JiraTask model)
        {
            var user = await _userManager.GetUserAsync(User);
            var userEmail =  user.Email;


            var client = new HttpClient();
            var email = "rustam.r2005@gmail.com";
            var emailAndToken = $"{email}:{_jiraApiToken}";
            var accountId = String.Empty;


            var responseW = await client.GetAsync($"{_jiraBaseUrl}/rest/api/3/user/search?query={userEmail}");
            var responseContentW = await responseW.Content.ReadAsStringAsync();
            Console.WriteLine(responseContentW);

            var authToken = Convert.ToBase64String(Encoding.ASCII.GetBytes(emailAndToken));
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authToken);
            var userCheckResponse = await client.GetAsync($"{_jiraBaseUrl}/rest/api/3/user/search?query={email}");
            if (userCheckResponse.IsSuccessStatusCode)
            {
                // Проверяем содержимое ответа сервера
                // Create new user if not exists
                var newUser = new
                {
                    name = User.Identity.Name.Split('@')[0],
                    password = "123Ai@qwerty",
                    emailAddress = userEmail,
                    displayName = User.Identity.Name,
                    products = new[] { "jira-software" }
                };

                var newUserJson = JsonConvert.SerializeObject(newUser);
                var newUserContent = new StringContent(newUserJson, Encoding.UTF8, "application/json");

                var createUserResponse = await client.PostAsync($"{_jiraBaseUrl}/rest/api/3/user", newUserContent);

                if (createUserResponse.IsSuccessStatusCode)
                {
                    var rc = await createUserResponse.Content.ReadAsStringAsync();
                    var createdUser = JsonConvert.DeserializeObject<JObject>(rc);
                     accountId = createdUser.Value<string>("accountId");
                }
                else
                {
                    var errorContent = await createUserResponse.Content.ReadAsStringAsync();
                    Console.WriteLine("Error creating user: " + errorContent);
                    return Content($"Error creating user: {errorContent}");
                }
            }

            var issueData = new
            {
                fields = new
                {
                    project = new { key = _jiraProjectKey },
                    summary = model.Summary,
                    description = new
                    {
                        type = "doc",
                        version = 1,
                        content = new object[]
                        {
                new
                {
                    type = "paragraph",
                    content = new object[]
                    {
                        new
                        {
                            type = "text",
                            text = model.Description + " | "
                        },
                        new
                        {
                            type = "text",
                            text = "Click here",
                            marks = new object[]
                            {
                                new
                                {
                                    type = "link",
                                    attrs = new
                                    {
                                        href = "https://warhammerdev.atlassian.net/" 
                                    }
                                }
                            }
                        },
                        new
                        {
                            type = "text",
                            text = " to see the page it was sent from."
                        }
                    }
                }
                        }
                    },
                    issuetype = new { name = "Task" },
                    priority = new { name = model.Priority }
                }
            };

            var json = JsonConvert.SerializeObject(issueData, Formatting.Indented);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"{_jiraBaseUrl}/rest/api/3/issue", content);
            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseContent);

         
            if (response.IsSuccessStatusCode)
            {
                var createdIssue = JsonConvert.DeserializeObject<JObject>(responseContent);
                var issueKey = createdIssue.Value<string>("key");
                issueUrl = $"{_jiraBaseUrl}/browse/{issueKey}";
                return RedirectToAction("Success", new { url = issueUrl });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        public IActionResult Error()
        {
            return View("Error");
        }
        public IActionResult Success(string url)
        {
            ViewBag.IssueURl = url;    
            return View("Success");
        }
    }
    
}
