using ProjectEvent.Core.Event.Models;
using ProjectEvent.Core.Event.Structs;
using ProjectEvent.Core.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ProjectEvent.Core.Services.Tasks
{
    public class KeyboardTaskService : IKeyboardTaskService
    {
        private readonly IEventService eventService;
        private bool ishook = false;
        private KeyboardHook keyboardHook;
        public KeyboardTaskService(IEventService eventContainerService)
        {
            eventService = eventContainerService;
            eventService.OnAddEvent += EventService_OnAddEvent;
            eventService.OnRemoveEvent += EventService_OnRemoveEvent;
            eventService.OnUpdateEvent += EventService_OnUpdateEvent;
            keyboardHook = new KeyboardHook();
            keyboardHook.KeyDownEvent += KeyboardHook_KeyDownEvent;
            keyboardHook.KeyUpEvent += KeyboardHook_KeyUpEvent; ;
        }

        private void KeyboardHook_KeyUpEvent(int keycode)
        {
            var data = new KeyboardEventDataStruct()
            {
                Action = "up",
                KeyCode = keycode,
                KeyName = keyboardHook.ToKeyString(keycode)
            };
            Handle(data);
        }

        private void KeyboardHook_KeyDownEvent(int keycode)
        {
            var data = new KeyboardEventDataStruct()
            {
                Action = "down",
                KeyCode = keycode,
                KeyName = keyboardHook.ToKeyString(keycode)
            };
            Handle(data);
        }

        private void Handle(KeyboardEventDataStruct data)
        {
            var evs = eventService.
                GetEvents().
                Where(m => m.EventType == Event.Types.EventType.KeyboardEvent).
                ToList();

            foreach (var ev in evs)
            {
                eventService.Invoke(ev, data);
            }

        }

        private void EventService_OnUpdateEvent(EventModel oldValue, EventModel newValue)
        {
            Hook();
        }

        private void EventService_OnRemoveEvent(EventModel ev)
        {
            Hook();
        }

        private void EventService_OnAddEvent(EventModel ev)
        {
            Hook();
        }

        private void Hook()
        {
            if (eventService.
               GetEvents().
               Where(m => m.EventType == Event.Types.EventType.KeyboardEvent
               ).Any())
            {
                if (!ishook)
                {
                    //Hook键盘事件
                    keyboardHook.Start();
                    ishook = true;
                }
            }
            else
            {
                if (ishook)
                {
                    //取消
                    keyboardHook.Stop();
                    ishook = false;
                }
            }

        }
        public void Run()
        {
            Hook();
        }
    }
}
