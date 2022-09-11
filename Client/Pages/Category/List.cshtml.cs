using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Client.Protos;
namespace Client.Pages.Category
{
    public class ListModel : PageModel
    {
		[TempData]
		public string? ActionResult { get; set; }
		private readonly CategoryService.CategoryServiceClient _categoryServiceClient;

		public ListModel(CategoryService.CategoryServiceClient categoryServiceClient) =>
				_categoryServiceClient = categoryServiceClient;

		public List<string> CategoryList { get; set; } = new();

		public async Task<IActionResult> OnGetAsync()
		{
			try
			{
				var response = await _categoryServiceClient.ListCategoriesAsync(new());
				List<string> categories = response.Categories.ToList();
				if (categories != null)
					CategoryList = categories;
				return Page();
			}
			catch (Exception)
			{
				ActionResult = "Something went wrong, Try again later";
				return RedirectToPage("./List");
			}
		}
	}
}
