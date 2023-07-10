using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using DriveNow.Context;
using DriveNow.DBContext;
using DriveNow.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace DriveNow.Controllers
{
	[ApiController]
	[Route("SingIn")]
	public class SingInAction: ControllerBase
	{
		public ShopContext _context;

		private readonly IOptions<AuthOptions> options;
		
		public SingInAction(ShopContext context, IOptions<AuthOptions> options)
		{
			_context = context;
			this.options = options;
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
						var token = GenerateToken(user);

						return Ok(new {
							access_token = token
						}) ;
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

                        var token = GenerateToken(user);

                        return Ok(new
                        {
                            access_token = token
                        });
                    }
				}
				else {
					return BadRequest("Bad!");
				}
			}
			return Ok("Finished!"); 
		}
		[HttpPost("Token")]
		public string GenerateToken(User user) {

			var authParams = options.Value;

			var securityKey = authParams.GetSymmetricSecurityKey();

			var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

			var claims = new List<Claim>()
			{
				new Claim (JwtRegisteredClaimNames.Name, user.SecondName),
				new Claim (JwtRegisteredClaimNames.Sub, user.UserId.ToString())
			};

			var token = new JwtSecurityToken(authParams.Issuer,
				authParams.Audience,
				claims,
                expires: DateTime.Now.AddSeconds(authParams.TokenLifeTime),
                signingCredentials: credentials);

			return new JwtSecurityTokenHandler().WriteToken(token);
        }
	}
}
