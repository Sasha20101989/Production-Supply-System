
using CommunityToolkit.Mvvm.ComponentModel;

using DAL.Models;

namespace UI_Interface.ViewModels
{
    public partial class LotViewModel(Lot lot, int quantityContainers) : ObservableObject
    {
        [ObservableProperty]
        private Lot _lot = lot;

        [ObservableProperty]
        private int _quantityContainers = quantityContainers;
    }
}
