using Microsoft.Toolkit.Uwp.Notifications;
using ProjectEvent.Core.Services.Tasks;
using ProjectEvent.Core.Services.TimerTask;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.Core.Services
{
    public class MainService : IMainService
    {
        private readonly IDeviceTaskService deviceTaskService;
        private readonly ITimerTaskService timerTaskService;
        private readonly IProcessTaskService processTaskService;
        private readonly IFileTaskService fileTaskService;
        private readonly IKeyboardTaskService keyboardTaskService;
        private readonly INetworkStatusTaskService networkStatusTaskService;
        private readonly IBluetoothTaskService bluetoothTaskService;
        public MainService(IDeviceTaskService deviceTaskService,
             ITimerTaskService timerTaskService,
             IProcessTaskService processTaskService,
             IFileTaskService fileTaskService,
             IKeyboardTaskService keyboardTaskService,
             INetworkStatusTaskService networkStatusTaskService,
             IBluetoothTaskService bluetoothTaskService)
        {
            this.deviceTaskService = deviceTaskService;
            this.timerTaskService = timerTaskService;
            this.processTaskService = processTaskService;
            this.fileTaskService = fileTaskService;
            this.keyboardTaskService = keyboardTaskService;
            this.networkStatusTaskService = networkStatusTaskService;
            this.bluetoothTaskService = bluetoothTaskService;

        }
        public void Run()
        {
            DesktopNotificationManagerCompat.RegisterActivator<MyNotificationActivator>();

            //启动所有task service
            deviceTaskService.Run();
            timerTaskService.Run();
            processTaskService.Run();
            fileTaskService.Run();
            keyboardTaskService.Run();
            networkStatusTaskService.Run();
            bluetoothTaskService.Run();
        }
    }
}
