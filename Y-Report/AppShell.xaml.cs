using Y_Report.Views;

namespace Y_Report
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            //Routing
            Routing.RegisterRoute(nameof(SearchDocumentByCodesView), typeof(SearchDocumentByCodesView));
        }
    }
}
