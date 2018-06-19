using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Integration;
using Microsoft.Reporting.WinForms;

namespace SaaMedW.ViewModel
{
    public class Report_InvoiceViewModel
    {
        private ReportViewer reportViewer;
        public WindowsFormsHost Viewer { get; set; }
        private Invoice invoice;
        public Report_InvoiceViewModel(Invoice _invoice)
        {
            invoice = _invoice;
            WindowsFormsHost windowsFormsHost = new WindowsFormsHost();
            reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            var templateName = Path.Combine(Path.GetDirectoryName(
                 System.Reflection.Assembly.GetExecutingAssembly().Location), "templates", "Invoice.rdl");
            reportViewer.LocalReport.ReportPath = templateName;
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("Invoice1", invoice.InvoiceDetail));

            // установка параметров
            var parameters = new List<ReportParameter>();
            parameters.Add(new ReportParameter("Id", invoice.Id.ToString()));
            parameters.Add(new ReportParameter("Firma", "Галиум"));
            parameters.Add(new ReportParameter("NumDt", "Счет № " + invoice.Id.ToString() + " от " + invoice.Dt.ToString("dd.MM.yyyy")));
            reportViewer.LocalReport.SetParameters(parameters);
            
            reportViewer.RefreshReport();
            windowsFormsHost.Child = reportViewer;
            this.Viewer = windowsFormsHost;
        }
    }
}
