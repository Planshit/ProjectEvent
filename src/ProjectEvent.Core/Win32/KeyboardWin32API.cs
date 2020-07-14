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

{"Back",  8},       
        // TAB 键

{"Tab",  9},        
        // CLEAR 键

{"Clear",  0xC},      
        // ENTER 键

{"Return",  13},     
        // SHIFT 键

{"Shift",  16},     
        // CTRL 键

{"Ctrl",  17},   
        // Alt 键  (键码18)

{"Alt",  18},         
        // Windows 键

{"Win",  91},      
        // PAUSE 键

{"Pause",  19},     
        // CAPS LOCK 键

{"Capital",  20},   
        // ESC 键

{"Esc",  27},    
        // SPACEBAR 键

{"Space",  32},     
        // PAGE UP 键

{"PageUp",  33},
//page down键
{"PageDown",  34},
        // End 键

{"End",  35},       
        // HOME 键

{"Home",  36},      
        // LEFT ARROW 键

{"Left",  37},      
        // UP ARROW 键

{"Up",  38},        
        // RIGHT ARROW 键

{"Right",  39},     
        // DOWN ARROW 键

{"Down",  40},      
        // Select 键

{"Select",  41},    
        // PRINT SCREEN 键

{"Print",  42},     
        // EXECUTE 键

{"Execute",  43},   
        // SNAPSHOT 键

{"Snapshot",  44},  
        // Delete 键

{"Del",  46},    
        // HELP 键

{"Help",  47},      
        // NUM LOCK 键

{"Numlock",  144},   

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

{"Numpad0",  96 },    
        //1 键

{"Numpad1",  97 },    
        //2 键

{"Numpad2",  98 },    
        //3 键

{"Numpad3",  99 },    
        //4 键

{"Numpad4",  100 },    
        //5 键

{"Numpad5",  101 },    
        //6 键

{"Numpad6",  102 },    
        //7 键

{"Numpad7",  103 },    
        //8 键

{"Numpad8",  104 },    
        //9 键

{"Numpad9",  105 },    
        // MULTIPLICATIONSIGN(*)键

{"Multiply",  106 },   
        // PLUS SIGN(+) 键

{"Add",  107 },        
        // 分隔键

{"Separator",  108 },  
        // MINUS SIGN(-) 键

{"Subtract",  109 },   
        // DECIMAL POINT(.) 键

{"Decimal",  110 },    
        // DIVISION SIGN(/) 键

{"Divide",  111 },     


        //F1到F12按键
        //F1 键

{"F1",  112 },   
        //F2 键

{"F2",  113 },   
        //F3 键

{"F3",  114 },   
        //F4 键

{"F4",  115 },   
        //F5 键

{"F5",  116 },   
        //F6 键

{"F6",  117 },   
        //F7 键

{"F7",  118 },   
        //F8 键

{"F8",  119 },   
        //F9 键

{"F9",  120 },   
        //F10 键

{"F10",  121 },  
        //F11 键

{"F11",  122 },  
        //F12 键

{"F12",  123 },  

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
