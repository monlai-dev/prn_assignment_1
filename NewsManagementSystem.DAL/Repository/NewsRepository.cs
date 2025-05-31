using Microsoft.EntityFrameworkCore;
using NewManagementSystem.Models;
using NewsManagementSystem.DataAccess.Repository.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsManagementSystem.DataAccess.Repository
{
	public class NewsRepository : INewsRepository
	{
		private readonly FunewsManagementContext _context;
		public NewsRepository(FunewsManagementContext context)
		{
			_context = context;
		}

		public List<NewsArticle> GetAllNewsWithDetails()
		{
			return _context.NewsArticles
				.Include(n => n.Category)
				.Include(n => n.Tags)         
				.Include(n => n.CreatedBy)
				.OrderByDescending(n => n.CreatedDate)
				.ToList();
		}
	}
}
