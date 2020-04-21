using ProjectEvent.UI.Models;
using ProjectEvent.UI.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.UI.ViewModels
{
    public class AddEventPageVM : AddEventPageModel
    {

        public Command AddActionCommand { get; set; }

        public AddEventPageVM()
        {
            Actions = new System.Collections.ObjectModel.ObservableCollection<BaseActionItemModel>();
            AddActionCommand = new Command(new Action<object>(OnAddActionCommand));
        }

        private void OnAddActionCommand(object obj)
        {
            Actions.Add(new WriteFileActionModel()
            {
                ActionName = "写文件",
                ActionType = Types.ActionType.WriteFile,
                Icon= "\xF2E6",
                Index = new Random().Next(10)
            });
        }
    }
}
