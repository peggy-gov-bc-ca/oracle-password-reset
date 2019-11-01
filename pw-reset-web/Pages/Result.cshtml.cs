using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace pw_reset_web.Pages
{
    public class ResultModel : PageModel
    {
        public string Message { get; set; }

        public void OnGet()
        {
            byte[] bytes;
            HttpContext.Session.TryGetValue("_msg", out bytes);
            Message = Encoding.ASCII.GetString(bytes);
        }
    }
}
