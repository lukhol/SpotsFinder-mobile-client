using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotFinder.ViewModels.Xaml
{
    public class CriteriaViewModel : BaseViewModel
    {
        public CriteriaViewModel()
        {
            typesMap = CreateTypesMap();
        }

        private Dictionary<string, bool> typesMap;
        public Dictionary<string, bool> TypesMap
        {
            get => typesMap;
        }

        private Dictionary<string, bool> CreateTypesMap()
        {
            return new Dictionary<string, bool>
            {
                { "Skatepark", false },
                { "Skatespot", false },
                { "DIY", false }
            };
        }
    }
}
