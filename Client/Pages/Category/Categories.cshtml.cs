using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Client.Protos;
using Grpc.Core;

namespace Client.Pages.Category
{
    public class CategoriesModel : PageModel
    {
		[TempData]
		public string? ActionResult { get; set; }
		[BindProperty]
		[Required]
		[Display(Name = "Category Name")]
		public string categoryAdded { get; set; } = string.Empty;
		[FromRoute(Name = "category")]
		public string categoryDeleted { get; set; } = string.Empty;
		[FromRoute(Name = "category")]
		[Display(Name = "Old Category Name")]
		public string currentCategory { get; set; } = string.Empty;
		[BindProperty]
		[Required]
		[Display(Name = "New Category Name")]
		public string categoryEdited { get; set; } = string.Empty;
		public List<string> CategoryList { get; set; } = new();
		private readonly CategoryService.CategoryServiceClient _categoryServiceClient;

		public CategoriesModel(CategoryService.CategoryServiceClient categoryServiceClient) =>
				_categoryServiceClient = categoryServiceClient;

		//Adding Category
		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
				return Page();
			try
			{
				var response = await _categoryServiceClient.CreateCategoryAsync(new()
				{
					NewTitle = categoryAdded
				});
				ActionResult = "Created successfully";
			}
			catch (Exception)
			{
				ActionResult = "Something went wrong, Try again later";
			}
			return RedirectToPage("./Categories");
		}

		//Deleting Category
		public async Task<IActionResult> OnDeleteAsync()
		{
			try
			{
				var response = await _categoryServiceClient.DeleteCategoryAsync(new() { NewTitle = categoryDeleted });
				ActionResult = "Created successfully";
			}
			catch (Exception)
			{
				ActionResult = "Something went wrong, Try again later";
			}
			return RedirectToPage("./Categories");
		}

		//Editing Category
		public async Task<IActionResult> OnPutAsync()
		{
			if (!ModelState.IsValid)
				return Page();
			try
			{
				var response = await _categoryServiceClient.UpdateCategoryAsync(new()
				{
					NewTitle = categoryEdited,
					OldTitle = currentCategory
				});
				ActionResult = "Created successfully";
			}
			catch (RpcException ex)
			{
				ActionResult = ex.Status.Detail;
			}
			catch (Exception)
			{
				ActionResult = "Something went wrong, Try again later";
			}
			return RedirectToPage("./Categories");
		}

		//List Categories
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
				return RedirectToPage("./Categories");
			}
		}
	}
}
