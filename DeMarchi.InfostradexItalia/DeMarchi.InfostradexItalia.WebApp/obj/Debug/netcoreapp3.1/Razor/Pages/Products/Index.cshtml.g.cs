#pragma checksum "C:\Filippo\PW_secondoanno\PW-Info-Strade-X-l-Italia\DeMarchi.InfostradexItalia\DeMarchi.InfostradexItalia.WebApp\Pages\Products\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "cd41519f86f180a73353dd8bc9519de9e3ba3464"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(DeMarchi.InfostradexItalia.WebApp.Pages.Products.Pages_Products_Index), @"mvc.1.0.razor-page", @"/Pages/Products/Index.cshtml")]
namespace DeMarchi.InfostradexItalia.WebApp.Pages.Products
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Filippo\PW_secondoanno\PW-Info-Strade-X-l-Italia\DeMarchi.InfostradexItalia\DeMarchi.InfostradexItalia.WebApp\Pages\_ViewImports.cshtml"
using DeMarchi.InfostradexItalia.WebApp;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"cd41519f86f180a73353dd8bc9519de9e3ba3464", @"/Pages/Products/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"a2dbc9cc576bb6e04d48207bb3f804b530723545", @"/Pages/_ViewImports.cshtml")]
    public class Pages_Products_Index : global::Microsoft.AspNetCore.Mvc.RazorPages.Page
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 3 "C:\Filippo\PW_secondoanno\PW-Info-Strade-X-l-Italia\DeMarchi.InfostradexItalia\DeMarchi.InfostradexItalia.WebApp\Pages\Products\Index.cshtml"
  
    ViewData["Title"] = "Index";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<h1>Index</h1>\r\n\r\n");
#nullable restore
#line 9 "C:\Filippo\PW_secondoanno\PW-Info-Strade-X-l-Italia\DeMarchi.InfostradexItalia\DeMarchi.InfostradexItalia.WebApp\Pages\Products\Index.cshtml"
 foreach(var t in Model.Traffic)
{

#line default
#line hidden
#nullable disable
            WriteLiteral("    <div>");
#nullable restore
#line 11 "C:\Filippo\PW_secondoanno\PW-Info-Strade-X-l-Italia\DeMarchi.InfostradexItalia\DeMarchi.InfostradexItalia.WebApp\Pages\Products\Index.cshtml"
    Write(t.Description);

#line default
#line hidden
#nullable disable
            WriteLiteral("</div>\r\n    <hr />\r\n");
#nullable restore
#line 13 "C:\Filippo\PW_secondoanno\PW-Info-Strade-X-l-Italia\DeMarchi.InfostradexItalia\DeMarchi.InfostradexItalia.WebApp\Pages\Products\Index.cshtml"
}

#line default
#line hidden
#nullable disable
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<DeMarchi.InfostradexItalia.WebApp.Pages.Products.IndexModel> Html { get; private set; }
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<DeMarchi.InfostradexItalia.WebApp.Pages.Products.IndexModel> ViewData => (global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<DeMarchi.InfostradexItalia.WebApp.Pages.Products.IndexModel>)PageContext?.ViewData;
        public DeMarchi.InfostradexItalia.WebApp.Pages.Products.IndexModel Model => ViewData.Model;
    }
}
#pragma warning restore 1591
