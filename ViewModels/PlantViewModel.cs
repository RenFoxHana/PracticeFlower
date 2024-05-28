using Practice.Models;
using System.Collections.ObjectModel;

namespace Practice.ViewModels
{
	public class PlantViewModel
    {
		private PracticeContext db = new PracticeContext();
		public ObservableCollection<Plant> ListPlant { get; set; }

		public PlantViewModel()
		{
			ListPlant = new ObservableCollection<Plant>(db.Plants.ToList());
		}

		public PlantViewModel(int floristId)
		{
			Florist florist = db.Florists.FirstOrDefault(f => f.IdUser == App.currentUser.IdUser);
			int shopId = florist.IdShop;
			ListPlant = new ObservableCollection<Plant>(
				db.PlantsAssortmentFlorists
					.Where(paf => paf.IdFloristNavigation.IdShop == shopId) 
					.Select(paf => paf.IdPlantNavigation) 
					.ToList()
			);

		}
	}
}
