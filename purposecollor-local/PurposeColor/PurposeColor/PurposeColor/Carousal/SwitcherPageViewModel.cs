using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace CustomLayouts.ViewModels
{
	public class SwitcherPageViewModel : BaseViewModel
	{
		public SwitcherPageViewModel()
		{
			Pages = new List<CarousalViewModel>() {
				new CarousalViewModel { Title = "1", Background = Color.White, ImageSource = "icon.png" },
				new CarousalViewModel { Title = "2", Background = Color.Red, ImageSource = "icon.png" },
				new CarousalViewModel { Title = "3", Background = Color.Blue, ImageSource = "one.jpeg" },
				new CarousalViewModel { Title = "4", Background = Color.Yellow, ImageSource = "icon.png" },
			};

			CurrentPage = Pages.First();
		}

		IEnumerable<CarousalViewModel> _pages;
		public IEnumerable<CarousalViewModel> Pages {
			get {
				return _pages;
			}
			set {
				SetObservableProperty (ref _pages, value);
				CurrentPage = Pages.FirstOrDefault ();
			}
		}

		CarousalViewModel _currentPage;
		public CarousalViewModel CurrentPage {
			get {
				return _currentPage;
			}
			set {
				SetObservableProperty (ref _currentPage, value);
			}
		}
	}

	public class CarousalViewModel : BaseViewModel
	{
		public CarousalViewModel() {}

		public string Title { get; set; }
		public Color Background { get; set; }
		public string ImageSource { get; set; }
	}
}

