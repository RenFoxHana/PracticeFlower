using Practice.Models;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Practice.View
{

	public partial class NewFlower : Window
	{
		private PracticeContext db = new PracticeContext();
		public ObservableCollection<ShopsForSale> ListShop { get; set; }
		public ObservableCollection<Florist> ListFlorist { get; set; }
		public NewFlower()
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
				this.Height = 455;
				Grid.SetRow(btSave, 8);
				Grid.SetRow(btClose, 8);
			}
		}

		private void Save_Click(object sender, RoutedEventArgs e)
		{
			if (App.currentUser.IdRole == 1)
			{
				if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtColor.Text) ||
				string.IsNullOrWhiteSpace(txtSize.Text) || string.IsNullOrWhiteSpace(txtLife.Text) ||
				string.IsNullOrWhiteSpace(txtSizeB.Text) || string.IsNullOrWhiteSpace(txtDesign.Text) ||
				string.IsNullOrWhiteSpace(txtPackaging.Text) || string.IsNullOrWhiteSpace(txtPrice.Text) ||
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

					if (string.IsNullOrWhiteSpace(txtSize.Text))
						txtSize.BorderBrush = Brushes.Red;
					else
						txtSize.BorderBrush = Brushes.Black;

					if (string.IsNullOrWhiteSpace(txtLife.Text))
						txtLife.BorderBrush = Brushes.Red;
					else
						txtLife.BorderBrush = Brushes.Black;

					if (string.IsNullOrWhiteSpace(txtSizeB.Text))
						txtSizeB.BorderBrush = Brushes.Red;
					else
						txtSizeB.BorderBrush = Brushes.Black;

					if (string.IsNullOrWhiteSpace(txtDesign.Text))
						txtDesign.BorderBrush = Brushes.Red;
					else
						txtDesign.BorderBrush = Brushes.Black;

					if (string.IsNullOrWhiteSpace(txtPackaging.Text))
						txtPackaging.BorderBrush = Brushes.Red;
					else
						txtPackaging.BorderBrush = Brushes.Black;

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
				string.IsNullOrWhiteSpace(txtSize.Text) || string.IsNullOrWhiteSpace(txtLife.Text) ||
				string.IsNullOrWhiteSpace(txtSizeB.Text) || string.IsNullOrWhiteSpace(txtDesign.Text) ||
				string.IsNullOrWhiteSpace(txtPackaging.Text) || string.IsNullOrWhiteSpace(txtPrice.Text))
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

					if (string.IsNullOrWhiteSpace(txtSize.Text))
						txtSize.BorderBrush = Brushes.Red;
					else
						txtSize.BorderBrush = Brushes.Black;

					if (string.IsNullOrWhiteSpace(txtLife.Text))
						txtLife.BorderBrush = Brushes.Red;
					else
						txtLife.BorderBrush = Brushes.Black;

					if (string.IsNullOrWhiteSpace(txtSizeB.Text))
						txtSizeB.BorderBrush = Brushes.Red;
					else
						txtSizeB.BorderBrush = Brushes.Black;

					if (string.IsNullOrWhiteSpace(txtDesign.Text))
						txtDesign.BorderBrush = Brushes.Red;
					else
						txtDesign.BorderBrush = Brushes.Black;

					if (string.IsNullOrWhiteSpace(txtPackaging.Text))
						txtPackaging.BorderBrush = Brushes.Red;
					else
						txtPackaging.BorderBrush = Brushes.Black;

					if (string.IsNullOrWhiteSpace(txtPrice.Text))
						txtPrice.BorderBrush = Brushes.Red;
					else
						txtPrice.BorderBrush = Brushes.Black;

					return;
				}
			}

			decimal price;
			if (decimal.TryParse(txtPrice.Text, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out price))
			{
				Flower flower = new Flower
				{
					Name = txtName.Text,
					Color = txtColor.Text,
					FlowerSize = txtSize.Text,
					ShelfLife = txtLife.Text,
					BouquetSize = Convert.ToInt32(txtSizeB.Text),
					BouquetDesign = txtDesign.Text,
					Packaging = txtPackaging.Text,
					Price = price,
				};
				db.Flowers.Add(flower);
				db.SaveChanges();

				if (App.currentUser.IdRole == 1)
				{
					if (cmbFlorist.SelectedItem != null)
					{
						Florist selectedFlorist = (Florist)cmbFlorist.SelectedItem;
						var flowerFlorist = new FlowerFlorist
						{
							IdFlower = flower.IdFlower,
							IdFlorist = selectedFlorist.IdFlorist
						};

						db.FlowerFlorists.Add(flowerFlorist);
						db.SaveChanges();
					}
				}
				else
				{
					Florist florist = db.Florists.FirstOrDefault(f => f.IdUser == App.currentUser.IdUser);
					int floristId = florist.IdFlorist;
					var flowerFlorist = new FlowerFlorist
					{
						IdFlower = flower.IdFlower,
						IdFlorist = floristId,
					};
					db.FlowerFlorists.Add(flowerFlorist);
					db.SaveChanges();
				}
					MessageBox.Show("Новый цветок успешно добавлен!");
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

			if (textBox.Name == "txtSizeB")
			{
				if (e.Text == "0" && textBox.Text.Length == 0)
				{
					MessageBox.Show("Введите число не начиная с нуля.");
					e.Handled = true;
				}
				else if (!char.IsDigit(e.Text, 0) || textBox.Text.Length >= 4)
				{
					MessageBox.Show("Вводите только числа и не более четырех цифр.");
					e.Handled = true;
				}
			}

			if (textBox.Name == "txtPrice")
			{
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
}
