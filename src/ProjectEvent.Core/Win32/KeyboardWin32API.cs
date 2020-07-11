using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace ProjectEvent.Core.Win32
{
    public static class KeyboardWin32API
    {
        //https://www.cnblogs.com/xielong/p/6763121.html

        public static Dictionary<string, byte> KeysMap = new Dictionary<string, byte>()
        {
            #region bVk参数 常量定义
            
        // 鼠标左键

{"LButton",  0x1},    
        // 鼠标右键

{"RButton",  0x2},    
        // CANCEL 键

{"Cancel",  0x3},     
        // 鼠标中键

{"MButton",  0x4},    
        // BACKSPACE 键

{"Back",  0x8},       
        // TAB 键

{"Tab",  0x9},        
        // CLEAR 键

{"Clear",  0xC},      
        // ENTER 键

{"Return",  0xD},     
        // SHIFT 键

{"Shift",  0x10},     
        // CTRL 键

{"Ctrl",  0x11},   
        // Alt 键  (键码18)

{"Alt",  18},         
        // MENU 键

{"Menu",  0x12},      
        // PAUSE 键

{"Pause",  0x13},     
        // CAPS LOCK 键

{"Capital",  0x14},   
        // ESC 键

{"Escape",  0x1B},    
        // SPACEBAR 键

{"Space",  0x20},     
        // PAGE UP 键

{"PageUp",  0x21},    
        // End 键

{"End",  0x23},       
        // HOME 键

{"Home",  0x24},      
        // LEFT ARROW 键

{"Left",  0x25},      
        // UP ARROW 键

{"Up",  0x26},        
        // RIGHT ARROW 键

{"Right",  0x27},     
        // DOWN ARROW 键

{"Down",  0x28},      
        // Select 键

{"Select",  0x29},    
        // PRINT SCREEN 键

{"Print",  0x2A},     
        // EXECUTE 键

{"Execute",  0x2B},   
        // SNAPSHOT 键

{"Snapshot",  0x2C},  
        // Delete 键

{"Delete",  0x2E},    
        // HELP 键

{"Help",  0x2F},      
        // NUM LOCK 键

{"Numlock",  0x90},   

        //常用键 字母键A到Z
        {"A",  65},
        {"B",  66},
        {"C",  67},
        {"D",  68},
        {"E",  69},
        {"F",  70},
        {"G",  71},
        {"H",  72},
        {"I",  73},
        {"J",  74},
        {"K",  75},
        {"L",  76},
        {"M",  77},
        {"N",  78},
        {"O",  79 },
        {"P",  80 },
        {"Q",  81 },
        {"R",  82 },
        {"S",  83 },
        {"T",  84 },
        {"U",  85 },
        {"V",  86 },
        {"W",  87 },
        {"X",  88 },
        {"Y",  89 },
        {"Z",  90 },

        //数字键盘0到9
        // 0 键

{"0",  48 },    
        // 1 键

{"1",  49 },    
        // 2 键

{"2",  50 },    
        // 3 键

{"3",  51 },    
        // 4 键

{"4",  52 },    
        // 5 键

{"5",  53 },    
        // 6 键

{"6",  54 },    
        // 7 键

{"7",  55 },    
        // 8 键

{"8",  56 },    
        // 9 键

{"9",  57 },    


        //0 键

{"Numpad0",  0x60 },    
        //1 键

{"Numpad1",  0x61 },    
        //2 键

{"Numpad2",  0x62 },    
        //3 键

{"Numpad3",  0x63 },    
        //4 键

{"Numpad4",  0x64 },    
        //5 键

{"Numpad5",  0x65 },    
        //6 键

{"Numpad6",  0x66 },    
        //7 键

{"Numpad7",  0x67 },    
        //8 键

{"Numpad8",  0x68 },    
        //9 键

{"Numpad9",  0x69 },    
        // MULTIPLICATIONSIGN(*)键

{"Multiply",  0x6A },   
        // PLUS SIGN(+) 键

{"Add",  0x6B },        
        // ENTER 键

{"Separator",  0x6C },  
        // MINUS SIGN(-) 键

{"Subtract",  0x6D },   
        // DECIMAL POINT(.) 键

{"Decimal",  0x6E },    
        // DIVISION SIGN(/) 键

{"Divide",  0x6F },     


        //F1到F12按键
        //F1 键

{"F1",  0x70 },   
        //F2 键

{"F2",  0x71 },   
        //F3 键

{"F3",  0x72 },   
        //F4 键

{"F4",  0x73 },   
        //F5 键

{"F5",  0x74 },   
        //F6 键

{"F6",  0x75 },   
        //F7 键

{"F7",  0x76 },   
        //F8 键

{"F8",  0x77 },   
        //F9 键

{"F9",  0x78 },   
        //F10 键

{"F10",  0x79 },  
        //F11 键

{"F11",  0x7A },  
        //F12 键

{"F12",  0x7B },  

        #endregion
        };
        /// <summary>
        /// 导入模拟键盘的方法
        /// </summary>
        /// <param name="bVk" >按键的虚拟键值</param>
        /// <param name= "bScan" >扫描码，一般不用设置，用0代替就行</param>
        /// <param name= "dwFlags" >选项标志：0：表示按下，2：表示松开</param>
        /// <param name= "dwExtraInfo">一般设置为0</param>
        [DllImport("user32.dll")]
        public static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        /// <summary>
        /// 按下按键
        /// </summary>
        /// <param name="key"></param>
        public static void Press(string key)
        {
            keybd_event(KeysMap[key], 0, 0, 0);
        }
        /// <summary>
        /// 松开按键
        /// </summary>
        /// <param name="key"></param>
        public static void Up(string key)
        {
            keybd_event(KeysMap[key], 0, 2, 0);
        }
    }
}
