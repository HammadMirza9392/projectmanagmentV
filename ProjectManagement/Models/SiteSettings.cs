namespace ProjectManagement.Models
{
    // Branding / footer settings shown across the app and on PDF reports.
    // Persisted to a JSON file (sitesettings.json) â€” no database migration required.
    public class SiteSettings
    {
        // The business / site name (e.g. "Al Hafiz"). Used in PDF footers.
        public string SiteName { get; set; } = "Al Hafiz";

        // When true, the developer credit line is shown on PDF report footers.
        public bool ShowDeveloperInfo { get; set; } = true;

        // Developer name shown when ShowDeveloperInfo is true.
        public string DeveloperName { get; set; } = "Hammad Mirza";

        // Developer contact number shown when ShowDeveloperInfo is true.
        public string DeveloperContact { get; set; } = "03183500557";
    }
}

