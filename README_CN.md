# TinaX Framework - Core.

<img src="readme_res/logo.png" width = "360" height = "160" alt="logo" align=center />

[![LICENSE](https://img.shields.io/badge/license-NPL%20(The%20996%20Prohibited%20License)-blue.svg)](https://github.com/996icu/996.ICU/blob/master/LICENSE)
<a href="https://996.icu"><img src="https://img.shields.io/badge/link-996.icu-red.svg" alt="996.icu"></a>
[![LICENSE](https://camo.githubusercontent.com/890acbdcb87868b382af9a4b1fac507b9659d9bf/68747470733a2f2f696d672e736869656c64732e696f2f62616467652f6c6963656e73652d4d49542d626c75652e737667)](https://github.com/yomunsam/TinaX/blob/master/LICENSE)

<!-- [![LICENSE](https://camo.githubusercontent.com/3867ce531c10be1c59fae9642d8feca417d39b58/68747470733a2f2f696d672e736869656c64732e696f2f6769746875622f6c6963656e73652f636f6f6b6965592f596561726e696e672e737667)](https://github.com/yomunsam/TinaX/blob/master/LICENSE) -->

TinaX是一个简洁、完整、愉快的开箱即用的Unity应用游戏开发框架， 它采用"Unity 包"的形式提供功能。

`TinaX.Core` 是TinaX的核心内容包.

- 框架核心
- 控制反转容器 （IoC）
- 事件广播系统
- 时间驱动系统
- 常用方法扩展

<br>

package name: `io.nekonya.tinax.core`

------

## 安装

### 使用[OpenUPM](https://openupm.com/)安装

``` bash
# Install openupm-cli if not installed.
npm install -g openupm-cli
# OR yarn global add openupm-cli

#run install in your project root folder
openupm add io.nekonya.tinax.core
```

<br>

### 通过npm安装 (UPM)

修改您的工程中的`Packages/manifest.json` 文件，并在文件的"dependencies"节点前添加如下内容：

``` json
"scopedRegistries": [
    {
        "name": "TinaX",
        "url": "https://registry.npmjs.org",
        "scopes": [
            "io.nekonya"
        ]
    },
    {
        "name": "package.openupm.com",
        "url": "https://package.openupm.com",
        "scopes": [
            "com.cysharp.unitask",
            "com.neuecc.unirx"
        ]
    }
],
```

如果在进行上述操作后，您仍然未能在"Unity Package Manager"窗口中找到TinaX的相关Packages，您也可以尝试刷新、重启编辑器，或手动添加如下配置到"dependencies":

``` json
"io.nekonya.tinax.core" : "6.6.3"
```

<br>

### 通过Git方式安装(UPM)

你可使用如下地址在Unity Package Manager窗口中安装本包： 

```
git://github.com/yomunsam/TinaX.Core.git
```

如果您想手动指定安装某个本本, 您可以使用 release tag, 例如 `#6.6.3`. 或访问Release页面了解细节: [https://github.com/yomunsam/TinaX.Core/releases](https://github.com/yomunsam/TinaX.Core/releases)

如不指定版本，Unity将会安装当前git仓库中的最新版本，可能会造成兼容性错误。

<br>

### 特殊地区用户指引

由于部分中文开发者居住和生活的地区较为特殊，可能无法以全世界大多数人相同的方式使用互联网。如果在安装时出现问题，可尝试使用如下方式解决：

- 请尽可能努力以各种方式与全世界大多数人保持同样的网络环境。
- 尝试使用[cnpm](https://developer.aliyun.com/mirror/NPM?from=tnpm)镜像,包括UPM方式和OpenUPM方式
- 使用openupm时，您可按照[此处说明](https://github.com/openupm/openupm-cli#command-options)尝试使用第三方register 
    - 需要说明的是，TinaX的第三方依赖，比如`UniRx`并没有发布到`npmjs.org`, 因此在cnpm中也是找不到它的。
- 如果下载源码遇到困难，可以试试从TinaX在[Gitee的镜像](https://gitee.com/organizations/nekonyas/projects)下载
- 可以在同一个项目中使用不同的方式安装处理不同的packages，如你可以使用"Gitee"、"cnpm"等渠道安装TinaX packages，而如果实在下载不下来第三方依赖（如"Unirx"）的话，直接给下载下来放在项目的Packages目录里也是可行的。（当然，把所有东西都一股脑扔进Packages目录也是可行的，不过这样可能升级的时候会不方便）



<br><br>
------

## Dependencies

- [com.neuecc.unirx](https://github.com/neuecc/UniRx#upm-package) :`https://github.com/neuecc/UniRx.git?path=Assets/Plugins/UniRx/Scripts`
- [com.cysharp.unitask](https://github.com/Cysharp/UniTask#install-via-git-url) :`https://github.com/Cysharp/UniTask.git?path=src/UniTask/Assets/Plugins/UniTask`

> 如果您通过Git方式安装Package，那么您需要手动确保所有依赖项已被安装。如果使用NPM/OpenUPM安装本Package，则所有依赖都将自动被安装。 

<br><br>

------

## Learn TinaX

您可以访问TinaX的[文档页面](https://tinax.corala.space/#/cmn-hans)来学习了解各个功能的使用

------

## Third-Party

本项目中使用了以下优秀的第三方库：

The following excellent third-party libraries are used in this project:

- **[CatLib](https://catlib.io/)** : lightweight dependency injection container
- **[UniRx](https://github.com/neuecc/UniRx)** : Reactive Extensions for Unity
- **[UniTask](https://github.com/Cysharp/UniTask)** : Provides an efficient async/await integration to Unity.
- **[SharpZipLib](https://github.com/icsharpcode/SharpZipLib)** : a Zip, GZip, Tar and BZip2 library written entirely in C# for the .NET platform. 
- **[unity-toolbar-extender](https://github.com/marijnz/unity-toolbar-extender)** : Extend the Unity Toolbar with your own Editor UI code.
