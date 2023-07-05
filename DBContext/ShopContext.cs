using System;
using DriveNow.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace DriveNow.DBContext
{
	public class ShopContext: DbContext
	{
		public ShopContext(DbContextOptions<ShopContext> options) : base(options)
		{

		}

		public DbSet<User> users { get; set; }
	}
}

