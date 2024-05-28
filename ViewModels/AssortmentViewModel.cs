using Practice.Models;
using System.Collections.ObjectModel;

namespace Practice.ViewModels
{
	public class AssortmentViewModel
    {
		private PracticeContext db = new PracticeContext();
		public ObservableCollection<Assortment> ListAssortment { get; set; }

		public AssortmentViewModel()
		{
			ListAssortment = new ObservableCollection<Assortment>(db.Assortments.ToList());
		}
		public AssortmentViewModel(int floristId)
		{
			Florist florist = db.Florists.FirstOrDefault(f => f.IdUser == App.currentUser.IdUser);
			int shopId = florist.IdShop;
			ListAssortment = new ObservableCollection<Assortment>(
				db.PlantsAssortmentFlorists
					.Where(paf => paf.IdFloristNavigation.IdShop == shopId) // Сравниваем айди магазина у флориста с айди магазина у текущего флориста
					.Select(paf => paf.IdAssortmentNavigation) // Выбираем ассортименты
					.ToList()
			);
		}


	}
}
