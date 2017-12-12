using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KioskCheckoutSystemTest
{
    public interface IBuilder<out T> where T : class
    {
        T Build();
    }
}
