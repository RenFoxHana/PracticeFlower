using OfficeOpenXml;
using Practice.Models;
using Practice.ViewModels;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Forms;

namespace Practice.Pages
{
	public partial class ShopsPage : Page
	{
		public ObservableCollection<ShopsForSale> ListShop { get; set; }
		public ShopsPage(PracticeContext context)
		{
			InitializeComponent();
			ShopViewModel viewModel = new ShopViewModel();
			DataContext = viewModel;
			ListShop = viewModel.ListShop;
		}

		private void ToExcelButton_OnClick(object sender, RoutedEventArgs e)
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx";
			if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

				using (ExcelPackage excelPackage = new ExcelPackage())
				{
					ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Магазины");

					int row = 1;
					foreach (var shop in ListShop)
					{
						worksheet.Cells[row, 1].Value = shop.Index;
						worksheet.Cells[row, 2].Value = shop.City;
						worksheet.Cells[row, 3].Value = shop.Street;
						worksheet.Cells[row, 4].Value = shop.Building;
						worksheet.Cells[row, 5].Value = shop.NameOfShop;
						worksheet.Cells[row, 6].Value = shop.AreaOfTradingFloor;
						worksheet.Cells[row, 7].Value = shop.TypeOfSale;
						worksheet.Cells[row, 8].Value = shop.AvailabilityOfOrders;
						row++;
					}

					FileInfo excelFile = new FileInfo(saveFileDialog.FileName);
					excelPackage.SaveAs(excelFile);
				}

				System.Windows.MessageBox.Show("Данные успешно экспортированы в файл Excel.");
			}
		}

		private void RefreshShops()
		{
			ICollectionView view = CollectionViewSource.GetDefaultView(ListShop);

			if (OptovayaCheckBox.IsChecked == true && RoznichnayaCheckBox.IsChecked == true)
			{
				view.Filter = null;
			}
			else if (OptovayaCheckBox.IsChecked == true)
			{
				view.Filter = item => ((ShopsForSale)item).TypeOfSale == "Оптовая";
			}
			else if (RoznichnayaCheckBox.IsChecked == true)
			{
				view.Filter = item => ((ShopsForSale)item).TypeOfSale == "Розничная";
			}
			else
			{
				view.Filter = null;
			}
		}

		private void CheckBox_Checked(object sender, RoutedEventArgs e)
		{
			RefreshShops();
		}

		private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
		{
			RefreshShops();
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
