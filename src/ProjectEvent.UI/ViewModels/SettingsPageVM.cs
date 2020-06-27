using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.UI.ViewModels
{
    public class SettingsPageVM
    {
        private readonly MainViewModel mainVM;

        public SettingsPageVM(MainViewModel mainVM)
        {
            this.mainVM = mainVM;
            mainVM.IsShowNavigation = false;
            mainVM.IsShowTitleBar = true;
            mainVM.Title = "设置";
        }

    }
}
