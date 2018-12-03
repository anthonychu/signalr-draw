using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SignalRDraw.Pages
{
    public class IndexModel : PageModel
    {
        [HttpGet]
        public IActionResult OnGet(string room)
        {
            if (string.IsNullOrEmpty(room))
            {
                return RedirectToPage("index", new { room = "1" });
            }

            return Page();
        }
    }
}
