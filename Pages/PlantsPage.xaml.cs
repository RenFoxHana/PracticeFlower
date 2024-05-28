using Practice.Models;
using Practice.View;
using Practice.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Practice.Pages
{
	public partial class PlantsPage : Page
    {
		private PracticeContext db = new PracticeContext();
		public PlantsPage(PracticeContext context)
        {
            InitializeComponent();
			if (App.currentUser.IdRole == 1)
			{
				PlantViewModel viewModel = new PlantViewModel();
				DataContext = viewModel;
			}
			else
			{
				Florist florist = db.Florists.FirstOrDefault(f => f.IdUser == App.currentUser.IdUser);
				int floristId = florist.IdFlorist;
				PlantViewModel viewModel = new PlantViewModel(floristId);
				DataContext = viewModel;
			}
		}

		private void Add_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			NewPlant newPlant = new NewPlant();
			newPlant.ShowDialog();
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
				MessageBox.Show("Пожалуйста, выберите растение для редактирования.");
				return;
			}

			Plant selectedPlant = (Plant)dataGrid.SelectedItem;

			if (selectedPlant != null)
			{
				EditPlant editPlant = new EditPlant(selectedPlant);
				editPlant.ShowDialog();
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
				MessageBox.Show("Пожалуйста, выберите комнатное растение для удаления.");
				return;
			}
			Plant selectedPlant = (Plant)dataGrid.SelectedItem;
			MessageBoxResult result = MessageBox.Show("Удалить данные по растению: "
						+ selectedPlant.Name, "Предупреждение", MessageBoxButton.OKCancel,
					MessageBoxImage.Warning);
			if (result == MessageBoxResult.OK)
			{
				var entryToDelete = db.PlantsAssortmentFlorists.FirstOrDefault(entry => entry.IdPlant == selectedPlant.IdPlant);

				if (entryToDelete != null)
				{
					db.PlantsAssortmentFlorists.Remove(entryToDelete);
					db.SaveChanges();
				}

				db.Plants.Remove(selectedPlant);
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
			PlantViewModel viewModel = (PlantViewModel)DataContext;
			viewModel.ListPlant.Clear();

			var plants = db.Plants.ToList();

			foreach (var plant in plants)
			{
				viewModel.ListPlant.Add(plant);
			}
		}

		public void RefreshFlorist()
		{
			PlantViewModel viewModel = (PlantViewModel)DataContext;
			viewModel.ListPlant.Clear();

			var florist = db.Florists.FirstOrDefault(f => f.IdUser == App.currentUser.IdUser);
			int shopId = florist.IdShop;
			if (florist != null)
			{
				var plants = db.PlantsAssortmentFlorists
				.Where(paf => paf.IdFloristNavigation.IdShop == shopId)
				.Select(paf => paf.IdPlantNavigation)
				.ToList();

				foreach (var plant in plants)
				{
					viewModel.ListPlant.Add(plant);
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
