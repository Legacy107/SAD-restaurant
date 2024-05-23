using Database.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoalaPayment.Models
{
    internal interface IPrintableItem
    {
        string GetPrintContent(DataContext context);
    }
}
