using ProjectEvent.Core.Action;
using ProjectEvent.Core.Event.Models;
using ProjectEvent.Core.Extensions;
using ProjectEvent.Core.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Text;

namespace ProjectEvent.Core.Services.Tasks
{
    public class ProcessTaskService : IProcessTaskService
    {
        public event EventHandler OnEventTrigger;
        private readonly IEventService eventService;
        private ManagementEventWatcher watcher;
        private bool isRun = false;

        //游戏
        /// <summary>
        /// 游戏平台启动记录
        /// </summary>
        private DateTime gamePlatformRunLog = DateTime.Now;
        /// <summary>
        /// 游戏关联的进程（未经过大量测试）
        /// </summary>
        private readonly string[] gameAssociatedProcesses = { "GameBarPresenceWriter.exe" };
        public ProcessTaskService(IEventService eventService)
        {
            this.eventService = eventService;
            EventQuery query = new EventQuery();
            query.QueryString = "SELECT * FROM" +
                " __InstanceCreationEvent WITHIN 1 " +
                "WHERE TargetInstance isa 'Win32_Process'";
            watcher = new ManagementEventWatcher(query);
            watcher.EventArrived += NewProcess_Created;
            eventService.OnAddEvent += _eventService_OnAddAndRemoveEvent;
            eventService.OnRemoveEvent += _eventService_OnAddAndRemoveEvent;
            eventService.OnUpdateEvent += EventService_OnUpdateEvent;
        }

        private void EventService_OnUpdateEvent(EventModel oldValue, EventModel newValue)
        {
            Run();
        }

        private void _eventService_OnAddAndRemoveEvent(EventModel @event)
        {
            Run();
        }

        public void Run()
        {
            var hasEv = eventService.
               GetEvents().
               Where(
                m => m.EventType == Event.Types.EventType.OnProcessCreated ||
               m.EventType == Event.Types.EventType.OnProcessShutdown ||
               m.EventType == Event.Types.EventType.RunGameEvent
               ).
               Any();

            if (hasEv && !isRun)
            {
                //存在进程创建事件且未启动时
                watcher.Start();
                isRun = true;
            }
            else if (!hasEv && isRun)
            {
                //不存在进程创建事件且在运行中时
                watcher.Stop();
                isRun = false;
            }
        }
        private void HandleCreatedProcess(ManagementBaseObject baseObject)
        {
            var evs = eventService.
                GetEvents().
                Where(m => m.EventType == Event.Types.EventType.OnProcessCreated ||
                m.EventType == Event.Types.EventType.OnProcessShutdown).
                ToList();

            foreach (var ev in evs)
            {
                if (ev.EventType == Event.Types.EventType.OnProcessCreated)
                {
                    eventService.Invoke(ev, baseObject);
                }
            }

        }

        private void HandleRunGame(ManagementBaseObject baseObject)
        {
            var evs = eventService.
                GetEvents().
                Where(m => m.EventType == Event.Types.EventType.RunGameEvent).
                ToList();
            if (evs.Count > 0)
            {
                bool isGame = false;
                GamePlatformType gamePlatform = GamePlatformType.Other;

                //判断游戏平台
                var p = ((ManagementBaseObject)(baseObject as ManagementBaseObject)["TargetInstance"]);
                //进程名称
                string processName = p.TryGetProperty("Name");
                //命令
                string commandLine = p.TryGetProperty("CommandLine");
                if (processName == "GameOverlayUI.exe")
                {
                    //steam平台
                    isGame = true;
                    gamePlatform = GamePlatformType.Steam;
                    gamePlatformRunLog = DateTime.Now;
                    Debug.WriteLine("已启动steam平台游戏");
                }
                else if (commandLine.Contains("-epicapp"))
                {
                    //epic
                    isGame = true;
                    gamePlatform = GamePlatformType.Epic;
                    gamePlatformRunLog = DateTime.Now;
                    Debug.WriteLine("已启动epic平台游戏");

                }
                else if (processName == "TenSafe_1.exe")
                {
                    //腾讯游戏
                    isGame = true;
                    gamePlatform = GamePlatformType.Tencent;
                    gamePlatformRunLog = DateTime.Now;
                    Debug.WriteLine("已启动腾讯平台游戏");

                }
                else if (gameAssociatedProcesses.Contains(processName))
                {
                    //游戏关联进程启动
                    TimeSpan diff = DateTime.Now - gamePlatformRunLog;
                    if (diff.TotalMilliseconds > 1000)
                    {
                        isGame = true;
                    }
                   
                }


                if (isGame)
                {
                    foreach (var ev in evs)
                    {
                        eventService.Invoke(ev, gamePlatform.ToString());
                    }
                }

            }


        }

        private void NewProcess_Created(object sender, EventArrivedEventArgs se)
        {
            HandleCreatedProcess(se.NewEvent);
            HandleRunGame(se.NewEvent);
        }
    }
}
