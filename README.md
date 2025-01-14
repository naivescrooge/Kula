# Kula 语言
Kula 是一个基于 .net Framework 平台的解释型脚本语言。

![Kula_Daiamondo](https://gimg2.baidu.com/image_search/src=http%3A%2F%2Fimg31.51tietu.net%2Fpic%2F2016-120803%2F20161208032542kjea4a3vlv462716.jpg&refer=http%3A%2F%2Fimg31.51tietu.net&app=2002&size=f9999,10000&q=a80&n=0&g=0n&fmt=jpeg?sec=1624628894&t=79f9d7388b65c5efa64aae6f37c53c7a)

## 更新日志
### kula - ice coffin 0 (2021-5-26)
* 较为完善的基本语法
* 动态强类型系统
* 足够基本使用的内置函数
* 不完全封装的错误提示

### kula - test (2021-5-23)
* 测试级的语法
* 逻辑基本不能使用
* 不够完善的错误提示

## 项目其他信息
### 主要负责人
[HanaYabuki @github.com](https://github.com/HanaYabuki)

### 参与贡献代码的方式
直接和我击剑

### 开源协议
[GPL3.0](./LICENSE)

------

## [简明教程 ：开始 Kula 语言](./docs/tutorial-0.md)
Kula 是一个由 C# 编写的解释型脚本语言。

## 简介
Kula 是一个具有即时编译器的，解释型脚本语言。他是由 [*HanamaruYabuki*](https://hanayabuki.github.com) 个人开发的，自拟标准的**玩具语言**。它可以基于 .net Framework 4.6 (或以上) 运行在 Windows 系统的计算机上。 

### Kula 所支持的(为数不多的)特性
* 环境易部署
* 语法简单易学
* 面向过程
* 支持 *Lambda* 函数式编程

> Q1: Kula 语言是什么？我怎么没听说过？我需要学习他吗？  
> A1: 要知道，Kula 暂时只是一个玩具语言，不具有 **很强的工程性 或 学习价值**，仅作为个人学习编译原理的一个中间产物。但是，如果您对这个项目感兴趣，并且愿意对语言加以研究 或 改进语言的一些细节设计，鄙人感激不尽。  
> Q2: 为什么叫 *Kula* 语言？    
> A2: 因为 [*Hana*](https://hanayabuki.github.com) 喜欢 Kula (KOF游戏角色)，就和 “为什么Java叫Java” 是相同的道理 (🍀

## 环境配置

Kula 使用环境的搭建十分容易。

### 在 Windows 平台使用
两种方式，如果您喜欢简洁明了的：    
1. 在当前 Git 仓库中寻找最新的 Release
2. 下载他
3. 把他解压到一个合适的位置，例如 `C:\Program Files\kula`
4. 在系统环境变量中 找到 `Path` 项，将 `kula.exe` 的所在路径写在 `Path` 中
5. 打开控制台，输入 `kula` 测试一下吧 ~

如果您喜欢折腾！！！
1. 克隆当前 Git 仓库到本地
2. 用 Visual Studio 等 C# 语言集成开发环境 打开 .sln 项目管理文件
3. 以 Release模式 编译当前项目
4. 在编译路径下，找到 `kula.exe`
5. 重复上一种方法的 3~5 步骤

采用这种方法可以让你提早使用尚未发布的新特性，当然，我可能Bug还没写完就推了上去，到时候咱自认倒霉就行。(🍀

### 在 Mac / Linux 平台使用
并不建议您在非 Windows 环境下折腾 .net，如果您执意要折腾的话，下面给出笼统的操作步骤
1. 安装 .net 运行时环境，例如 .net5 / .net Core
2. 克隆当前 Git 仓库
3. 开启新项目，设置目标平台为已安装的运行时环境
4. 将原有的代码复制到刚刚创建的新项目内
5. 手动修复Bug后编译
6. 找到编译目标文件后，配置操作系统下对应的环境

本方案未经过验证，仅为理论可行。

## 使用
作为解释型语言，Kula 支持 REPL(交互模式) 和 脚本模式 两种使用方案
```shell
$ kula                      # 默认为使用DEBUG模式下的交互模式
$ kula xxx.kula             # 单一参数启动，使用DEBUG模式下的脚本模式
$ kula xxx.kula  --release  # 两项参数启动，使用RELEASE模式下的脚本模式
```
