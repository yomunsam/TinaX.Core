# TinaX Framework - Core.

<img src="readme_res/logo.png" width = "420" height = "187" alt="logo" align=center />

[![LICENSE](https://img.shields.io/badge/license-NPL%20(The%20996%20Prohibited%20License)-blue.svg)](https://github.com/996icu/996.ICU/blob/master/LICENSE)
<a href="https://996.icu"><img src="https://img.shields.io/badge/link-996.icu-red.svg" alt="996.icu"></a>
[![LICENSE](https://camo.githubusercontent.com/3867ce531c10be1c59fae9642d8feca417d39b58/68747470733a2f2f696d672e736869656c64732e696f2f6769746875622f6c6963656e73652f636f6f6b6965592f596561726e696e672e737667)](https://github.com/yomunsam/TinaX/blob/master/LICENSE)

这是尚在开发阶段的新TinaX Framework的核心包。

This is the core package of the new TinaX Framework, which is still under development.
<br><br><br>

package name: `io.nekonya.tinax.core`

## Install this package

### Install via openupm

``` bash
# Install openupm-cli if not installed.
npm install -g openupm-cli
# OR yarn global add openupm-cli

#run install in your project root folder
openupm add io.nekonya.tinax.core
```

<br>

### Install via npm (UPM)

Modify `Packages/manifest.json` file in your project, and add the following code before "dependencies" node of this file:

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

<br>

### Install via git UPM:

You can use the following to install and use this package in UPM GUI.  

```
git://github.com/yomunsam/TinaX.Core.git
```

If you want to set a target version, you can use release tag like `#6.6.0-preview`. for detail you can see this page: [https://github.com/yomunsam/TinaX.Core/releases](https://github.com/yomunsam/TinaX.Core/releases)



<br><br>
------

## Dependencies

在安装之前，请先确保已安装如下依赖：

Before setup `TinaX.Core`, please ensure the following dependencies are installed by `Unity Package Manager`:

- [com.neuecc.unirx](https://github.com/neuecc/UniRx#upm-package) :`https://github.com/neuecc/UniRx.git?path=Assets/Plugins/UniRx/Scripts`
- [com.cysharp.unitask](https://github.com/Cysharp/UniTask#install-via-git-url) :`https://github.com/Cysharp/UniTask.git?path=src/UniTask/Assets/Plugins/UniTask`

> if you install packages by git UPM， You need to install the dependencies manually. Or dependencies will installed automatically by NPM / OpenUPM
>
> 如果您通过Git方式安装Package，那么您需要手动确保所有依赖项已被安装。如果使用NPM/OpenUPM安装本Package，则所有依赖都将自动被安装。 

<br><br>

------

## Third-Party

本项目中使用了以下优秀的第三方库：

The following excellent third-party libraries are used in this project:

- **[CatLib](https://catlib.io/)** : lightweight dependency injection container
- **[UniRx](https://github.com/neuecc/UniRx)** : Reactive Extensions for Unity
- **[UniTask](https://github.com/Cysharp/UniTask)** : Provides an efficient async/await integration to Unity.
- **[SharpZipLib](https://github.com/icsharpcode/SharpZipLib)** : a Zip, GZip, Tar and BZip2 library written entirely in C# for the .NET platform. 
- **[unity-toolbar-extender](https://github.com/marijnz/unity-toolbar-extender)** : Extend the Unity Toolbar with your own Editor UI code.
