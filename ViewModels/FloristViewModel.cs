using Practice.Models;
using System.Collections.ObjectModel;

namespace Practice.ViewModels
{
	public class FloristViewModel
    {
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Patronymic { get; set; }
		public string ShopName { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }

		private PracticeContext db = new PracticeContext();
		public ObservableCollection<FloristViewModel> ListFlorist { get; set; }
		public FloristViewModel()
		{
		}

	}
}
