using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetReceivables
{
    class Bill : AccountingItem
    {
        #region Public Properties

        public string Vendor { get; set; }

        #endregion
    }
}
