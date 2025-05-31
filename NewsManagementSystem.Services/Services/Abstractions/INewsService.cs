using NewManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsManagementSystem.Services.Services.Abstractions
{
	public interface INewsService
	{
		List<NewsArticle> GetAllNewsWithDetails();
	}
}
