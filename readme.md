# Project Event
一个类似于iOS自动化快捷指令的PC版软件，提供了可以在Windows上创建各种事件监听方案的能力，当事件触发时自动执行方案所设置的操作。依靠Project Event软件，无需编写代码也可以创造出各种实用、有趣的功能。
<p align="center">
  <img alt="VS Code in action" src="https://i.loli.net/2020/07/05/Cu4DA7KHd1vqRcZ.jpg">
</p>

### 下载

你可以在这里 [Releases](https://github.com/Planshit/ProjectEvent/releases) 下载所有发布版本已编译的EXE文件压缩包，一般是Project Event xxx.zip。无需安装直接解压运行`ProjectEvent.UI.exe`。

### 基本使用

成功运行软件后你可以在右下角状态栏看到⚡图标，左键双击直接进入主界面，右键单击显示菜单。

##### 创建事件监听/自动化方案

在主界面点击右上角的`+`按钮，进入创建方案界面。可以看到4个选项卡

描述：表示方案的一些信息，主要用于分辨方案的功能和用途；

事件：触发的前提条件；

条件：表示事件触发所需要的后续条件；

操作：成功触发后执行的操作。鼠标按住移动可以上下调整顺序，操作将从上往下依次执行。

##### 变量

有些`操作`可以输入或设置，就可能需要正确地输入或设置才能执行成功。输入内容支持一些变量，点击输入框的位置可以弹出变量选择框。如果你熟悉iOS的快捷指令那么应该无需过多解释什么是变量。

##### 常见问题

如果启动软件没有任何反应，也看不到图标，可能是没有安装运行需要的组件[.NET Core3.0或更高](https://dotnet.microsoft.com/download/dotnet-core/current/runtime) 。如果已安装还是无法运行请打开软件所在目录，查看是否存在`Log`文件夹，如果存在则表示软件出错了，请将文件夹内容反馈提交。

### 运行条件

系统OS:Windows10

运行组件Runtime:[.NET Core3.0或更高](https://dotnet.microsoft.com/download/dotnet-core/current/runtime) **必须安装，否则无法运行**

### 问题反馈

建议优先选择该页面的[issues](https://github.com/Planshit/ProjectEvent/issues)功能，先搜索是否存在相同的问题。如果没有再提交，为了加快解决问题，请尽量描述清楚问题、操作步骤、系统基本信息（操作系统版本，版本号，在设置 > 系统 > 关于可以查看）。能提供截图的情况下请一并提交。还可以查看软件目录下的`Log`文件夹，查找是否有运行日志，将日志内容附上。

如果你并不熟悉怎么操作[issues](https://github.com/Planshit/ProjectEvent/issues)，还可以通过底部的关于开发者链接，找到电子邮箱或微博发送问题反馈。

不局限于问题反馈，还欢迎提供想法和改进建议或者与开发者交谈。

### 其他

请勿从本页面指定的地址外下载软件，以防被恶意篡改导致电脑异常。

[Todo](https://github.com/Planshit/ProjectEvent/projects) [追踪开发进度和版本计划]

[开发者博客](http://thelittlepandaisbehind.com) [收藏防失联]

[关于开发者](http://thelittlepandaisbehind.com/about.html) [看看是谁写的狗屎]
