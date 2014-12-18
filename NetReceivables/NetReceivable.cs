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
            if (invoice == null) throw new ArgumentNullException("invoice", "Null invoice");
            Invoices = new List<Invoice> { invoice };
            if (bill == null) throw new ArgumentNullException("bill", "Null bill");
            Bills = new List<Bill> { bill };
        }

        #endregion

        #region Public Properties

        public string CompositeId { get; private set; }

        public List<Invoice> Invoices { get; set; }

        public List<Bill> Bills { get; set; } 

        #endregion
    }
}
