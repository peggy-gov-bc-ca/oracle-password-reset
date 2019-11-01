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
using pw_reset_web.Services;
using Microsoft.Extensions.Logging;
using System.Text;

namespace pw_reset_web.Pages
{
    public class VerifyModel : PageModel
    {
        private readonly ILogger _logger;


        public VerifyModel(ILogger<AboutModel> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> OnGet()
        {
            userVerifyModel = new UserVerifyModel();
            byte[] bytes;
            HttpContext.Session.TryGetValue("_username", out bytes);
            if (bytes != null)
            {
                userVerifyModel.UserName = Encoding.ASCII.GetString(bytes);
            }
            else 
            {
                HttpContext.Session.Set("_msg", Encoding.ASCII.GetBytes("You have to start with Password Reset Service Landing Page."));
                return RedirectToPage("./Result");
            }

            ApiResult result = new ApiResult();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constants.API_BASE_URL);
                UserResetPwdInfo info = new UserResetPwdInfo();
                info.UserName = userVerifyModel.UserName;
                using (var response = await client.PostAsJsonAsync<UserResetPwdInfo>("GetUserEmail", info))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    userVerifyModel.EmailAddr = apiResponse;
                }
            }
            return Page();
        }

        [BindProperty]
        public UserVerifyModel userVerifyModel { get; set; }

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
                UserVerifyInfo userVerifyInfo = userVerifyModel.ToInfo();
                using (var response = await client.PostAsJsonAsync<UserVerifyInfo>("VerifyCode", userVerifyInfo))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<ApiResult>(apiResponse);
                    if (result.resultCode == ResultCode.SUCCESS)
                    {
                        UserEmailModel userEmailModel = new UserEmailModel();
                        userEmailModel.UserName = userVerifyInfo.UserName;
                        userEmailModel.EmailAddr = userVerifyInfo.EmailAddr;
                        HttpContext.Session.Set("_username", Encoding.ASCII.GetBytes(userVerifyInfo.UserName));
                        return RedirectToPage("./ResetPassword");
                    }
                }
            }
            ViewData["Error"] = result.errMsg;
            _logger.LogInformation(result.errMsg);
            return Page();
        }
    }
}
