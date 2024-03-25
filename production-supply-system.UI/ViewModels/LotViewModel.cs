using CommunityToolkit.Mvvm.ComponentModel;

using production_supply_system.EntityFramework.DAL.LotContext.Models;

namespace UI_Interface.ViewModels
{
    public partial class LotViewModel(Lot lot) : ObservableObject
    {
        [ObservableProperty]
        private Lot _lot = lot;

        [ObservableProperty]
        private int _quantityContainers = lot.ContainersInLots.Count;
    }
}
