using Atata;

/// <summary>
/// PageObjects package for Examples of Testing automation 
/// </summary>
namespace AutoPractice18.Pages
{
    using _ = SignInPage;
    [Url("#/signin")]
    public class SignInPage : Page<_> {
        
        public TextInput<_> Email { get; set; }

        public PasswordInput<_> Password { get; set; }

        public Button<UsersPage,_> SignIn { get; set; }
    }
}
