using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Text.Encodings.Web;

namespace E_commerce_23TH0024.Common.Helpers
{
    public static class ButtonHelper
    {
        public static IHtmlContent ModalButton(this IHtmlHelper htmlHelper, string text, string btnClass, string extraClass, string modalId)
        {
            var button = new TagBuilder("button");
            button.Attributes["type"] = "button";
            button.AddCssClass($"btn {btnClass} {extraClass}");
            button.Attributes["data-bs-toggle"] = "modal";
            button.Attributes["data-bs-target"] = $"#{modalId}";
            button.InnerHtml.Append(text);
            return button;
        }

        // Khôi phục
        public static IHtmlContent RestoreButton(this IHtmlHelper htmlHelper, object id, string url, string btnClass = "btn-success btn-sm")
        {
            // Form tag
            var form = new TagBuilder("form");
            form.Attributes["action"] = url;
            form.Attributes["method"] = "post";
            form.Attributes["onsubmit"] = "return confirm('Bạn có chắc chắn muốn khôi phục?');";
            form.AddCssClass("d-inline");

            // Anti-forgery token
            var antiForgery = htmlHelper.AntiForgeryToken();

            // Hidden ID input
            var hidden = new TagBuilder("input");
            hidden.Attributes["type"] = "hidden";
            hidden.Attributes["name"] = "id";
            hidden.Attributes["value"] = id?.ToString();

            // Submit button
            var button = new TagBuilder("button");
            button.Attributes["type"] = "submit";
            button.AddCssClass($"btn {btnClass}");
            button.InnerHtml.Append("Khôi phục");

            // Gộp các phần tử vào form
            using var writer = new System.IO.StringWriter();
            antiForgery.WriteTo(writer, HtmlEncoder.Default);
            hidden.WriteTo(writer, HtmlEncoder.Default);
            button.WriteTo(writer, HtmlEncoder.Default);
            form.InnerHtml.AppendHtml(writer.ToString());

            return form;
        }
        // Xóa vĩnh viễn
        public static IHtmlContent DeleteHardButton(this IHtmlHelper htmlHelper, object id, string url, string btnClass = "btn-danger btn-sm")
        {
            // Form tag
            var form = new TagBuilder("form");
            form.Attributes["action"] = url;
            form.Attributes["method"] = "post";
            form.Attributes["onsubmit"] = "return confirm('Bạn có chắc chắn muốn xóa vĩnh viễn?');";
            form.AddCssClass("d-inline");

            // Anti-forgery token
            var antiForgery = htmlHelper.AntiForgeryToken();

            // Hidden ID input
            var hidden = new TagBuilder("input");
            hidden.Attributes["type"] = "hidden";
            hidden.Attributes["name"] = "id";
            hidden.Attributes["value"] = id?.ToString();

            // Submit button
            var button = new TagBuilder("button");
            button.Attributes["type"] = "submit";
            button.AddCssClass($"btn {btnClass}");
            button.InnerHtml.Append("Xóa vĩnh viễn");

            // Gộp các phần tử vào form
            using var writer = new System.IO.StringWriter();
            antiForgery.WriteTo(writer, HtmlEncoder.Default);
            hidden.WriteTo(writer, HtmlEncoder.Default);
            button.WriteTo(writer, HtmlEncoder.Default);
            form.InnerHtml.AppendHtml(writer.ToString());

            return form;
        }

        public static IHtmlContent DeleteButton(this IHtmlHelper htmlHelper, object id, string url, string btnClass = "btn-danger btn-sm")
        {
            // Form tag
            var form = new TagBuilder("form");
            form.Attributes["action"] = url;
            form.Attributes["method"] = "post";
            form.Attributes["onsubmit"] = "return confirm('Bạn có chắc chắn muốn xóa?');";
            form.AddCssClass("d-inline");

            // Anti-forgery token
            var antiForgery = htmlHelper.AntiForgeryToken();

            // Hidden ID input
            var hidden = new TagBuilder("input");
            hidden.Attributes["type"] = "hidden";
            hidden.Attributes["name"] = "id";
            hidden.Attributes["value"] = id?.ToString();

            // Submit button
            var button = new TagBuilder("button");
            button.Attributes["type"] = "submit";
            button.AddCssClass($"btn {btnClass}");
            button.InnerHtml.Append("Xóa");

            // Gộp các phần tử vào form
            using var writer = new System.IO.StringWriter();
            antiForgery.WriteTo(writer, HtmlEncoder.Default);
            hidden.WriteTo(writer, HtmlEncoder.Default);
            button.WriteTo(writer, HtmlEncoder.Default);
            form.InnerHtml.AppendHtml(writer.ToString());

            return form;
        }
        public static string BuildDeleteFormHtml(int id, string actionUrl, string token, string buttonClass = "btn btn-sm btn-danger", string confirmMessage = "Bạn có chắc muốn xóa?")
        {
            var form = new TagBuilder("form");
            return $@"
            <form action='{actionUrl}' method='post' style='display:inline;' onsubmit='return confirm(""{confirmMessage}"")'>
                <input type='hidden' name='id' value='{id}' />
                <input type='hidden' name='__RequestVerificationToken' value='{token}' />
                <button type='submit' class='{buttonClass}'>Xóa</button>
            </form>";
        }
        public static string BuildEditFormHtml(int id, string actionUrl, string buttonClass = "btn btn-primary")
        {
            return $@"
            <a data-id='{id}' data-url='{actionUrl}' class='btn btn-primary EditModal'>Sửa</a>";
        }
        public static IHtmlContent EditButton(this IHtmlHelper htmlHelper, string controller, object id, string action = "Edit", string btnClass = "btn btn-sm btn-edit", string text = "Sửa")
        {
            var urlHelperFactory = htmlHelper.ViewContext.HttpContext.RequestServices.GetRequiredService<IUrlHelperFactory>();
            var urlHelper = urlHelperFactory.GetUrlHelper(htmlHelper.ViewContext);

            var href = urlHelper.Action(action, controller, new { id });

            var anchor = new TagBuilder("a");
            anchor.Attributes["href"] = href;
            anchor.AddCssClass($"btn {btnClass}");
            anchor.InnerHtml.Append(text);

            return anchor;
        }

    }
}
