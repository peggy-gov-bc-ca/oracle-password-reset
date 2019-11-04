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
using System.Text;
using Microsoft.Extensions.Logging;

namespace pw_reset_web.Pages
{
    public class ResetPasswordModel : PageModel
    {
        private readonly ILogger _logger;
        public ResetPasswordModel(ILogger<ResetPasswordModel> logger)
        {
            this._logger = logger;
        }

        //public IActionResult OnGet()
        //public async Task<IActionResult> OnGet()
        public IActionResult OnGet()
        {
            userResetPwdModel = new UserResetPwdModel();
            byte[] bytes;
            _logger.LogInformation("get username value from session");
            HttpContext.Session.TryGetValue("_username", out bytes);
            if (bytes != null)
            {
                userResetPwdModel.UserName = Encoding.ASCII.GetString(bytes);
                _logger.LogInformation("get username value from session"+userResetPwdModel.UserName);
                _logger.LogInformation(userResetPwdModel.ToString());
                return Page();
            }
            else 
            {
                HttpContext.Session.Set("_msg", Encoding.ASCII.GetBytes("You have to start with Password Reset Service Landing Page."));
                return RedirectToPage("./Result");
            }           
        }

        [BindProperty]
        public UserResetPwdModel userResetPwdModel { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            _logger.LogInformation("Post from page");
            if (!ModelState.IsValid)
            {
                _logger.LogInformation("ModelState is not valid.");
                return Page();
            }

            ApiResult result=new ApiResult();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constants.API_BASE_URL);
                UserResetPwdInfo userResetPwdInfo = userResetPwdModel.ToInfo();
                _logger.LogInformation("Call DoResetPwd.");
                using (var response = await client.PostAsJsonAsync<UserResetPwdInfo>("DoResetPwd", userResetPwdInfo))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<ApiResult>(apiResponse);
                    if (result.resultCode == ResultCode.SUCCESS)
                    {
                        HttpContext.Session.Set("_msg", Encoding.ASCII.GetBytes("Your Oracle Database Account Password has been changed!"));
                        HttpContext.Session.Set("_username", Encoding.ASCII.GetBytes(""));
                        return RedirectToPage("./Result");
                    }
                }
            }
            ViewData["Error"] = result.errMsg;
            return Page();
        }
    }
}
