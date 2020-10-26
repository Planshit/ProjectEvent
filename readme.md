# Project Event

一个类似于iOS自动化快捷指令的PC版软件，提供了可以在Windows上创建事件监听方案的能力，当事件触发时自动执行方案所设定的操作。

<p align="center">
  <img alt="Project Event" src="https://i.loli.net/2020/07/05/Cu4DA7KHd1vqRcZ.jpg">
</p>

### 基本使用

安装启动软件后你可以在右下角状态栏看到⚡图标，左键双击直接进入主界面，右键单击显示快捷菜单。

##### 创建事件监听/自动化方案

在主界面点击右上角的`+`按钮，进入创建方案界面。可以看到4个选项卡

描述：表示方案的一些信息，主要用于分辨该方案的用途；

事件：触发的前提条件；

条件：事件触发所需要的后续条件，但并不是所有事件都需要后续条件；

操作：成功触发事件后执行的操作。鼠标按住移动可以上下调整顺序，以从上往下的顺序依次执行。

##### 变量

部分`操作`支持输入内容作为选项，就可以在输入框中使用`变量`作为选项的一部分。变量有三种： **全局变量 **（即所有支持输入的`操作`随时都可以直接使用）； **事件变量 **（每个`事件`所产生的变量都不相同，有些事件不会产生变量）； **操作结果变量 **（当某些`操作`被执行完成后可以产生一些返回数据，在被执行的操作以下的操作中都可以通过`操作结果变量`获取到返回的数据）。点击输入框的位置可以看到变量选择框，点击需要的变量将被添加到当前输入框中。如果获取变量的时机不对时，你会看到输入框变成红色，表示输入框中存在错误且无法获取的变量。

### 反馈

[问题反馈](https://github.com/Planshit/ProjectEvent/issues/new?assignees=&labels=bug&template=bug-report-----.md&title=%5Bbug%5D+%E8%BD%AF%E4%BB%B6%E6%9C%89%E9%97%AE%E9%A2%98)

[功能建议](https://github.com/Planshit/ProjectEvent/issues/new?assignees=&labels=enhancement&template=feature-request-----.md&title=%5Bfeature%5D+%E6%88%91%E5%B8%8C%E6%9C%9B%E8%83%BD%E5%A2%9E%E5%8A%A0%2F%E8%B0%83%E6%95%B4...)

### 其他

[Todo](https://github.com/Planshit/ProjectEvent/projects) [追踪开发进度和版本计划]

[用户帮助文档](https://littlepanda.gitbook.io/project-event/)
