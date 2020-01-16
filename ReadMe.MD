# 这个仓库是干什么的？
这个仓库保存了create-lib-reference的代码，并会在修改代码后自动编译成二进制文件，然后推送到[qt-deploy-pri](https://github.com/sanjusss/qt-deploy-pri)仓库中。

# create-lib-reference可以做什么？
create-lib-reference可以在[qt-deploy-pri](https://github.com/sanjusss/qt-deploy-pri)的框架下，为Qt/QMake的动态库/静态库/应用程序代码生成pri文件。其他项目包含这个pri文件后，将自动添加相关依赖，生成时也会复制对应的依赖文件。

# 如何使用create-lib-reference？
动态链接库：
```cmd
create-lib-reference <debug|release> <lib path> <dll path> <include path> <pro path>
```
静态链接库：
```cmd
create-lib-reference <debug|release> <lib path> <include path> <pro path>
```
应用程序：
```cmd
create-lib-reference <debug|release> <exe path> <pro path>
```
其他pro解决方案：
```cmd
create-lib-reference <pro path>
```