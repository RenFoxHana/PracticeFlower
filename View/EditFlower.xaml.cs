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
	public partial class EditFlower : Window
	{
		private PracticeContext _db;
		public Flower SelectedFlower { get; set; }
		public ObservableCollection<ShopsForSale> ListShop { get; set; }
		public ObservableCollection<Florist> ListFlorist { get; set; }
		public EditFlower(Flower selectedFlower)
		{
			SelectedFlower = selectedFlower;
			InitializeComponent();
			_db = new PracticeContext();
			if (App.currentUser.IdRole == 1)
			{
				if (selectedFlower != null)
				{
					txtName.Text = selectedFlower.Name;
					txtColor.Text = selectedFlower.Color;
					txtSize.Text = selectedFlower.FlowerSize;
					txtLife.Text = selectedFlower.ShelfLife;
					txtSizeB.Text = selectedFlower.BouquetDesign.ToString();
					txtDesign.Text = selectedFlower.BouquetDesign;
					txtPackaging.Text = selectedFlower.Packaging;
					txtPrice.Text = selectedFlower.Price.ToString();

					ListShop = new ObservableCollection<ShopsForSale>(_db.ShopsForSales.ToList());
					cmbShop.ItemsSource = ListShop;
					cmbShop.DisplayMemberPath = "NameOfShop";
					cmbShop.SelectionChanged += CmbShop_SelectionChanged;

					FlowerFlorist shop = _db.FlowerFlorists.Include(paf => paf.IdFloristNavigation.IdShopNavigation).FirstOrDefault(paf => paf.IdFlower == selectedFlower.IdFlower);
					if (shop != null && shop.IdFloristNavigation != null && shop.IdFloristNavigation.IdShopNavigation != null)
					{
						cmbShop.Text = shop.IdFloristNavigation.IdShopNavigation.NameOfShop;
						cmbFlorist.Text = shop.IdFloristNavigation.FName;
					}
					DataContext = selectedFlower;
				}
			}

			else
			{
				if (selectedFlower != null)
				{
					txtName.Text = selectedFlower.Name;
					txtColor.Text = selectedFlower.Color;
					txtSize.Text = selectedFlower.FlowerSize;
					txtLife.Text = selectedFlower.ShelfLife;
					txtSizeB.Text = selectedFlower.BouquetDesign.ToString();
					txtDesign.Text = selectedFlower.BouquetDesign;
					txtPackaging.Text = selectedFlower.Packaging;
					txtPrice.Text = selectedFlower.Price.ToString();
					DataContext = selectedFlower;

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

			if (SelectedFlower != null)
			{
				SelectedFlower.Name = txtName.Text;
				SelectedFlower.Color = txtColor.Text;
				SelectedFlower.FlowerSize = txtSize.Text;
				SelectedFlower.ShelfLife = txtLife.Text;
				SelectedFlower.BouquetDesign = txtDesign.Text;
				SelectedFlower.Packaging = txtPackaging.Text;

				int sizeB;
				decimal price;
				if (decimal.TryParse(txtPrice.Text, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out price) && int.TryParse(txtSizeB.Text, out sizeB))
				{
					SelectedFlower.Price = price;
					SelectedFlower.BouquetSize = sizeB;

					_db.Flowers.Update(SelectedFlower);

					if (App.currentUser.IdRole == 1)
					{
						if (cmbFlorist.SelectedItem != null)
						{
							Florist selectedFlorist = (Florist)cmbFlorist.SelectedItem;

							FlowerFlorist flowerFlorist = SelectedFlower.FlowerFlorists.FirstOrDefault();
							if (flowerFlorist != null)
							{
								flowerFlorist.IdFlorist = selectedFlorist.IdFlorist;
								_db.FlowerFlorists.Update(flowerFlorist);
							}
						}
					}
					_db.SaveChanges();
					MessageBox.Show("Цветок успешно изменен!");
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
