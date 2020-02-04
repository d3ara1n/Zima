# Zima

这是一个通用包管理工具服务端.
Zima 取自爱死机中的同名人物, 寄寓于其精神, 希望能返璞归真, 回归极简. 出于此, Zima 仅含极少部分必要功能, 即包括少量的[API](https://api.swaggerhub.com/apis/Uni34/zima-api/1.0.0). 
大部分时候自定义不需要代码, 完全可以通过配置完成.

## 使用及[接口](https://api.swaggerhub.com/apis/Uni34/zima-api/1.0.0)
Zima 的对外接口分为两部分, 其一负责包管理, 另者负责accesskey验证. 其中后者可以被弃用, 而使用第三方accesskey验证服务接口代替. 自带的accesskey验证接口仅适用于测试或者私有化服务.

## 部署(Docker)
Zima 本体已发布到Docker Hub, 镜像名为 `zqy2419328026/zima` .

Zima 使用mysql作为服务端数据库, 使用前需要在配置文件中填入mysql的connectionstring.

使用如下bash命令在设备上创建服务.
```bash
# 创建项目目录, 该目录用于储存容器数据
mkdir zima
# Packages目录用于保存上传的包文件
cd zima && mkdir Packages
# 创建appsettings.json配置文件
vi appsettings.json
docker pull zqy2419328026/zima
# 默认http端口为927, '-p'将其映射到本机的927端口
# 容器内不带配置文件, 第二个'-v' 挂载配置到容器内. 第一个'-v'挂载包储存文件夹到容器
docker run --name zima -p 927:927 -d -v ~/zima/Packages:/app/Packages -v ~/zima/appsettings.json:/app/appsettings.json zqy2419328026/zima
```

Zima 初代不含日志服务, 运行成没成功自行进入容器内观察(反正诸位docker耍的都比我溜). 报错信息多与数据库有关, 自行排查.

## 部署(传统)
需要[dotnet sdk](https://dotnet.microsoft.com/).
```bash
# 克隆本项目到本地
git clone https://github.com/ac682/Zima.git
cd Zima/Zima
dotnet restore
# 编译
dotnet build
# 创建appsettings.json文件
vi bin/Debug/{arch}/appsettings.json
# 跑起来
dotnet run
```

## 配置文件
程序所需的所有配置都被保存在与可执行文件同目录的 `appsettings.json` 文件, 该文件具有以下默认模板:
```json
{
    "Application": {
        "Database": {
            "ConnectionString": "Server=your_mysql_db;Database=db_name;Uid=db_user;Pwd=db_passwd;"
        },
        "KeyAuthorization": {
            "TrustedKey": "a_magic_string_that_hard_to_guess",
            "KeyValidationSite": "http://localhost:927/api/validate?key={0}",
            "ResultPath": ""
        }
    }
}
```
配置项功能如其名字所示.

### 使用内置accesskey验证接口
这需要将 `KeyValidationSite` 设置为 `本机地址/api/validate?key={0}`, 其中 `key={0}` 不可省略且不可修改. 该字段值意味着使用自带的接口验证accesskey, 而验证方法为判断key参数是否匹配 `TrustedKey` 字段, 若匹配则使用输入的key参数作为operationkey, 否则404.

### 使用自建accesskey验证接口
自建接口仅支持http get, 通过query传递参数; 其返回内容为json格式或平原文本, 若accesskey不正确则返回404.

使用可访问的地址填入 `KeyValidationSite` 字段即可替换为自建接口. 其中请求的accesskey会被替换入该字段值中的 `{0}`.

对于json格式的response body, 还需要配置 `ResultPath` 字段用于解析operationkey. 注意: 该字段虽然名字里有 Path, 但其不代表 Path, 正确含义为 Property Name, 即指向 json object 中的对应属性名的属性的值, 而不是对应属性路径的属性的值.

对于平原格式的response body, 且其内容刚好就是operationkey, 则 `ResultPath` 留空.

## 支持(乞丐)作者
[爱发电](https://afdian.net/@nanjiu)
