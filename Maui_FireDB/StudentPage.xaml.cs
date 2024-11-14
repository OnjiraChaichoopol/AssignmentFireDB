using Maui_FireDB.Services;
using Maui_FireDB.ViewModels;

namespace Maui_FireDB;

public partial class StudentPage : ContentPage
{
	public StudentPage()
	{
		InitializeComponent();
		var firestoreService = new FireDBService();
		BindingContext = new StudentViewModel(firestoreService);
	}
}