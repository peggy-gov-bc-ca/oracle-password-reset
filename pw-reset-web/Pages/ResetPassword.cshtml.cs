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

namespace pw_reset_web.Pages
{
    public class ResetPasswordModel : PageModel
    {
        public ResetPasswordModel()
        {
        }

        //public IActionResult OnGet()
        public async Task<IActionResult> OnGet()
        {
            userResetPwdModel = new UserResetPwdModel();
            byte[] bytes;
            HttpContext.Session.TryGetValue("_username", out bytes);
            if (bytes != null)
            {
                userResetPwdModel.UserName = Encoding.ASCII.GetString(bytes);
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
            if (!ModelState.IsValid)
            {
                return Page();
            }

            ApiResult result=new ApiResult();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constants.API_BASE_URL);
                UserResetPwdInfo userResetPwdInfo = userResetPwdModel.ToInfo();
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
