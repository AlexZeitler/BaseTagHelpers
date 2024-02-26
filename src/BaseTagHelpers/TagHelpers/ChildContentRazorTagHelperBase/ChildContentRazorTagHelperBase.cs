using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace BaseTagHelpers.TagHelpers.ChildContentRazorTagHelperBase;

/// <summary>
/// Base class for tag helpers that render a partial view with child content without a model
/// </summary>
public abstract class ChildContentRazorTagHelperBase : TagHelper
{
  [HtmlAttributeNotBound] [ViewContext] public ViewContext? ViewContext { get; set; }
  private readonly IHtmlHelper? _htmlHelper;
  private string? _partialName;

  public ChildContentRazorTagHelperBase(
    IHtmlHelper htmlHelper
  ) =>
    _htmlHelper = htmlHelper;


  /// <summary>
  /// Set the partial name to be used when rendering the partial
  /// </summary>
  /// <param name="partialName">Name / path of the partial view</param>
  protected void SetPartialName(
    [AspMvcView] [AspMvcPartialView] string partialName
  ) =>
    _partialName = partialName;

  public override async Task ProcessAsync(
    TagHelperContext context,
    TagHelperOutput output
  )
  {
    (_htmlHelper as IViewContextAware)?.Contextualize(ViewContext);

    var childContent = await output.GetChildContentAsync();
    var children = childContent.GetContent();

    output.TagMode = TagMode.StartTagAndEndTag;
    output.SuppressOutput();

    var content = await _htmlHelper.PartialAsync(_partialName, children);

    output.PreContent.AppendHtml(content);
  }
}
