using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PMS.Data;
using PMS.Web.Models.Entities;
using PMS.Web.Models.Enums;
using System.Security.Claims;

namespace PMS.Web.Pages.FieldOfficer
{
    [Authorize(Roles = "CircleOfficer,DivisionOfficer,FMSOfficer")]
    public class DashboardModel : PageModel
    {
        private readonly AppDbContext _db;
        public DashboardModel(AppDbContext db) => _db = db;

        public List<Petition> Assigned      { get; set; } = new();
        public List<Petition> InProgress    { get; set; } = new();
        public List<Petition> PendingClosure{ get; set; } = new();
        public List<Petition> Closed        { get; set; } = new();

        public int CountAssigned       => Assigned.Count;
        public int CountInProgress     => InProgress.Count;
        public int CountPendingClosure => PendingClosure.Count;
        public int CountClosed         => Closed.Count;

        public string OfficerName { get; set; } = string.Empty;
        public string OfficerRole { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync()
        {
            var officerId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
            OfficerName = User.Identity?.Name ?? string.Empty;
            OfficerRole = User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;

            var all = await _db.Petitions
                .Include(p => p.ComplaintCategory)
                .Include(p => p.LocationDistrict)
                .Where(p => p.AssignedOfficerId == officerId)
                .OrderByDescending(p => p.AssignedAt)
                .ToListAsync();

            Assigned       = all.Where(p => p.Status == PetitionStatus.Assigned).ToList();
            InProgress     = all.Where(p => p.Status == PetitionStatus.InProgress || p.Status == PetitionStatus.Reopened).ToList();
            PendingClosure = all.Where(p => p.Status == PetitionStatus.SubmittedForClosure).ToList();
            Closed         = all.Where(p => p.Status == PetitionStatus.Closed || p.Status == PetitionStatus.Dropped).ToList();

            return Page();
        }
    }
}
