namespace GMTWEB.Models
{
    public class DashboardViewModel
    {
        public int WebsitesCount { get; set; }
        public int UsersCount { get; set; }
        public int BlogPostsCount { get; set; }
        public List<int> MonthlyWebsiteData { get; set; } = new List<int>();
        public List<int> MonthlyBlogPostData { get; set; } = new List<int>();

        public List<string> ProjectTypeLabels { get; set; } = new List<string>();
        public List<int> ProjectTypeData { get; set; } = new List<int>();
    }
}
