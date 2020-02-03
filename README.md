<p align="center"><img width="128" height="128" src="https://cdn.tdouplus.com/img/logo.png"></p>

<p align="center">
<a href="https://github.com/t-dou/kit-cre/blob/master/LICENSE">
  <img src="https://img.shields.io/badge/license-MIT-blue.svg" title="license-mit" /></a>
<a href="https://ci.appveyor.com/project/gjmvvv/kit">
  <img src="https://ci.appveyor.com/api/projects/status/tk3o571mwbw2rykj?svg=true" title="Build status"/></a>
<a href="https://github.com/t-dou/kit/">
  <img src="https://img.shields.io/badge/version-v1-green.svg" title="GitHub version" ></a>
<a href="https://github.com/t-dou/kit/releases">
  <img src="https://img.shields.io/badge/Download-1k-green.svg" title="Downloads" /></a>
<a href="https://github.com/t-dou/Kit">
  <img src="https://img.shields.io/github/stars/t-dou/Kit.svg?style=social&label=Stars" title="GitHub stars" /></a>
<a href="https://github.com/t-dou/Kit">
  <img src="https://img.shields.io/github/forks/t-dou/Kit.svg?style=social&label=Fork" title="GitHub forks" /></a>
</p>

> We are currently preparing to convert the comments in the code to the English version. We look forward to your joining.

了解 [Core](https://github.com/t-dou/kit-core) 源码


## 关于 Kit[尚未出生,孵化中. . . ]

Kit 是Unity3D开发的工具包集合, 集成常见的开发组件,工具,组件库等. 免于重复造轮子
,Kit设计初衷则是根据业务需求自由组合搭配其中组件\tool\dll等,项目在任何阶段都可以轻松接入。


## 主要特色

kit-core [.net]

- base
	- 静态工具(时间戳转换,字符串优化,IO相关操作)
	- 事件消息
	- 引用池
- 自定义池
- 线程池


kit-unity
- [ ] (Timeline)新手引导编辑器
- [ ] (Timeline)剧情编辑器
- [ ] 任务编辑器
- [ ] 技能编辑器
- [ ] 动画编辑器
- [ ] 特效编辑器
- [ ] 2D地图编辑器
	- 根据刷的方块或其他自动生成1个物理碰撞或安装指定规则生成大的碰撞
	- 刷地图版块功能
- [ ] ***Setting*** 实现本地数据缓存, key=value
- [ ] ***Network*** 实现网络连接 tcp,udp,kcp
	- [ ] socket-tcp protobuf
	- [ ] scoket-udp
	- [ ] socket-kcp
- [ ] ***FSM*** 有限状态机
- [ ] ***Download*** 实现并发下载,多线程下载 
- [ ] ***Res*** 集成 Resources,StreamingAssets-AB,网络下载AB,管理资源, 基于XAsset实现 AssetBundle,自定义开发AssetBundleEditor指定打包规则.
	- [ ] ***Scene*** 基于Res(编辑器、AB),实现场景之间切换,附加,移除.
	- [ ] ***Audio*** 基于Res(编辑器、AB),网络动态下载,网络在线资源(mp3,wav)等
	- [ ] ***Picture*** 基于Res(编辑器、AB),实现Sprite自动化引用管理以及释放,网络动态下载,网络在线资源
	- [ ] ***Entity*** 基于Res(编辑器、AB),实现GameObject 对象池处理资源加载卸载

 
kit-tool
- unity 自动打包
	- [ ] android@{apk}
	- [ ] ios@{xcode project,ipa}
	- [ ] android@{apk} ios@{ipa] 自动上传 fir.im
	- [ ] ios@{ipa} 自动上传appstore






## 学习 Kit

[文档](https://kylin.app/) 

或者您也可以通过 issues 来提出您的问题，我们及时为您解答。

请不要提问「现成」问题，即那些简单搜一搜就能得到答案的，对经验和洞察力几乎没有要求的问题。 

详请参考《[提问的智慧](https://github.com/ryanhanwu/How-To-Ask-Questions-The-Smart-Way/blob/master/README-zh_CN.md)》


## 授权

开源许可：[MIT license](http://opensource.org/licenses/MIT).


## 项目开发计划

进入 [Kit开发计划](https://www.teambition.com/project/5c641818c156ca00170bcc98/tasks/scrum/5c6418a49502f00017416bd7)来了解未来的开发序列。


## 技术支持

* E-mail: me@tdouplus.com


## 优秀的开源框架/项目
- [QFramework](https://github.com/liangxiegame/QFramework) QFramework 是一套 渐进式 的 快速开发 框架。目标是作为无框架经验的公司、独立开发者、以及 Unity3D 初学者们的 第一套框架。框架内部积累了多个项目的在各个技术方向的解决方案。学习成本低，接入成本低，重构成本低，二次开发成本低，文档内容丰富(提供使用方式以及原理、开发文档)。

- [XAssets](https://github.com/xasset/xasset) xasset 提供了一种使用资源路径的简单的方式来加载资源，简化了Unity项目资源打包，更新，加载，和回收的作业流程。 ([fjy](https://github.com/fengjiyuan))

- [GameFramework](http://gameframework.cn/) 是一个基于 Unity 5.3+ 引擎的游戏框架，主要对游戏开发过程中常用模块进行了封装，很大程度地规范开发过程、加快开发速度并保证产品质量。（[@Ellan](https://github.com/EllanJiang)）

- [ET框架](https://github.com/egametang/ET) 是一个Unity3d客户端+C#分布式服务端框架。使用组件式开发，提供客户端热更，服务端热更功能，提供erlang式分布式消息机制（[@熊猫](https://github.com/egametang)）

- [CatLib](https://catlib.io) 是一套渐进式的服务提供者框架。框架为客户端提供多个实现，并把他们从多个实现中解耦出来。服务提供者的改变对它们的客户端是透明的，这样提供了更好的可扩展性。她不仅易于上手，还便于与第三方库或既有项目整合。([@喵喵](https://github.com/yb199478)) 

- [BlackFire](https://github.com/BlackFire-Studio/BlackFire) Framework 是专门为了提高中小型企业程序研发团队工作效率和降低中小型企业研发成本而设计的Unity3D游戏开发框架，框架遵循MIT协议，目前还在开发阶段，预计未来框架将友好地面向游戏、三维仿真、VR、AR、Web、区块链等业务开发团队。 ([@Alan](https://github.com/0x69h)) 
