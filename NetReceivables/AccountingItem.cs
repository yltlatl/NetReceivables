using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetReceivables
{
    abstract class AccountingItem
    {
        #region Constructors

        public AccountingItem()
        { }

        public AccountingItem(string id)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("id is null or empty.");
            Id = id;
        }

        #endregion


        #region Fields

        private string _id;

        private double _amount;

        private DateTime _date;

        private int _PastDue;

        #endregion

        #region Properties

        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public double Amount
        {
            get { return _amount; }
            set { _amount = value; }
        }

        public DateTime Date
        {
            get { return _date; }
            set { _date = value; }
        }

        public DateTime DueDate { get; set; }

        public int PastDue { get; private set; }

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
