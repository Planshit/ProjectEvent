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

        public MainService(IDeviceTaskService deviceTaskService,
             ITimerTaskService timerTaskService,
             IProcessTaskService processTaskService,
             IFileTaskService fileTaskService)
        {
            this.deviceTaskService = deviceTaskService;
            this.timerTaskService = timerTaskService;
            this.processTaskService = processTaskService;
            this.fileTaskService = fileTaskService;

        }
        public void Run()
        {
            //启动所有task service
            deviceTaskService.Run();
            timerTaskService.Run();
            processTaskService.Run();
            fileTaskService.Run();
        }
    }
}
