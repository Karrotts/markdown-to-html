using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConvertMarkdown.Web.Models;
using System.Diagnostics;

namespace ConvertMarkdown.Web.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ViewResult Index()
        {
            return View();
        }

        [HttpPost]
        public ViewResult RenderResult(Models.Markdown markdown)
        {
            if (string.IsNullOrEmpty(markdown.MarkdownText)) return View("Index");

            List<string> lines = markdown.MarkdownText.Split(
                                    new[] { "\r\n", "\r", "\n" },
                                    StringSplitOptions.None
                                    ).ToList<string>();
            markdown.HTMLText = Markdown.Convert(lines);
            return View("Rendered", markdown);
        }

    }
}
