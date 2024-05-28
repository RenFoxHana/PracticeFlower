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
	public partial class EditAssortment : Window
	{
		private PracticeContext _db;
		public Assortment SelectedAssortment { get; set; }
		public ObservableCollection<ShopsForSale> ListShop { get; set; }
		public ObservableCollection<Florist> ListFlorist { get; set; }
		public EditAssortment(Assortment selectedAssortment)
		{
			SelectedAssortment = selectedAssortment;
			InitializeComponent();
			_db = new PracticeContext();
			if (App.currentUser.IdRole == 1)
			{
				if (selectedAssortment != null)
				{
					txtName.Text = selectedAssortment.Name;
					txtCategory.Text = selectedAssortment.Category;
					txtDescription.Text = selectedAssortment.Description;
					txtPrice.Text = selectedAssortment.Price.ToString();

					ListShop = new ObservableCollection<ShopsForSale>(_db.ShopsForSales.ToList());
					cmbShop.ItemsSource = ListShop;
					cmbShop.DisplayMemberPath = "NameOfShop";
					cmbShop.SelectionChanged += CmbShop_SelectionChanged;

					PlantsAssortmentFlorist shop = _db.PlantsAssortmentFlorists.Include(paf => paf.IdFloristNavigation.IdShopNavigation).FirstOrDefault(paf => paf.IdAssortment == selectedAssortment.IdAssortment);
					if (shop != null && shop.IdFloristNavigation != null && shop.IdFloristNavigation.IdShopNavigation != null)
					{
						cmbShop.Text = shop.IdFloristNavigation.IdShopNavigation.NameOfShop;
						cmbFlorist.Text = shop.IdFloristNavigation.FName;
					}
					DataContext = selectedAssortment;
				}
			}
			else
			{
				if (selectedAssortment != null)
				{
					txtName.Text = selectedAssortment.Name;
					txtCategory.Text = selectedAssortment.Category;
					txtDescription.Text = selectedAssortment.Description;
					txtPrice.Text = selectedAssortment.Price.ToString();
					DataContext = selectedAssortment;

					cmbShop.Visibility = Visibility.Collapsed;
				cmbFlorist.Visibility = Visibility.Collapsed;
				textFlorist.Visibility = Visibility.Collapsed;
				textShop.Visibility = Visibility.Collapsed;
				this.Height = 300;
				Grid.SetRow(btSave, 4);
				Grid.SetRow(btClose, 4);
				}
			}
		}

		private void Save_Click(object sender, RoutedEventArgs e)
		{
			if (App.currentUser.IdRole == 1)
			{
				if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtCategory.Text) ||
					string.IsNullOrWhiteSpace(txtDescription.Text) || string.IsNullOrWhiteSpace(txtPrice.Text) ||
					(cmbFlorist.SelectedItem as ComboBoxItem) == null || (cmbShop.SelectedItem as ComboBoxItem) == null)
				{
					MessageBox.Show("Заполните все поля!");

					if (string.IsNullOrWhiteSpace(txtName.Text))
						txtName.BorderBrush = Brushes.Red;
					else txtName.BorderBrush = Brushes.Black;

					if (string.IsNullOrWhiteSpace(txtCategory.Text))
						txtCategory.BorderBrush = Brushes.Red;
					else txtCategory.BorderBrush = Brushes.Black;

					if (string.IsNullOrWhiteSpace(txtDescription.Text))
						txtDescription.BorderBrush = Brushes.Red;
					else txtDescription.BorderBrush = Brushes.Black;

					if (string.IsNullOrWhiteSpace(txtPrice.Text))
						txtPrice.BorderBrush = Brushes.Red;
					else txtPrice.BorderBrush = Brushes.Black; 

					return;
				}
			}
			else
			{
				if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtCategory.Text) ||
					string.IsNullOrWhiteSpace(txtDescription.Text) || string.IsNullOrWhiteSpace(txtPrice.Text))
				{
					MessageBox.Show("Заполните все поля!");

					if (string.IsNullOrWhiteSpace(txtName.Text))
						txtName.BorderBrush = Brushes.Red;
					else txtName.BorderBrush = Brushes.Black;

					if (string.IsNullOrWhiteSpace(txtCategory.Text))
						txtCategory.BorderBrush = Brushes.Red;
					else txtCategory.BorderBrush = Brushes.Black;

					if (string.IsNullOrWhiteSpace(txtDescription.Text))
						txtDescription.BorderBrush = Brushes.Red;
					else txtDescription.BorderBrush = Brushes.Black;

					if (string.IsNullOrWhiteSpace(txtPrice.Text))
						txtPrice.BorderBrush = Brushes.Red;
					else txtPrice.BorderBrush = Brushes.Black;

					return;
				}
			}

			if (SelectedAssortment != null)
			{
				SelectedAssortment.Name = txtName.Text;
				SelectedAssortment.Category = txtCategory.Text;
				SelectedAssortment.Description = txtDescription.Text;

				decimal price;
				if (decimal.TryParse(txtPrice.Text, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out price))
				{
					SelectedAssortment.Price = price;

					_db.Assortments.Update(SelectedAssortment);

					if (App.currentUser.IdRole == 1)
					{
						if (cmbFlorist.SelectedItem != null)
						{
							Florist selectedFlorist = (Florist)cmbFlorist.SelectedItem;
							PlantsAssortmentFlorist plantsAssortmentFlorist = SelectedAssortment.PlantsAssortmentFlorists.FirstOrDefault();

							if (plantsAssortmentFlorist != null)
							{
								plantsAssortmentFlorist.IdFlorist = selectedFlorist.IdFlorist;
								_db.PlantsAssortmentFlorists.Update(plantsAssortmentFlorist);
							}
						}
					}

					_db.SaveChanges();
					MessageBox.Show("Ассортимент успешно изменен!");
					Close();
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
				MessageBox.Show("Вводите только числа с разделителем точкой.");
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
