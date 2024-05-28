using CourseProjectItems.Models;

namespace CourseProjectItems.ViewModels
{
	public class HomeViewModel
	{
		public List<Item> LatestItems { get; set; }
		public List<Collection> LargestCollections { get; set; }
		public List<string> Tags { get; set; }
	}
}
