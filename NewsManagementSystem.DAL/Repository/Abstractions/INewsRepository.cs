using NewManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsManagementSystem.DataAccess.Repository.Abstractions
{
	public interface INewsRepository
	{
		List<NewsArticle> GetAllNewsWithDetails();
	}
}
