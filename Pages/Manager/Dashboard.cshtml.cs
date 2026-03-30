using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PMS.Data;
using PMS.Web.Models.Entities;
using PMS.Web.Models.Enums;

namespace PMS.Web.Pages.Manager
{
    [Authorize(Roles = "Manager")]
    public class DashboardModel : PageModel
    {
        private readonly AppDbContext _db;
        public DashboardModel(AppDbContext db) => _db = db;

        // New petitions waiting for Manager to assign OA
        public List<Petition> NewPetitions    { get; set; } = new();
        // Assigned to OA — being scrutinized
        public List<Petition> AssignedToOA    { get; set; } = new();
        // Sent back from APCCF — Manager needs to assign OA for summary
        public List<Petition> SentBack        { get; set; } = new();
        // OA submitted summary — waiting APCCF final
        public List<Petition> SummaryPending  { get; set; } = new();
        // Closed/Dropped
        public List<Petition> Closed          { get; set; } = new();

        public int CountNew           => NewPetitions.Count;
        public int CountAssignedToOA  => AssignedToOA.Count;
        public int CountSentBack      => SentBack.Count;
        public int CountSummaryPending=> SummaryPending.Count;
        public int CountClosed        => Closed.Count;

        public async Task<IActionResult> OnGetAsync()
        {
            var all = await _db.Petitions
                .Include(p => p.ComplaintCategory)
                .Include(p => p.LocationDistrict)
                .Include(p => p.AssignedOA)
                .OrderByDescending(p => p.SubmittedAt)
                .ToListAsync();

            NewPetitions   = all.Where(p => p.Status == PetitionStatus.Submitted).ToList();
            AssignedToOA   = all.Where(p =>
                p.Status == PetitionStatus.AssignedToOA ||
                p.Status == PetitionStatus.UnderScrutiny ||
                p.Status == PetitionStatus.OADropped ||
                p.Status == PetitionStatus.RecommendedForAction ||
                p.Status == PetitionStatus.RecommendedForDrop).ToList();
            SentBack       = all.Where(p => p.Status == PetitionStatus.SentToManager).ToList();
            SummaryPending = all.Where(p =>
                p.Status == PetitionStatus.PendingOASummary ||
                p.Status == PetitionStatus.SummarySubmitted).ToList();
            Closed         = all.Where(p =>
                p.Status == PetitionStatus.Closed ||
                p.Status == PetitionStatus.Dropped).ToList();

            return Page();
        }
    }
}
