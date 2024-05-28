using Microsoft.EntityFrameworkCore;
using Practice.Models;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Practice.View
{
	public partial class EditPlant : Window
	{
		private PracticeContext _db;
		public Plant SelectedPlant { get; set; }
		public ObservableCollection<ShopsForSale> ListShop { get; set; }
		public ObservableCollection<Florist> ListFlorist { get; set; }
		public EditPlant(Plant selectedPlant)
		{
			SelectedPlant = selectedPlant;
			InitializeComponent();
			_db = new PracticeContext();

			if (App.currentUser.IdRole == 1)
			{
				if (selectedPlant != null)
				{
					txtName.Text = selectedPlant.Name;
					txtColor.Text = selectedPlant.Color;
					txtSoil.Text = selectedPlant.RequieredSoil;
					txtFlowering.Text = selectedPlant.FloweringPeriod;
					txtShape.Text = selectedPlant.FlowerShape;
					txtSize.Text = selectedPlant.FlowerSize;
					txtLife.Text = selectedPlant.ShelfLife;
					txtPrice.Text = selectedPlant.Price.ToString();

					ListShop = new ObservableCollection<ShopsForSale>(_db.ShopsForSales.ToList());
					cmbShop.ItemsSource = ListShop;
					cmbShop.DisplayMemberPath = "NameOfShop";
					cmbShop.SelectionChanged += CmbShop_SelectionChanged;

					PlantsAssortmentFlorist shop = _db.PlantsAssortmentFlorists.Include(paf => paf.IdFloristNavigation.IdShopNavigation).FirstOrDefault(paf => paf.IdPlant == selectedPlant.IdPlant);
					if (shop != null && shop.IdFloristNavigation != null && shop.IdFloristNavigation.IdShopNavigation != null)
					{
						cmbShop.Text = shop.IdFloristNavigation.IdShopNavigation.NameOfShop;
						cmbFlorist.Text = shop.IdFloristNavigation.FName;
					}
					DataContext = selectedPlant;
				}
			}
			else
			{
				if (selectedPlant != null)
				{
					txtName.Text = selectedPlant.Name;
					txtColor.Text = selectedPlant.Color;
					txtSoil.Text = selectedPlant.RequieredSoil;
					txtFlowering.Text = selectedPlant.FloweringPeriod;
					txtShape.Text = selectedPlant.FlowerShape;
					txtSize.Text = selectedPlant.FlowerSize;
					txtLife.Text = selectedPlant.ShelfLife;
					txtPrice.Text = selectedPlant.Price.ToString();
					DataContext = selectedPlant;

					cmbShop.Visibility = Visibility.Collapsed;
					cmbFlorist.Visibility = Visibility.Collapsed;
					textFlorist.Visibility = Visibility.Collapsed;
					textShop.Visibility = Visibility.Collapsed;
					this.Height = 455;
					Grid.SetRow(btSave, 8);
					Grid.SetRow(btClose, 8);
				}
			}
		}

		private void Save_Click(object sender, RoutedEventArgs e)
		{
			if (App.currentUser.IdRole == 1)
			{
				if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtColor.Text) ||
				string.IsNullOrWhiteSpace(txtSoil.Text) || string.IsNullOrWhiteSpace(txtFlowering.Text) ||
				string.IsNullOrWhiteSpace(txtShape.Text) || string.IsNullOrWhiteSpace(txtSize.Text) ||
				string.IsNullOrWhiteSpace(txtLife.Text) || string.IsNullOrWhiteSpace(txtPrice.Text) ||
				cmbFlorist.SelectedItem == null || cmbShop.SelectedItem == null)
				{
					MessageBox.Show("Заполните все поля!");

					if (string.IsNullOrWhiteSpace(txtName.Text))
						txtName.BorderBrush = Brushes.Red;
					else
						txtName.BorderBrush = Brushes.Black;

					if (string.IsNullOrWhiteSpace(txtColor.Text))
						txtColor.BorderBrush = Brushes.Red;
					else
						txtColor.BorderBrush = Brushes.Black;

					if (string.IsNullOrWhiteSpace(txtSoil.Text))
						txtSoil.BorderBrush = Brushes.Red;
					else
						txtSoil.BorderBrush = Brushes.Black;

					if (string.IsNullOrWhiteSpace(txtFlowering.Text))
						txtFlowering.BorderBrush = Brushes.Red;
					else
						txtFlowering.BorderBrush = Brushes.Black;

					if (string.IsNullOrWhiteSpace(txtShape.Text))
						txtShape.BorderBrush = Brushes.Red;
					else
						txtShape.BorderBrush = Brushes.Black;

					if (string.IsNullOrWhiteSpace(txtSize.Text))
						txtSize.BorderBrush = Brushes.Red;
					else
						txtSize.BorderBrush = Brushes.Black;

					if (string.IsNullOrWhiteSpace(txtLife.Text))
						txtLife.BorderBrush = Brushes.Red;
					else
						txtLife.BorderBrush = Brushes.Black;

					if (string.IsNullOrWhiteSpace(txtPrice.Text))
						txtPrice.BorderBrush = Brushes.Red;
					else
						txtPrice.BorderBrush = Brushes.Black;

					return;
				}
			}
			else
			{
				if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtColor.Text) ||
				string.IsNullOrWhiteSpace(txtSoil.Text) || string.IsNullOrWhiteSpace(txtFlowering.Text) ||
				string.IsNullOrWhiteSpace(txtShape.Text) || string.IsNullOrWhiteSpace(txtSize.Text) ||
				string.IsNullOrWhiteSpace(txtLife.Text) || string.IsNullOrWhiteSpace(txtPrice.Text))
				{
					MessageBox.Show("Заполните все поля!");

					if (string.IsNullOrWhiteSpace(txtName.Text))
						txtName.BorderBrush = Brushes.Red;
					else
						txtName.BorderBrush = Brushes.Black;

					if (string.IsNullOrWhiteSpace(txtColor.Text))
						txtColor.BorderBrush = Brushes.Red;
					else
						txtColor.BorderBrush = Brushes.Black;

					if (string.IsNullOrWhiteSpace(txtSoil.Text))
						txtSoil.BorderBrush = Brushes.Red;
					else
						txtSoil.BorderBrush = Brushes.Black;

					if (string.IsNullOrWhiteSpace(txtFlowering.Text))
						txtFlowering.BorderBrush = Brushes.Red;
					else
						txtFlowering.BorderBrush = Brushes.Black;

					if (string.IsNullOrWhiteSpace(txtShape.Text))
						txtShape.BorderBrush = Brushes.Red;
					else
						txtShape.BorderBrush = Brushes.Black;

					if (string.IsNullOrWhiteSpace(txtSize.Text))
						txtSize.BorderBrush = Brushes.Red;
					else
						txtSize.BorderBrush = Brushes.Black;

					if (string.IsNullOrWhiteSpace(txtLife.Text))
						txtLife.BorderBrush = Brushes.Red;
					else
						txtLife.BorderBrush = Brushes.Black;

					if (string.IsNullOrWhiteSpace(txtPrice.Text))
						txtPrice.BorderBrush = Brushes.Red;
					else
						txtPrice.BorderBrush = Brushes.Black;

					return;
				}
			}

			if (SelectedPlant != null)
			{
				SelectedPlant.Name = txtName.Text;
				SelectedPlant.Color = txtColor.Text;
				SelectedPlant.RequieredSoil = txtSoil.Text;
				SelectedPlant.FloweringPeriod = txtFlowering.Text;
				SelectedPlant.FlowerShape = txtShape.Text;
				SelectedPlant.FlowerSize = txtSize.Text;
				SelectedPlant.ShelfLife = txtLife.Text;

				decimal price;
				if (decimal.TryParse(txtPrice.Text, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out price))
				{
					SelectedPlant.Price = price;

					_db.Plants.Update(SelectedPlant);
					_db.SaveChanges();

					if (App.currentUser.IdRole == 1)
					{
						if (cmbFlorist.SelectedItem != null)
						{
							Florist selectedFlorist = (Florist)cmbFlorist.SelectedItem;

							PlantsAssortmentFlorist plantsAssortmentFlorist = SelectedPlant.PlantsAssortmentFlorists.FirstOrDefault();
							if (plantsAssortmentFlorist != null)
							{
								plantsAssortmentFlorist.IdFlorist = selectedFlorist.IdFlorist;
								_db.PlantsAssortmentFlorists.Update(plantsAssortmentFlorist);
							}

							_db.SaveChanges();
							MessageBox.Show("Комнатное растение успешно изменено!");
							Close();
						}
					}
					else
					{
						MessageBox.Show("Цветок успешно изменен!");
						Close();
					}
				}
				else
				{
					MessageBox.Show("Пожалуйста, введите корректную цену.");
				}
			}
		}

		private void CmbShop_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ShopsForSale selectedShop = (ShopsForSale)cmbShop.SelectedItem;

			if (selectedShop != null)
			{
				ListFlorist = new ObservableCollection<Florist>(_db.Florists.Where(f => f.IdShop == selectedShop.IdShop).ToList());
			}
			else
			{
				ListFlorist = new ObservableCollection<Florist>();
			}

			cmbFlorist.ItemsSource = ListFlorist;
			cmbFlorist.DisplayMemberPath = "FName";
		}

		private Florist selectedFlorist;

		private void CmbFlorist_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			selectedFlorist = (Florist)cmbFlorist.SelectedItem;
		}

		private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			TextBox? textBox = sender as TextBox;
			if (e.Text == "0" && textBox.Text.Length == 0)
			{
				MessageBox.Show("Введите число не начиная с нуля.");
				e.Handled = true; 
			}
			else if (!char.IsDigit(e.Text, 0) && e.Text != ".")
			{
				MessageBox.Show("Вводите только числа через точку.");
				e.Handled = true;
			}
			else
			{
				string text = textBox.Text.Insert(textBox.CaretIndex, e.Text);
				string[] parts = text.Split('.');

				if (parts.Length > 2 || (parts.Length == 2 && parts[1].Length > 2) || (parts.Length == 1 && parts[0].Length > 5))
				{
					MessageBox.Show("Вводите не более пять цифр до точки, и не более двух после.");
					e.Handled = true;
				}
			}
		}
	}
}
