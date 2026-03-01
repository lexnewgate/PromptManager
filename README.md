# PromptManager

WPF 卡片管理工具，使用 .NET 8 + Material Design 实现。

## 功能

- 主窗口展示所有卡片（名称 + 摘要），支持网格布局
- 点击右上角「添加卡片」按钮新建卡片
- 双击卡片打开编辑弹窗
- 编辑弹窗：名称、摘要、内容（Markdown），左侧编辑、右侧实时预览
- 数据持久化到本地：`%AppData%/PromptManager/cards.json`

## 运行

```bash
dotnet run --project src/PromptManager/PromptManager.csproj
```

或使用 Visual Studio / Rider 打开 `PromptManager.sln` 后 F5 运行。

## 技术栈

- WPF、.NET 8
- Material Design In Xaml Toolkit（主题与控件）
- Markdown.Xaml（Markdown 预览）
- CommunityToolkit.Mvvm（MVVM）
