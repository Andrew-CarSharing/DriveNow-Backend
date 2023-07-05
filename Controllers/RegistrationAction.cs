using System;
using System.Security.Cryptography;
using System.Text;
using DriveNow.Context;
using DriveNow.DBContext;
using DriveNow.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DriveNow.Controllers
{
	[ApiController]
	[Route("RegistrationAction")]
	public class RegistrationAction : ControllerBase
	{

		public ShopContext _context;

		public RegistrationAction(ShopContext context)
		{
			_context = context;
		}

		[HttpPost("Registration")]
		public async Task<IActionResult> Registration(RegistrationModel registrationModel) {

			if (registrationModel.Number != null) {

				var user = await _context.users.FirstOrDefaultAsync(x=>x.Number == registrationModel.Number);

				var sha = SHA256.Create();

				var asByteArray = Encoding.Default.GetBytes(registrationModel.Password);

                var hashedPassword = Convert.ToBase64String(sha.ComputeHash(asByteArray));

                _context.users.Add(new User
			{
				UserId = Guid.NewGuid(),
				FirstName = registrationModel.FirstName,
				SecondName = registrationModel.SecondName,
				Number = registrationModel.Number,
				Password = hashedPassword
			});

				await _context.SaveChangesAsync();

				return Ok("Successful");
			}

			else if (registrationModel.Email != null) {

				var user = await _context.users.FirstOrDefaultAsync(x => x.Email == registrationModel.Email);

				var sha = SHA256.Create();

				var asByteArray = Encoding.Default.GetBytes(registrationModel.Password);

				var hashedPassword = Convert.ToBase64String(sha.ComputeHash(asByteArray));

				_context.users.Add(new User
                {
					UserId = Guid.NewGuid(),
                    FirstName = registrationModel.FirstName,
                    SecondName = registrationModel.SecondName,
                    Email = registrationModel.Email,
                    Password = hashedPassword
                });

				await _context.SaveChangesAsync();

				return Ok("Successful!");
            }
			return Ok("Finish");
        }
	}			
}