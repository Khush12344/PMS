using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PMS.Data;
using PMS.Web.Models.Entities;
using PMS.Web.Models.Enums;

namespace PMS.Web.Pages.OfficeAssistant
{
    [Authorize(Roles = "OfficeAssistant")]
    public class DashboardModel : PageModel
    {
        private readonly AppDbContext _db;
        public DashboardModel(AppDbContext db) => _db = db;

        public List<Petition> NewPetitions   { get; set; } = new();
        public List<Petition> UnderScrutiny  { get; set; } = new();
        public List<Petition> ForAction      { get; set; } = new();
        public List<Petition> ForDrop        { get; set; } = new();
        public List<Petition> OADropped      { get; set; } = new();

        public int CountNew           => NewPetitions.Count;
        public int CountUnderScrutiny => UnderScrutiny.Count;
        public int CountForAction     => ForAction.Count;
        public int CountForDrop       => ForDrop.Count;
        public int CountOADropped     => OADropped.Count;

        public async Task<IActionResult> OnGetAsync()
        {
            var all = await _db.Petitions
                .Include(p => p.ComplaintCategory)
                .Include(p => p.LocationDistrict)
                .OrderByDescending(p => p.SubmittedAt)
                .ToListAsync();

            NewPetitions  = all.Where(p => p.Status == PetitionStatus.Submitted).ToList();
            UnderScrutiny = all.Where(p => p.Status == PetitionStatus.UnderScrutiny).ToList();
            ForAction     = all.Where(p => p.Status == PetitionStatus.RecommendedForAction).ToList();
            ForDrop       = all.Where(p => p.Status == PetitionStatus.RecommendedForDrop).ToList();
            OADropped     = all.Where(p => p.Status == PetitionStatus.OADropped).ToList();

            return Page();
        }
    }
}
