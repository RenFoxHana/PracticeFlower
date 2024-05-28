using Practice.Models;
using Practice.View;
using Practice.ViewModels;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Practice.Pages
{
	public partial class FloristPage : Page
	{
		public bool IsSelectionMade { get; set; }
		public ObservableCollection<FloristViewModel> ListFlorist { get; set; }

		private PracticeContext db = new PracticeContext();
		public FloristPage(PracticeContext context)
		{
			InitializeComponent();
			ListFlorist = new ObservableCollection<FloristViewModel>();
			DataContext = this;
			Loaded += FloristPage_Loaded;
		}

		private void FloristPage_Loaded(object sender, RoutedEventArgs e)
		{
			RefreshFloristData();
		}

		public void RefreshFloristData()
		{
			ListFlorist.Clear();
			using (var context = new PracticeContext())
			{
				var florists = context.Florists
					.Select(f => new FloristViewModel
					{
						FirstName = f.FName,
						LastName = f.LName,
						Patronymic = f.Patronymic,
						ShopName = f.IdShopNavigation.NameOfShop,
						Username = f.IdUserNavigation.UserLogin,
						Password = f.IdUserNavigation.UserPassword
					})
					.ToList();

				foreach (var florist in florists)
				{
					ListFlorist.Add(florist);
				}
			}
		}

		public void Add_Click(object sender, RoutedEventArgs e)
		{
			RegisNewFlorist newWindow = new RegisNewFlorist();
			newWindow.DataUpdated += DataWindow_DataUpdated;
			newWindow.Show();
		}

		public void Edit_Click(object sender, RoutedEventArgs e)
		{
			if (!IsSelectionMade)
			{
				MessageBox.Show("Пожалуйста, выберите флориста перед редактированием.");
				return;
			}

			FloristViewModel selectedFlorist = (FloristViewModel)dataGrid.SelectedItem;

			if (selectedFlorist != null)
			{
				Florist florist = new Florist
				{
					FName = selectedFlorist.FirstName,
					LName = selectedFlorist.LastName,
					Patronymic = selectedFlorist.Patronymic,
					IdShopNavigation = new ShopsForSale { NameOfShop = selectedFlorist.ShopName },
					IdUserNavigation = new User { UserLogin = selectedFlorist.Username, UserPassword = selectedFlorist.Password }
				};

				EditFlorist editFloristWindow = new EditFlorist(florist);
				editFloristWindow.DataUpdated += DataWindow_DataUpdated;
				editFloristWindow.Show();
			}
		}
		
		private void DataWindow_DataUpdated(object sender, EventArgs e)
		{
			ListFlorist.Clear();
			using (var context = new PracticeContext())
			{
				var florists = context.Florists
					.Select(f => new FloristViewModel
					{
						FirstName = f.FName,
						LastName = f.LName,
						Patronymic = f.Patronymic,
						ShopName = f.IdShopNavigation.NameOfShop,
						Username = f.IdUserNavigation.UserLogin,
						Password = f.IdUserNavigation.UserPassword
					})
					.ToList();

				foreach (var florist in florists)
				{
					ListFlorist.Add(florist);
				}
			}
		}
		private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			IsSelectionMade = dataGrid.SelectedItem != null;
		}

		private void ListView_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			var totalAvailableWidth = dataGrid.ActualWidth - SystemParameters.VerticalScrollBarWidth;

			var columns = (dataGrid.View as GridView).Columns;

			columns[0].Width = totalAvailableWidth * 0.167;
			columns[1].Width = totalAvailableWidth * 0.167;
			columns[2].Width = totalAvailableWidth * 0.167;
			columns[3].Width = totalAvailableWidth * 0.167;
			columns[4].Width = totalAvailableWidth * 0.167;
			columns[5].Width = totalAvailableWidth * 0.167;
		}

	}
}
