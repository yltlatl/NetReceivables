using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetReceivables
{
    abstract class AccountingItem
    {
        #region Fields

        private string _id;

        private double _amount;

        private DateTime _date;

        private int _PastDue;

        #endregion

        #region Properties

        protected string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        protected double Amount
        {
            get { return _amount; }
            set { _amount = value; }
        }

        protected DateTime Date
        {
            get { return _date; }
            set { _date = value; }
        }

        protected DateTime DueDate { get; set; }

        protected int PastDue { get; private set; }

        public void SetPastDue()
        {
            PastDue = DateTime.Today.Subtract(DueDate).Days;
        }

        public void SetPastDue(int days)
        {
            PastDue = days;
        }



        #endregion

    }
}
