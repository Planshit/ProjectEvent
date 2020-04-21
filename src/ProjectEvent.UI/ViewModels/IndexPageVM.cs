using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.UI.ViewModels
{
    public class IndexPageVM
    {
        private readonly MainViewModel mainVM;
        public Command RedirectCommand { get; set; }

        public IndexPageVM(MainViewModel mainVM)
        {
            this.mainVM = mainVM;
            RedirectCommand = new Command(new Action<object>(OnRedirectCommand));
        }

        private void OnRedirectCommand(object obj)
        {
            mainVM.Uri = obj.ToString();
        }
    }
}
