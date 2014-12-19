using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetReceivables
{
    class NetReceivable
    {
        #region Constructors

        public NetReceivable()
        {
            Invoices = new List<Invoice>();
            Bills = new List<Bill>();
        }

        public NetReceivable(Invoice invoice)
            : this(invoice, new Bill())
        { }

        public NetReceivable(Bill bill)
            : this(new Invoice(), bill)
        { }

        public NetReceivable(Invoice invoice, Bill bill)
        {
            Invoices = invoice == null ? new List<Invoice>() : new List<Invoice> { invoice };
            Bills = bill == null ? new List<Bill>() : new List<Bill> { bill };
        }

        #endregion

        #region Public Properties

        public List<Invoice> Invoices { get; set; }

        public List<Bill> Bills { get; set; } 

        #endregion
    }
}
