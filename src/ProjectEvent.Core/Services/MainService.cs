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

        public MainService(IDeviceTaskService deviceTaskService,
             ITimerTaskService timerTaskService)
        {
            this.deviceTaskService = deviceTaskService;
            this.timerTaskService = timerTaskService;

        }
        public void Run()
        {
            //启动所有task service
            deviceTaskService.Run();
            timerTaskService.Run();
        }
    }
}
