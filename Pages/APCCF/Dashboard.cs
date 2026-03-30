using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PMS.Data;
using PMS.Web.Models.Entities;
using PMS.Web.Models.Enums;

namespace PMS.Web.Pages.APCCF
{
    [Authorize(Roles = "APCCF")]
    public class DashboardModel : PageModel
    {
        private readonly AppDbContext _db;
        public DashboardModel(AppDbContext db) => _db = db;

        // ── Section A: Direct inbox (all petitions from public) ──
        public List<Petition> DirectInbox       { get; set; } = new(); // All submitted/under scrutiny/OADropped
        public List<Petition> OADroppedPending  { get; set; } = new(); // Subset: OADropped needing APCCF review

        // ── Section B: From OA ────────────────────────────────────
        public List<Petition> FromOAForAction   { get; set; } = new();
        public List<Petition> FromOAForDrop     { get; set; } = new();
        public List<Petition> Assigned          { get; set; } = new();
        public List<Petition> PendingClosure    { get; set; } = new();
        public List<Petition> Closed            { get; set; } = new();

        // Stats
        public int CountDirectUnreviewed  => DirectInbox.Count;
        public int CountOADroppedPending  => OADroppedPending.Count;
        public int CountFromOA            => FromOAForAction.Count + FromOAForDrop.Count;
        public int CountAssigned          => Assigned.Count;
        public int CountPendingClosure    => PendingClosure.Count;
        public int CountClosed            => Closed.Count;

        public async Task<IActionResult> OnGetAsync()
        {
            var all = await _db.Petitions
                .Include(p => p.ComplaintCategory)
                .Include(p => p.LocationDistrict)
                .Include(p => p.AssignedOfficer)
                .OrderByDescending(p => p.SubmittedAt)
                .ToListAsync();

            // Section A: everything that came directly from public
            // (submitted, under scrutiny, OA-dropped, recommended — APCCF sees all)
            DirectInbox = all.Where(p =>
                p.Status == PetitionStatus.Submitted ||
                p.Status == PetitionStatus.UnderScrutiny ||
                p.Status == PetitionStatus.OADropped ||
                p.Status == PetitionStatus.RecommendedForAction ||
                p.Status == PetitionStatus.RecommendedForDrop).ToList();

            // Sub-tab: OA dropped — needs APCCF to review and potentially override
            OADroppedPending = all.Where(p => p.Status == PetitionStatus.OADropped).ToList();

            // Section B: OA has reviewed and sent with recommendation
            FromOAForAction = all.Where(p => p.Status == PetitionStatus.RecommendedForAction).ToList();
            FromOAForDrop   = all.Where(p => p.Status == PetitionStatus.RecommendedForDrop).ToList();

            Assigned       = all.Where(p => p.Status == PetitionStatus.Assigned || p.Status == PetitionStatus.InProgress).ToList();
            PendingClosure = all.Where(p => p.Status == PetitionStatus.SubmittedForClosure).ToList();
            Closed         = all.Where(p => p.Status == PetitionStatus.Closed || p.Status == PetitionStatus.Dropped).ToList();

            return Page();
        }
    }
}
