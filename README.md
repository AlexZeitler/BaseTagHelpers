# BaseTagHelpers

![Logo](https://raw.githubusercontent.com/AlexZeitler/BaseTagHelpers/main/icon.png)

[![NuGet](https://img.shields.io/nuget/v/BaseTagHelpers.svg)](https://www.nuget.org/packages/BaseTagHelpers)
[![NuGet](https://img.shields.io/nuget/dt/BaseTagHelpers.svg)](https://www.nuget.org/packages/BaseTagHelpers)
[![Discord](https://img.shields.io/discord/1070453198000767076)](https://discord.gg/pR6duvNHtV)

BaseTagHelpers is a collection of ASP.NET Core Tag Helper base classes to create Tag Helpers from.

## Installation

You can install the package via NuGet:

```bash
dotnet add package BaseTagHelpers
```

## Usage

### RazorTagHelperBase without Child Content

The `RazorTagHelperBase` class is a base class for creating Tag Helpers that render Razor content.

```csharp
public record MyTagHelperModel(string FirstName, string LastName);

MyRazorTagHelper : RazorTagHelperBase
{
  [HtmlAttributeName("model")] public MyTagHelperModel? Model { get; set; }
    
  public MyRazorTagHelper(
    IHtmlHelper htmlHelper
  ) : base(htmlHelper)
  {
  }
    
  public override void Process(TagHelperContext context, TagHelperOutput output)
  {
    SetPartialName(
      "MyRazorTagHelper.cshtml",
      Model
    );
    await base.ProcessAsync(context, output);
  }
}
```

```html
@model MyTagHelperModel
<div>
  <h1>Hello @Model.FirstName @Model.LastName</h1>
</div>
```

### RazorTagHelperBase with Child Content

```csharp
public class MyTagHelperModel : IHasChildContent
{
  public string FirstName { get; set; }
  public string LastName { get; set; }
  public string? ChildContent { get; set; }
}

MyRazorTagHelper : RazorTagHelperBase
{
  [HtmlAttributeName("model")] public MyTagHelperModel? Model { get; set; }
    
  public MyRazorTagHelper(
    IHtmlHelper htmlHelper
  ) : base(htmlHelper)
  {
  }
    
  public override void Process(TagHelperContext context, TagHelperOutput output)
  {
    SetPartialName(
      "MyRazorTagHelper.cshtml",
      Model,
      true // Set to true to allow child content
    );
    await base.ProcessAsync(context, output);
  }
}
```

```html
@model MyTagHelperModel
<div>
  <h1>Hello @Model.FirstName @Model.LastName</h1>
</div>
<div>
  @Html.Raw(@Model.ChildContent)
</div>
```

### ChildContentRazorTagHelperBase

The `ChildContentRazorTagHelperBase` class is a base class for creating Tag Helpers that render child content without having a model.

```csharp
MyChildContentTagHelper : ChildContentRazorTagHelperBase
{
  public MyChildContentTagHelper(
    IHtmlHelper htmlHelper
  ) : base(htmlHelper)
  {
  }
    
  public override void Process(TagHelperContext context, TagHelperOutput output)
  {
    SetPartialName(
      "MyChildContentTagHelper.cshtml"
    );
    await base.ProcessAsync(context, output);
  }
}
```

```html
@model string

<div>
  @Html.Raw(@Model)
</div>
```

## Want to contribute?

This project is just getting off the ground and could use some help with cleaning things up and refactoring.

If you want to contribute - we'd love it! Just open an issue to work against so you get full credit for your fork. You can open the issue first so we can discuss and you can work your fork as we go along.

If you see a bug, please be so kind as to show how it's failing, and we'll do our best to get it fixed quickly.

Before sending a PR, please [create an issue](https://github.com/AlexZeitler/BaseTagHelpers/issues/new) to introduce your idea and have a reference for your PR.

We're using [conventional commits](https://www.conventionalcommits.org), so please use it for your commits as well.

### Discussions

If you want to discuss an `BaseTagHelpers` issue or PR in more detail, feel free to [start a discussion](https://github.com/AlexZeitler/BaseTagHelpers/discussions).

You can also join our [Discord server](https://discord.gg/pR6duvNHtV) to discuss the project.

## License

The MIT License (MIT). Please see [License File](LICENSE) for more information.