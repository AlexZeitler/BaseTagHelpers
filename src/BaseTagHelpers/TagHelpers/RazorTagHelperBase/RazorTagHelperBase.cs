using JetBrains.Annotations;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace BaseTagHelpers.TagHelpers.RazorTagHelperBase;

/// <summary>
/// Interface for models that have child content
/// </summary>
public interface IHasChildContent
{
  string? ChildContent { get; set; }
}

/// <summary>
/// Base class for tag helpers that render a partial view with a model and might have child content
/// </summary>
/// <typeparam name="TModel"></typeparam>
public abstract class RazorTagHelperBase<TModel> : TagHelper
{
  [HtmlAttributeNotBound] [ViewContext] public ViewContext? ViewContext { get; set; }
  private readonly IHtmlHelper _htmlHelper;

  protected RazorTagHelperBase(
    IHtmlHelper htmlHelper
  ) =>
    _htmlHelper = htmlHelper;

  private string? _partialName;
  private TModel? _model;
  private bool _allowChildContent;

  /// <summary>
  /// Set the partial name and model to be used when rendering the partial
  /// </summary>
  /// <param name="partialName">Name / path of the partial view</param>
  /// <param name="model">The model instance</param>
  /// <param name="allowChildContent">Allow child content to be placed inside the Tag Helper</param>
  protected void SetPartialName(
    [AspMvcView] [AspMvcPartialView] string partialName,
    TModel? model,
    bool allowChildContent = false
  )
  {
    _partialName = partialName;
    _model = model;
    _allowChildContent = allowChildContent;
  }

  public override async Task ProcessAsync(
    TagHelperContext context,
    TagHelperOutput output
  )
  {
    (_htmlHelper as IViewContextAware)?.Contextualize(ViewContext);

    var childContent = (await output.GetChildContentAsync()).GetContent();

    IHtmlContent viewContent;
    switch (_allowChildContent)
    {
      case false when !string.IsNullOrWhiteSpace(childContent):
        throw new InvalidOperationException("Invalid child content. Set allowChildContent to true");
      case true:
      {
        output.TagMode = TagMode.StartTagAndEndTag;
        output.SuppressOutput();

        if (_model is IHasChildContent modelWithChildContent)
          modelWithChildContent.ChildContent = childContent;
        else
          throw new InvalidOperationException(
            $"Model of type {typeof(TModel).Name} does not implement IHasChildContent"
          );

        viewContent = await _htmlHelper.PartialAsync(_partialName, modelWithChildContent);
        break;
      }
      default:
        viewContent = await _htmlHelper.PartialAsync(_partialName, _model);
        break;
    }

    output.PreContent.AppendHtml(viewContent);
  }
}
