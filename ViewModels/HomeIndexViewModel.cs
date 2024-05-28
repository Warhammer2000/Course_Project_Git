namespace CourseProjectItems.ViewModels
{
    public class HomeIndexViewModel
    {
        public SearchViewModel SearchResults { get; set; }
        public IEnumerable<CollectionViewModel> Collections { get; set; }
    }
}
