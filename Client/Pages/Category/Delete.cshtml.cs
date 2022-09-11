using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Client.Protos;

namespace Client.Pages.Category
{
    public class DeleteModel : PageModel
    {
		[TempData]
		public string? ActionResult { get; set; }
		[FromRoute(Name = "Category")]
		public string Category { get; set; } = string.Empty;
		private readonly CategoryService.CategoryServiceClient _categoryServiceClient;

		public DeleteModel(CategoryService.CategoryServiceClient categoryServiceClient) =>
				_categoryServiceClient = categoryServiceClient;

		public void OnGet()
		{
		}

		public async Task<IActionResult> OnPostAsync()
		{
			try
			{
				var response = await _categoryServiceClient.DeleteCategoryAsync(new() { NewTitle = Category });
				ActionResult = "Created successfully";
			}
			catch (Exception)
			{
				ActionResult = "Something went wrong, Try again later";
			} 
			return RedirectToPage("./List");
		}
	}
}
