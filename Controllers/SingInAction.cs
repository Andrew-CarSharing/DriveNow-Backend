using System;
using System.Security.Cryptography;
using System.Text;
using DriveNow.DBContext;
using DriveNow.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DriveNow.Controllers
{
	[ApiController]
	[Route("SingIn")]
	public class SingInAction: ControllerBase
	{
		public ShopContext _context;
		
		public SingInAction(ShopContext context)
		{
			_context = context;
		}

		[HttpPost("SingInUser")]
		public async Task<IActionResult> SingIn(SingInModel singInModel){

			if (singInModel.Email != null)
			{

				var user = await _context.users.FirstOrDefaultAsync(x => x.Email == singInModel.Email);

				if (user != null)
				{


					var sha = SHA256.Create();

					var asByteArray = Encoding.Default.GetBytes(singInModel.Password);

					var hashedPassword = Convert.ToBase64String(sha.ComputeHash(asByteArray));

					if (user.Email == singInModel.Email && user.Password == hashedPassword)
					{

						return Ok("Successful!");
						//var token = GenerateToken(user);
					}
				}
				else {
					return BadRequest("Bad!");
				}
			}

			else if (singInModel.Number != null)
			{

				var user = await _context.users.FirstOrDefaultAsync(x => x.Number == singInModel.Number);

				if (user != null)
				{
					var sha = SHA256.Create();

					var asByteArray = Encoding.Default.GetBytes(singInModel.Password);

					var hashedPassword = Convert.ToBase64String(sha.ComputeHash(asByteArray));

					if (user.Number == singInModel.Number && user.Password == hashedPassword)
					{

						return Ok("Successful!");
						//var token = GenerateToken(user);
					}
				}
				else {
					return BadRequest("Bad!");
				}
			}
			return Ok("Finished!");
		}
	}
}