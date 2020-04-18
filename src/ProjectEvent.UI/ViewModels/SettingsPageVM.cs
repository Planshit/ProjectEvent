using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.UI.ViewModels
{
    public class SettingsPageVM
    {
        private readonly MainViewModel mainVM;

        public Command Goback { get; set; }
        public SettingsPageVM(MainViewModel mainVM)
        {
            this.mainVM = mainVM;

            Goback = new Command(new Action<object>(OnGoback));
        }

        private void OnGoback(object obj)
        {
            mainVM.Uri = "IndexPage";
        }
    }
}
