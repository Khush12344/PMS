using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PMS.Data;
using PMS.Web.Models.Entities;
using System.Security.Claims;

namespace PMS.Web.Pages.Portal
{
    [Authorize(Roles = "Public")]
    public class MyComplaintsModel : PageModel
    {
        private readonly AppDbContext _db;
        public MyComplaintsModel(AppDbContext db) => _db = db;

        public List<Petition> Complaints { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var mobile = User.FindFirst(ClaimTypes.MobilePhone)?.Value ?? string.Empty;

            Complaints = await _db.Petitions
                .Include(p => p.ComplaintCategory)
                .Where(p => p.MobileNumber == mobile)
                .OrderByDescending(p => p.SubmittedAt)
                .ToListAsync();

            return Page();
        }
    }
}
