using Microsoft.Toolkit.Uwp.Notifications;
using ProjectEvent.Core.Types;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Runtime.InteropServices;
using System.Windows;

namespace ProjectEvent.Core.Services
{
    [ClassInterface(ClassInterfaceType.None)]
    [ComSourceInterfaces(typeof(INotificationActivationCallback))]
    [Guid("891c9d99-9e37-4119-bad1-0505feed12b2"), ComVisible(true)]
    public class MyNotificationActivator : NotificationActivator
    {
        public override void OnActivated(string invokedArgs, NotificationUserInput userInput, string appUserModelId)
        {

            Application.Current.Dispatcher.Invoke(delegate
            {
                if (invokedArgs.Length > 0)
                {
                    ToastActionType actionType = (ToastActionType)int.Parse(invokedArgs.Split('`')[0]);
                    switch (actionType)
                    {
                        case ToastActionType.Url:
                            string url = invokedArgs.Split('`')[1];
                            if (!string.IsNullOrEmpty(url))
                            {
                                //MessageBox.Show("打开URL：" + url);
                                ProcessStartInfo psi = new ProcessStartInfo
                                {
                                    FileName = url,
                                    UseShellExecute = true
                                };
                                Process.Start(psi);
                            }
                            break;
                        default:
                            var client = new NamedPipeClientStream(nameof(ProjectEvent));
                            client.Connect();
                            StreamWriter writer = new StreamWriter(client);
                            string input = "0";
                            writer.WriteLine(input);
                            writer.Flush();
                            break;
                    }
                }
            });
            // TODO: Handle activation
        }
    }
}
