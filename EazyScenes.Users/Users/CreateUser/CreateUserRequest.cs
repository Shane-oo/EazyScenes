namespace EazyScenes.Users.Users.CreateUser;

// Always good practise to not tie your commands to you api requests
public class CreateUserRequest
{
    public string ConfirmPassword { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public string UserName { get; set; }
}
