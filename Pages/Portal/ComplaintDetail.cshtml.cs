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
    public class ComplaintDetailModel : PageModel
    {
        private readonly AppDbContext _db;
        public ComplaintDetailModel(AppDbContext db) => _db = db;

        public Petition? Petition { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                return RedirectToPage("/Portal/MyComplaints");

            var mobile = User.FindFirst(ClaimTypes.MobilePhone)?.Value ?? string.Empty;

            Petition = await _db.Petitions
                .Include(p => p.ComplaintCategory)
                .Include(p => p.LocationDistrict)
                .Include(p => p.LocationTaluk)
                .Include(p => p.WorkflowLogs.OrderBy(l => l.ActionAt))
                .FirstOrDefaultAsync(p =>
                    p.PetitionApplicationId == id &&
                    p.MobileNumber == mobile);  // Security: only owner can view

            if (Petition == null)
                return RedirectToPage("/Portal/MyComplaints");

            return Page();
        }
    }
}
