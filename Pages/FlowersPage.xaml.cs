using Practice.Models;
using Practice.View;
using Practice.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Practice.Pages
{
	public partial class FlowersPage : Page
    {
        public FlowersPage(PracticeContext context)
        {
			InitializeComponent();
			if (App.currentUser.IdRole == 1)
			{
				FlowersViewModel viewModel = new FlowersViewModel();
				DataContext = viewModel;
			}
			else
			{
				Florist florist = db.Florists.FirstOrDefault(f => f.IdUser == App.currentUser.IdUser);
				int floristId = florist.IdFlorist;
				FlowersViewModel viewModel = new FlowersViewModel(floristId);
				DataContext = viewModel;
			}
		}

		private PracticeContext db = new PracticeContext();

		private void Add_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			NewFlower newFlower = new NewFlower();
			newFlower.ShowDialog();
			if (App.currentUser.IdRole == 1)
			{
				Refresh();
			}
			else
			{
				RefreshFlorist();
			}
		}

		private void Edit_Click(object sender, RoutedEventArgs e)
		{
			if (dataGrid.SelectedItem == null)
			{
				MessageBox.Show("Пожалуйста, выберите цветок для редактирования.");
				return;
			}

			Flower selectedFlower = (Flower)dataGrid.SelectedItem;

			if (selectedFlower != null)
			{
				EditFlower editFlowerWindow = new EditFlower(selectedFlower);
				editFlowerWindow.ShowDialog();
				if (App.currentUser.IdRole == 1)
				{
					Refresh();
				}
				else
				{
					RefreshFlorist();
				}
			}
		}

		private void Delete_Click(object sender, RoutedEventArgs e)
		{
			if (dataGrid.SelectedItem == null)
			{
				MessageBox.Show("Пожалуйста, выберите цветок для удаления.");
				return;
			}
			Flower selectedFlower = (Flower)dataGrid.SelectedItem;
			MessageBoxResult result = MessageBox.Show("Удалить данные по цветку: "
						+ selectedFlower.Name, "Предупреждение", MessageBoxButton.OKCancel,
					MessageBoxImage.Warning);
			if (result == MessageBoxResult.OK)
			{
				var entryToDelete = db.FlowerFlorists.FirstOrDefault(entry => entry.IdFlower == selectedFlower.IdFlower);

				if (entryToDelete != null)
				{
					db.FlowerFlorists.Remove(entryToDelete); 
					db.SaveChanges();
				}


				db.Flowers.Remove(selectedFlower);
				db.SaveChanges();

				if (App.currentUser.IdRole == 1)
				{
					Refresh();
				}
				else
				{
					RefreshFlorist();
				}
			}
		}

		public void Refresh()
		{
			FlowersViewModel viewModel = (FlowersViewModel)DataContext;
			viewModel.ListFlower.Clear();

			var flowers = db.Flowers.ToList();

			foreach (var flower in flowers)
			{
				viewModel.ListFlower.Add(flower);
			}
		}

		public void RefreshFlorist()
		{
			FlowersViewModel viewModel = (FlowersViewModel)DataContext;
			viewModel.ListFlower.Clear();

			var florist = db.Florists.FirstOrDefault(f => f.IdUser == App.currentUser.IdUser);
			int shopId = florist.IdShop;
			if (florist != null)
			{
				var flowers = db.FlowerFlorists
					.Where(paf => paf.IdFloristNavigation.IdShop == shopId)
					.Select(paf => paf.IdFlowerNavigation)
					.ToList();

				foreach (var flower in flowers)
				{
					viewModel.ListFlower.Add(flower);
				}
			}
		}
		public bool IsSelectionMade { get; set; }
		private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			IsSelectionMade = dataGrid.SelectedItem != null;
		}

		private void ListView_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			var totalAvailableWidth = dataGrid.ActualWidth - SystemParameters.VerticalScrollBarWidth;

			var columns = (dataGrid.View as GridView).Columns;

			columns[0].Width = totalAvailableWidth * 0.125;
			columns[1].Width = totalAvailableWidth * 0.125;
			columns[2].Width = totalAvailableWidth * 0.125;
			columns[3].Width = totalAvailableWidth * 0.125;
			columns[4].Width = totalAvailableWidth * 0.125;
			columns[5].Width = totalAvailableWidth * 0.125;
			columns[6].Width = totalAvailableWidth * 0.125;
			columns[7].Width = totalAvailableWidth * 0.125;
		}
	}
}
