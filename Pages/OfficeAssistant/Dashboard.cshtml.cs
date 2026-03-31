using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PMS.Data;
using PMS.Web.Models.Entities;
using PMS.Web.Models.Enums;
using System.Security.Claims;

namespace PMS.Web.Pages.OfficeAssistant
{
    [Authorize(Roles = "OfficeAssistant")]
    public class DashboardModel : PageModel
    {
        private readonly AppDbContext _db;

        public DashboardModel(AppDbContext db)
        {
            _db = db;
        }

        public List<Petition> NewPetitions { get; set; } = new();
        public List<Petition> UnderScrutiny { get; set; } = new();
        public List<Petition> ForAction { get; set; } = new();
        public List<Petition> ForDrop { get; set; } = new();
        public List<Petition> OADropped { get; set; } = new();

        public int CountNew => NewPetitions.Count;
        public int CountUnderScrutiny => UnderScrutiny.Count;
        public int CountForAction => ForAction.Count;
        public int CountForDrop => ForDrop.Count;
        public int CountOADropped => OADropped.Count;

        public async Task<IActionResult> OnGetAsync()
        {
            // 🔥 SAFELY GET USER ID
            var userIdClaim = User.FindFirst("UserId")?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
            {
                // fallback safety
                return RedirectToPage("/Account/Login");
            }

            int oaUserId = int.Parse(userIdClaim);

            // 🔥 FETCH ONLY ASSIGNED PETITIONS
            var all = await _db.Petitions
                .Include(p => p.ComplaintCategory)
                .Include(p => p.LocationDistrict)
                .Include(p => p.AssignedOA)
                .Where(p => p.AssignedOAId == oaUserId)
                .OrderByDescending(p => p.SubmittedAt)
                .ToListAsync();

            // 🔥 STATUS MAPPING (UPDATED FLOW)

            // Manager assigned → appears here
            NewPetitions = all
                .Where(p => p.Status == PetitionStatus.AssignedToOA)
                .ToList();

            // After clicking Scrutinize
            UnderScrutiny = all
                .Where(p => p.Status == PetitionStatus.UnderScrutiny)
                .ToList();

            // Recommended to APCCF
            ForAction = all
                .Where(p => p.Status == PetitionStatus.RecommendedForAction)
                .ToList();

            ForDrop = all
                .Where(p => p.Status == PetitionStatus.RecommendedForDrop)
                .ToList();

            // Dropped by OA
            OADropped = all
                .Where(p => p.Status == PetitionStatus.OADropped)
                .ToList();

            return Page();
        }
    }
}