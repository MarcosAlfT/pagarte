﻿namespace IdentityService.Application.Dtos.Auth
{
	public class UserExistenceDto
	{
		public bool ExistUsername { get; set; }
		public bool ExistEmail { get; set; }
	}
}
