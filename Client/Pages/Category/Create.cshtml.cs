using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Client.Protos;

namespace Client.Pages.Category
{
    public class CreateModel : PageModel
    {
		[TempData]
		public string? ActionResult { get; set; }
		[BindProperty]
		[Required]
		[Display(Name = "Category Name")]
		public string Category { get; set; } = string.Empty;
		private readonly CategoryService.CategoryServiceClient _categoryServiceClient;

		public CreateModel(CategoryService.CategoryServiceClient categoryServiceClient) =>
				_categoryServiceClient = categoryServiceClient;
		public void OnGet()
		{
		}

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
				return Page();
			try
			{
				var response = await _categoryServiceClient.CreateCategoryAsync(new()
				{
					NewTitle = Category
				});
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
