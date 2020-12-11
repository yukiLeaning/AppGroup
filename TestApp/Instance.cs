using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp
{
    class Instance
    {
        private static Instance instance = new Instance();
        private Instance() { }
        public static Instance Get()
        {
            return instance;
        }
        public void Mes()
        {
            System.Windows.Forms.MessageBox.Show("OK");
        }
    }
}
