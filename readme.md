# Project Event
一个类似于iOS自动化快捷指令的PC版软件，提供了可以在Windows上创建事件监听方案的能力，当事件触发时自动执行方案所设定的操作。
<p align="center">
  <img alt="Project Event" src="https://i.loli.net/2020/07/05/Cu4DA7KHd1vqRcZ.jpg">
</p>

### 下载

你可以在这里 [Releases](https://github.com/Planshit/ProjectEvent/releases) 下载所有发布版本已编译的EXE文件压缩包，一般是Project Event xxx.zip。无需安装直接解压运行`ProjectEvent.UI.exe`。

### 基本使用

成功运行软件后你可以在右下角状态栏看到⚡图标，左键双击直接进入主界面，右键单击显示快捷菜单。

##### 创建事件监听/自动化方案

在主界面点击右上角的`+`按钮，进入创建方案界面。可以看到4个选项卡

描述：表示方案的一些信息，主要用于分辨该方案的用途；

事件：触发的前提条件；

条件：事件触发所需要的后续条件，但并不是所有事件都需要后续条件；

操作：成功触发事件后执行的操作。鼠标按住移动可以上下调整顺序，以从上往下的顺序依次执行。

##### 变量

部分`操作`支持输入内容作为选项，就可以在输入框中使用`变量`作为选项的一部分。变量有三种： **全局变量 **（即所有支持输入的`操作`随时都可以直接使用）； **事件变量 **（每个`事件`所产生的变量都不相同，有些事件不会产生变量）； **操作结果变量 **（当某些`操作`被执行完成后可以产生一些返回数据，在被执行的操作以下的操作中都可以通过`操作结果变量`获取到返回的数据）。点击输入框的位置可以看到变量选择框，点击需要的变量将被添加到当前输入框中。如果获取变量的时机不对时，你会看到输入框变成红色，表示输入框中存在错误且无法获取的变量。

##### 常见问题

如果启动软件没有任何反应，也看不到状态栏的⚡图标，那可能是没有安装运行所需要的组件[.NET Core3.0或更高版本](https://dotnet.microsoft.com/download/dotnet-core/current/runtime) 。如果已安装还是无法运行，请打开软件所在目录，查看是否存在`Log`文件夹，如果存在则表示软件发生了无法预料的错误，希望能将文件夹中的内容反馈给我。

### 运行条件

系统OS：Windows10

运行组件Runtime：[.NET Core3.0或更高](https://dotnet.microsoft.com/download/dotnet-core/current/runtime) **必须安装，否则无法运行**

### 问题反馈

建议优先选择该页面的[issues](https://github.com/Planshit/ProjectEvent/issues)功能，先搜索是否存在相同的问题。如果没有再提交，为了加快解决问题，请尽量描述清楚问题、操作步骤、系统基本信息（操作系统版本，版本号，在设置 > 系统 > 关于可以查看）。

如果你并不熟悉怎么操作[issues](https://github.com/Planshit/ProjectEvent/issues)，还可以通过底部的关于开发者链接，找到电子邮箱或微博发送问题反馈。

不局限于问题反馈，还欢迎提供想法和改进建议或者与开发者交谈。

### 其他

请勿从本页面指定的地址外下载软件，以防被恶意篡改导致电脑异常。

[Todo](https://github.com/Planshit/ProjectEvent/projects) [追踪开发进度和版本计划]

[开发者博客](http://thelittlepandaisbehind.com) [收藏防失联]

[关于开发者](http://thelittlepandaisbehind.com/about.html) [看看是谁写的狗屎]
