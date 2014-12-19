using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace NetReceivables
{
    public struct XRefFields
    {
        public const string InvoiceIDField = "TurnstoneInvoiceID";
        public const string BillIDField = "SubInvoiceID";
    }

    public struct InvoiceFields
    {
        public const string IdField = "Num";
        public const string Date = "Date";
        public const string DueDate = "Due Date";
        public const string PastDue = "Past Due";
        public const string Customer = "Client";
        public const string Amount = "Amount";
    }

    public struct BillFields
    {
        public const string IdField = "BillID";
        public const string Date = "Date";
        public const string DueDate = "Due Date";
        public const string PastDue = "Past Due";
        public const string Vendor = "Vendor";
        public const string Amount = "Amount";
    }

    class Program
    {
        
        static void Main(string[] args)
        {
            var argsValidator = new ArgsValidator(args);
            var netReceivables = new List<NetReceivable>();

            var xRefDF = new DelimitedFile(argsValidator.XRefPath, "ASCII", '\n', (char)44, (char)59, (char)34);
            xRefDF.GetNextRecord();
            while (!xRefDF.EndOfFile)
            {
                var invoiceID = xRefDF.GetFieldByName(XRefFields.InvoiceIDField);
                var billID = xRefDF.GetFieldByName(XRefFields.BillIDField);
                var blankInvoice = string.IsNullOrEmpty(invoiceID);
                var blankBill = string.IsNullOrEmpty(billID);
                if (blankInvoice && blankBill)
                {
                    Console.WriteLine("Blank line enountered.");
                }
                else if (blankInvoice)
                {
                    netReceivables.Add(new NetReceivable(null, new Bill(billID)));
                    break;
                }
                else if (blankBill)
                {
                    netReceivables.Add(new NetReceivable(new Invoice(invoiceID)));
                    break;
                }

                var found = false;
                foreach (var nr in netReceivables)
                {
                    var invoiceMatch = nr.Invoices.Any(i => i.Id.Equals(invoiceID));
                    var billMatch = nr.Bills.Any(b => b.Id.Equals(billID));
                    if (invoiceMatch && billMatch)
                    {
                        throw new InvalidDataException("Many-to-many relationship discovered.");
                    }
                    else if (invoiceMatch)
                    {
                        nr.Bills.Add(new Bill(billID));
                        found = true;
                    }
                    else if (billMatch)
                    {
                        nr.Invoices.Add(new Invoice(invoiceID));
                        found = true;
                    }
                }
                if (!found)
                {
                    var nr = new NetReceivable(new Invoice(invoiceID), new Bill(billID));
                    netReceivables.Add(nr);
                }
                xRefDF.GetNextRecord();
            }

            var invoiceDF = new DelimitedFile(argsValidator.ReceivablesPath, "ASCII", '\n', (char)44, (char)59, (char)34);
            invoiceDF.GetNextRecord();
            while (!invoiceDF.EndOfFile)
            {
                var invoice = new Invoice();
                invoice.Id = invoiceDF.GetFieldByName(InvoiceFields.IdField);
                invoice.Amount = Convert.ToDouble(invoiceDF.GetFieldByName(InvoiceFields.Amount));
                invoice.Customer = invoiceDF.GetFieldByName(InvoiceFields.Customer);
                invoice.Date = DateTime.Parse(invoiceDF.GetFieldByName(InvoiceFields.Date));
                invoice.DueDate = DateTime.Parse(invoiceDF.GetFieldByName(InvoiceFields.DueDate));
                invoice.SetPastDue();
                var found = false;
                for (var k = 0; k < netReceivables.Count; k++)
                {
                    for (var i = 0; i < netReceivables[k].Invoices.Count; i++)
                    {
                        if (netReceivables[k].Invoices[i].Id.Equals(invoice.Id))
                        {
                            netReceivables[k].Invoices[i] = invoice;
                            found = true;
                            break;
                        }
                    }
                    if (found)
                    {
                        break;
                    }
                }
                if (!found)
                {
                    var newInvoice = new Invoice(invoice.Id);
                    newInvoice = invoice;
                    netReceivables.Add(new NetReceivable(invoice));
                }
                invoiceDF.GetNextRecord();
            }
            
            var billDF = new DelimitedFile(argsValidator.PayablesPath, "ASCII", '\n', (char)44, (char)59, (char)34);
            billDF.GetNextRecord();
            while (!billDF.EndOfFile)
            {
                var bill = new Bill();
                bill.Id = billDF.GetFieldByName(BillFields.IdField);
                bill.Amount = Convert.ToDouble(billDF.GetFieldByName(BillFields.Amount));
                bill.Vendor = billDF.GetFieldByName(BillFields.Vendor);
                bill.Date = DateTime.Parse(billDF.GetFieldByName(BillFields.Date));
                bill.DueDate = DateTime.Parse(billDF.GetFieldByName(BillFields.DueDate));
                bill.SetPastDue();
                var found = false;
                for (var k = 0; k < netReceivables.Count; k++)
                {
                    for (var i = 0; i < netReceivables[k].Bills.Count; i++)
                    {
                        if (netReceivables[k].Bills[i].Id.Equals(bill.Id))
                        {
                            netReceivables[k].Bills[i] = bill;
                            found = true;
                            break;
                        }
                    }
                    if (found)
                    {
                        break;
                    }
                }
                if (!found)
                {
                    var newBill = new Bill(bill.Id);
                    newBill = bill;
                    netReceivables.Add(new NetReceivable(bill));
                }

                billDF.GetNextRecord();
            }

            using (var _str = new StreamWriter(@"C:\Users\AaronGardner\Dropbox\Turnstone Operations Folder\Sale\LDiscovery\Cash Calculations\testoutput.txt"))
            {
                _str.AutoFlush = true;
                foreach (var nr in netReceivables)
                {
                    foreach (var i in nr.Invoices)
                    {
                        foreach (var b in nr.Bills)
                        {
                            var line = string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}\t{11}", i.Id, i.Date, i.DueDate, i.PastDue, i.Customer, i.Amount, b.Id, b.Date, b.DueDate, b.PastDue, b.Vendor, b.Amount);
                            _str.WriteLine(line);
                        }
                    }
                }
            }
        }
    }
}
