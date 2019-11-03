using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using pw_reset_web.Models;
using System.Net.Http;
using Newtonsoft.Json;
using common;
using Microsoft.Extensions.Logging;
using System.Text;

namespace pw_reset_web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger _logger;


        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            //testing code
            //userEmailModel = new UserEmailModel();
            //userEmailModel.UserName = "JONNY";
            //userEmailModel.EmailAddr = "sDS@ADTE.COM";
            //testing
            return Page();
        }

        [BindProperty]
        public UserEmailModel userEmailModel { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            ApiResult result=new ApiResult();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constants.API_BASE_URL);
                UserEmailInfo usernameEmail = userEmailModel.ToInfo();
                _logger.LogDebug("start post ResetPwd");
                using (var response = await client.PostAsJsonAsync<UserEmailInfo>("ResetPwd", usernameEmail))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<ApiResult>(apiResponse);
                    _logger.LogDebug("ResetPwd returned result"+apiResponse);
                    if (result.resultCode == ResultCode.SUCCESS)
                    {
                        HttpContext.Session.Set("_username", Encoding.ASCII.GetBytes(usernameEmail.UserName));
                        return RedirectToPage("./VerifyCode");
                    }
                }
            }
            ViewData["Error"] = result.errMsg;
            _logger.LogInformation(result.errMsg);
            return Page();
        }
    }
}
