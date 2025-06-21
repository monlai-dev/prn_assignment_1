using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using NewManagementSystem.Models;
using NewsManagementSystem.BusinessObject.Configuration;
using NewsManagementSystem.BusinessObject.Models;

namespace NewsManagementSystem.Web.ViewModels.NewsArticle
{
    public class NewsArticleViewModel
    {
        public string? NewsTitle { get; set; }

        public string Headline { get; set; } = null!;

        public DateTime? CreatedDate { get; set; }

        public string? NewsContent { get; set; }

        public string? NewsSource { get; set; }

        public short? CategoryId { get; set; }

        public bool? NewsStatus { get; set; } = true;

        public short? CreatedById { get; set; }

        public short? UpdatedById { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public virtual Category? Category { get; set; }

        public virtual SystemAccount? CreatedBy { get; set; }
        public List<int> SelectedTagIds { get; set; } = new();

        public List<Tag>? AvailableTags { get; set; }

        public SelectList? CategoryList { get; set; }
        public SelectList? CreatedByList { get; set; }
        public SelectList? UpdatedByList { get; set; }
    }
}
