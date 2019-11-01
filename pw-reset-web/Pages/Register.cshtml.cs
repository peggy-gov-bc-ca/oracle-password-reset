using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using pw_reset_web.Models;
using pw_reset_web.Services;

namespace pw_reset_web.Pages
{
    public class RegiserModel : PageModel
    {

        public string Message { get; set; }

        [BindProperty]
        public UserRegisterModel userRegisterModel { get; set; }

        public RegiserModel()
        {
        }

        public IActionResult OnGet()
        {
            userRegisterModel = new UserRegisterModel();
            byte[] bytes;
            HttpContext.Session.TryGetValue("_username", out bytes);
            if (bytes != null)
            {
                userRegisterModel.UserName = Encoding.ASCII.GetString(bytes);
            }
            return Page();          
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            ApiResult result = new ApiResult();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constants.API_BASE_URL);
                UserRegisterInfo registerInfo = userRegisterModel.ToInfo();
                using (var response = await client.PostAsJsonAsync<UserRegisterInfo>("Register", registerInfo))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<ApiResult>(apiResponse);
                    if (result.resultCode == ResultCode.SUCCESS)
                    {
                        HttpContext.Session.Set("_msg", Encoding.ASCII.GetBytes("You have been successfully registered for the Oracle DB Password Reset Service."));
                        HttpContext.Session.Set("_username", null);
                        return RedirectToPage("./Result");
                    }
                   
                }
            }
            ViewData["Error"] = result.errMsg;
            return Page();
        }
    }
}
