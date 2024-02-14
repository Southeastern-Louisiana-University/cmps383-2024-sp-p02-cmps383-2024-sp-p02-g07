namespace Selu383.SP24.Api.Features.Users
{
    public class UserDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string[] Roles { get; set; }

    }

    public class UserCreateDto
    {
        public string UserName { get; set; }
        public string[] Roles { get; set; }
        public string Password { get; set; }
    }

    public class UserLoginDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
