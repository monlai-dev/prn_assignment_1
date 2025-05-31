using NewsManagementSystem.BusinessObject.ModelsDTO;

namespace NewsManagementSystem.WebMVC.ViewModels
{
	public class LoginRegisterViewModel
	{
		public LoginDTO Login { get; set; } = new LoginDTO();
		public RegisterDTO Register { get; set; } = new RegisterDTO();
	}
}
