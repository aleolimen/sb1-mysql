namespace CrmMaui;

public partial class CustomersPage : ContentPage
{
    private readonly DatabaseService _databaseService;

    public CustomersPage(DatabaseService databaseService)
    {
        InitializeComponent();
        _databaseService = databaseService;
        LoadCustomers();
    }

    private async void LoadCustomers()
    {
        var customers = await _databaseService.GetCustomersAsync();
        CustomersCollection.ItemsSource = customers.DefaultView;
    }
}