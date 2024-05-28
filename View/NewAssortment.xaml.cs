using Practice.Models;
using Practice.ViewModels;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Practice.View
{
	public partial class NewAssortment : Window
	{
		private PracticeContext db = new PracticeContext();
		public ObservableCollection<ShopsForSale> ListShop { get; set; }
		public ObservableCollection<Florist> ListFlorist { get; set; }
		public NewAssortment()
		{
			InitializeComponent();
			if (App.currentUser.IdRole == 1)
			{
				ListShop = new ObservableCollection<ShopsForSale>(db.ShopsForSales.ToList());
				cmbShop.ItemsSource = ListShop;
				cmbShop.DisplayMemberPath = "NameOfShop";
				cmbShop.SelectionChanged += CmbShop_SelectionChanged;
			}
			else
			{
				cmbShop.Visibility = Visibility.Collapsed;
				cmbFlorist.Visibility = Visibility.Collapsed;
				textFlorist.Visibility = Visibility.Collapsed;
				textShop.Visibility = Visibility.Collapsed;
				this.Height = 300;
				Grid.SetRow(btSave, 4);
				Grid.SetRow(btClose, 4);
			}
		}

		private void Save_Click(object sender, RoutedEventArgs e)
		{
			if (App.currentUser.IdRole == 1)
			{
				if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtCategory.Text) ||
			string.IsNullOrWhiteSpace(txtDescription.Text) || string.IsNullOrWhiteSpace(txtPrice.Text) ||
			cmbFlorist.SelectedItem == null || cmbShop.SelectedItem == null)
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
			decimal price;
			if (decimal.TryParse(txtPrice.Text, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out price))
			{
				Assortment assortment = new Assortment
				{
					Name = txtName.Text,
					Category = txtCategory.Text,
					Description = txtDescription.Text,
					Price = price,
				};

				db.Assortments.Add(assortment);
				db.SaveChanges();
				if (App.currentUser.IdRole == 1)
				{
					if (cmbFlorist.SelectedItem != null)
					{
						Florist selectedFlorist = (Florist)cmbFlorist.SelectedItem;
						var plantsAssortmentFlorist = new PlantsAssortmentFlorist
						{
							IdAssortment = assortment.IdAssortment,
							IdFlorist = selectedFlorist.IdFlorist
						};
						db.PlantsAssortmentFlorists.Add(plantsAssortmentFlorist);
						db.SaveChanges();
					}
				}

				else
				{
					Florist florist = db.Florists.FirstOrDefault(f => f.IdUser == App.currentUser.IdUser);
					int floristId = florist.IdFlorist;
					var plantsAssortmentFlorist = new PlantsAssortmentFlorist
					{
						IdAssortment = assortment.IdAssortment,
						IdFlorist = floristId,
					};
					db.PlantsAssortmentFlorists.Add(plantsAssortmentFlorist);
					db.SaveChanges();
				}

				MessageBox.Show("Новый ассортимент успешно добавлен!");
				Close();
			}
		}



		private void CmbShop_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ShopsForSale selectedShop = (ShopsForSale)cmbShop.SelectedItem;

			if (selectedShop != null)
			{
				ListFlorist = new ObservableCollection<Florist>(db.Florists.Where(f => f.IdShop == selectedShop.IdShop).ToList());
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
