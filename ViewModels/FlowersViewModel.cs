using Practice.Models;
using System.Collections.ObjectModel;

namespace Practice.ViewModels
{
	public class FlowersViewModel
    {
		private PracticeContext db = new PracticeContext();
		public ObservableCollection<Flower> ListFlower { get; set; }

		public FlowersViewModel()
		{
			ListFlower = new ObservableCollection<Flower>(db.Flowers.ToList());
		}

		public FlowersViewModel(int floristId)
		{
			Florist florist = db.Florists.FirstOrDefault(f => f.IdUser == App.currentUser.IdUser);

			if (florist != null)
			{
				int shopId = florist.IdShop;
				List<Flower> flowers = db.FlowerFlorists
					.Where(paf => paf.IdFloristNavigation.IdShop == shopId)
					.Select(paf => paf.IdFlowerNavigation)
					.ToList();

				if (flowers != null && flowers.Any())
				{
					ListFlower = new ObservableCollection<Flower>(flowers);
				}
				else
				{
					ListFlower = new ObservableCollection<Flower>();
				}
			}
		}


	}
}
