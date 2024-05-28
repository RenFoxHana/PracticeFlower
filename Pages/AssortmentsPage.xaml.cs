using Practice.Models;
using Practice.View;
using Practice.ViewModels;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Practice.Pages
{
	public partial class AssortmentsPage : Page
	{
		private PracticeContext db = new PracticeContext();
		public AssortmentsPage(PracticeContext context)
		{
			InitializeComponent();
			if (App.currentUser.IdRole == 1)
			{
				AssortmentViewModel viewModel = new AssortmentViewModel();
				DataContext = viewModel;
			}
			else
			{
				Florist florist = db.Florists.FirstOrDefault(f => f.IdUser == App.currentUser.IdUser);
				int floristId = florist.IdFlorist;
				AssortmentViewModel viewModel = new AssortmentViewModel(floristId);
				DataContext = viewModel;
			}
		}

		private void Add_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			NewAssortment newAssortment = new NewAssortment();
			newAssortment.ShowDialog();
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
				MessageBox.Show("Пожалуйста, выберите ассортимент для редактирования.");
				return;
			}

			Assortment selectedAssortment = (Assortment)dataGrid.SelectedItem;

			if (selectedAssortment != null)
			{
				EditAssortment editAssortmentWindow = new EditAssortment(selectedAssortment);
				editAssortmentWindow.ShowDialog();
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
				MessageBox.Show("Пожалуйста, выберите ассортимент для удаления.");
				return;
			}
			Assortment selectedAssortment = (Assortment)dataGrid.SelectedItem;
			MessageBoxResult result = MessageBox.Show("Удалить данные по товару: "
						+ selectedAssortment.Name, "Предупреждение", MessageBoxButton.OKCancel,
					MessageBoxImage.Warning);
			if (result == MessageBoxResult.OK)
			{

				var entryToDelete = db.PlantsAssortmentFlorists.FirstOrDefault(entry => entry.IdAssortment == selectedAssortment.IdAssortment);

				if (entryToDelete != null)
				{
					db.PlantsAssortmentFlorists.Remove(entryToDelete);
					db.SaveChanges();
				}

				db.Assortments.Remove(selectedAssortment);
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
			AssortmentViewModel viewModel = (AssortmentViewModel)DataContext;
			viewModel.ListAssortment.Clear();

			var assortments = db.Assortments.ToList();

			foreach (var assortment in assortments)
			{
				viewModel.ListAssortment.Add(assortment);
			}
		}

		public void RefreshFlorist()
		{
			AssortmentViewModel viewModel = (AssortmentViewModel)DataContext;
			viewModel.ListAssortment.Clear();

			var florist = db.Florists.FirstOrDefault(f => f.IdUser == App.currentUser.IdUser);
			int shopId = florist.IdShop;
			if (florist != null)
			{
				var assortments = db.PlantsAssortmentFlorists
					.Where(paf => paf.IdFloristNavigation.IdShop == shopId)
					.Select(paf => paf.IdAssortmentNavigation)
					.ToList();

				foreach (var assortment in assortments)
				{
					viewModel.ListAssortment.Add(assortment);
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

			columns[0].Width = totalAvailableWidth * 0.25;
			columns[1].Width = totalAvailableWidth * 0.25;
			columns[2].Width = totalAvailableWidth * 0.25;
			columns[3].Width = totalAvailableWidth * 0.25;
		}


	}
}
