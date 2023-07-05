using System;
namespace DriveNow.Context
{
	public class User
	{
		public Guid UserId { get; set; }

		public string FirstName { get; set; }

		public string SecondName { get; set; }

		public string? Number { get; set; }

		public string? Email { get; set; }

		public string Password { get; set; }

		public DateTime? Birthday { get; set; }

		public SexEnum? Sex { get; set; }

		public LanguageEnum? Language { get; set; }
	}

	public enum SexEnum {
		Male,
		Female
	}

	public enum LanguageEnum {
		Ukrainian,
		English
	}
}