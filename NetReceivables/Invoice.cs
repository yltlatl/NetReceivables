using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetReceivables
{
    class Invoice : AccountingItem
    {
        #region Properties

        public string Customer { get; set; }

        #endregion
    }
}
