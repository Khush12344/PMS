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
    public class DashboardModel : PageModel
    {
        private readonly AppDbContext _db;

        public DashboardModel(AppDbContext db)
        {
            _db = db;
        }

        public string UserName { get; set; } = string.Empty;
        public string MobileNumber { get; set; } = string.Empty;
        public int TotalComplaints { get; set; }
        public List<Petition> RecentComplaints { get; set; } = new List<Petition>();

        public async Task<IActionResult> OnGetAsync()
        {
            // Get user details from claims
            MobileNumber = User.FindFirst(ClaimTypes.MobilePhone)?.Value ?? "";
            UserName = User.Identity?.Name ?? MobileNumber;

            // Total complaints
            TotalComplaints = await _db.Petitions
                .CountAsync(p => p.MobileNumber == MobileNumber);

            // Recent complaints (no Include, safe query)
            RecentComplaints = await _db.Petitions
                .Where(p => p.MobileNumber == MobileNumber)
                .OrderByDescending(p => p.SubmittedAt)
                .Take(5)
                .Select(p => new Petition
                {
                    PetitionId = p.PetitionId,
                    PetitionApplicationId = p.PetitionApplicationId,
                    SubmittedAt = p.SubmittedAt,
                    Status = p.Status,
                    ComplaintCategory = new ComplaintCategory
                    {
                        Name = p.ComplaintCategory.Name
                    }
                })
                .ToListAsync();

            return Page();
        }
    }
}