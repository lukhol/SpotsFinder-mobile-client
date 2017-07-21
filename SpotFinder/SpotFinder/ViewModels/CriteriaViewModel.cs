using SpotFinder.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SpotFinder.ViewModels
{
    public class CriteriaViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private INavigation Navigation { get; }

        private bool skatepark;
        private bool skatespot;

        private bool ledge;
        private bool rail;
        private bool gap;
        private bool stairs;
        private bool handrail;
        private bool corners;
        private bool manualpad;

        public CriteriaViewModel(INavigation navigation)
        { 
            Navigation = navigation;
        }

        public bool Skatepark
        {
            get => skatepark;
            set
            {
                skatepark = value;
                OnPropertyChanged();
            }
        }

        public bool Skatespot
        {
            get => skatespot;
            set
            {
                skatespot = value;
                OnPropertyChanged();
            }
        }

        public bool Ledge
        {
            get => ledge;
            set
            {
                ledge = value;
                OnPropertyChanged();
            }
        }

        public bool Rail
        {
            get => rail;
            set
            {
                rail = value;
                OnPropertyChanged();
            }
        }

        public bool Gap
        {
            get => gap;
            set
            {
                gap = value;
                OnPropertyChanged();
            }
        }

        public bool Stairs
        {
            get => stairs;
            set
            {
                stairs = value;
                OnPropertyChanged();
            }
        }

        public bool Handrail
        {
            get => handrail;
            set
            {
                handrail = value;
                OnPropertyChanged();
            }
        }

        public bool Corners
        {
            get => corners;
            set
            {
                corners = value;
                OnPropertyChanged();
            }
        }

        public bool Manualpad
        {
            get => manualpad;
            set
            {
                manualpad = value;
                OnPropertyChanged();
            }
        }

        public Command SelectAllCommand => new Command(() => 
        {
            Ledge = true;
            Rail = true;
            Gap = true;
            Stairs = true;
            Handrail = true;
            Corners = true;
            Manualpad = true;
        });

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
