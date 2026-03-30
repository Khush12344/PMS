namespace PMS.Web.Models.Entities
{
    public class District
    {
        public int DistrictId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? NameKN { get; set; }
        public string? KGISCode { get; set; }
        public bool IsActive { get; set; } = true;

        // Navigation
        public ICollection<Taluk> Taluks { get; set; } = new List<Taluk>();
        public ICollection<Town> Towns   { get; set; } = new List<Town>();
    }

    public class Taluk
    {
        public int TalukId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? NameKN { get; set; }
        public string? MasterCode { get; set; }
        public int DistrictId { get; set; }
        public bool IsActive { get; set; } = true;

        // Navigation
        public District District { get; set; } = null!;
        public ICollection<Hobli> Hoblis { get; set; } = new List<Hobli>();
    }

    public class Hobli
    {
        public int HobliId { get; set; }
        public string MasterCode { get; set; } = string.Empty;
        public string NameEN { get; set; } = string.Empty;
        public string? NameKN { get; set; }
        public int TalukId { get; set; }
        public int DistrictId { get; set; }

        // Navigation
        public Taluk Taluk { get; set; } = null!;
        public District District { get; set; } = null!;
        public ICollection<Village> Villages { get; set; } = new List<Village>();
    }

    public class Village
    {
        public int VillageId { get; set; }
        public string MasterCode { get; set; } = string.Empty;
        public string NameEN { get; set; } = string.Empty;
        public string? NameKN { get; set; }
        public int HobliId { get; set; }
        public int TalukId { get; set; }
        public int DistrictId { get; set; }

        // Navigation
        public Hobli Hobli { get; set; } = null!;
        public Taluk Taluk { get; set; } = null!;
        public District District { get; set; } = null!;
    }

    public class Town
    {
        public int TownId { get; set; }
        public int KGISTownId { get; set; }
        public string KGISCode { get; set; } = string.Empty;
        public string NameEN { get; set; } = string.Empty;
        public string? TownType { get; set; }   // CC, CMC, TMC, NAC, TP
        public int DistrictId { get; set; }

        // Navigation
        public District District { get; set; } = null!;
    }

    public class ComplaintCategory
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
