using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotFinder.ViewModels
{
    public class TestClass
    {
        public TestClass(string str)
        {
            message = str;
        }

        private string message;

        public string Message
        {
            get => message;
            set
            {
                message = value;
            }
        }

        public TestClass()
        {

        }

    }
}
