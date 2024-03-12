using System;
using System.Threading.Tasks;

using DAL.Models;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace UI_Interface.ViewModels
{
    public class LotViewModel: ObservableObject
    {
        private Lot _lot;

        private int _quantityContainers;

        public LotViewModel(Lot lot, int quantityContainers)
        {
            _lot = lot;

            _quantityContainers = quantityContainers;
        }

        public Lot Lot
        {
            get => _lot;
            set => _ = SetProperty(ref _lot, value);
        }

        public int QuantityContainers
        {
            get => _quantityContainers;
            set => _ = SetProperty(ref _quantityContainers, value);
        }
    }
}
