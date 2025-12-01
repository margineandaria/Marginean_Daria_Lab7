using System;
using Marginean_Daria_Lab7.Models;
using Microsoft.Maui.Controls;

namespace Marginean_Daria_Lab7;

public partial class ListPage : ContentPage
{
    public ListPage()
    {
        InitializeComponent();
    }

    async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        var slist = (ShopList)BindingContext;
        slist.Date = DateTime.UtcNow;
        await App.Database.SaveShopListAsync(slist);
        await Navigation.PopAsync();
    }

    async void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        var slist = (ShopList)BindingContext;
        await App.Database.DeleteShopListAsync(slist);
        await Navigation.PopAsync();
    }
    async void OnChooseButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ProductPage((ShopList)
            this.BindingContext)
        {
            BindingContext = new Product()
        });
    }
    async void OnDeleteItemButtonClicked(object sender, EventArgs e)
    {
        if (listView.SelectedItem == null)
        {
            await DisplayAlert("Error", "Please select an item to delete", "OK");
            return;
        }

        var product = listView.SelectedItem as Product;
        var shopList = BindingContext as ShopList;

        if (shopList != null && product != null)
        {
            await App.Database.DeleteListProductAsync(shopList.ID, product.ID);
            listView.ItemsSource = await App.Database.GetListProductsAsync(shopList.ID);
        }
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var shopl = (ShopList)BindingContext;
        listView.ItemsSource = await App.Database.GetListProductsAsync(shopl.ID);
    }
    
}