﻿using NewManagementSystem.Models;

namespace NewsManagementSystem.WebMVC.ViewModels
{
    public class NewsArticleEditViewModel
    {
        public string NewsArticleId { get; set; }

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
    }
}
