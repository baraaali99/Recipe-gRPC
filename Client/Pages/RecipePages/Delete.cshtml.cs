using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Client.Protos;
using Grpc.Core;

namespace Client.Pages.RecipePages
{
    public class DeleteModel : PageModel
    {
		[TempData]
		public string? ActionResult { get; set; }
		[BindProperty(SupportsGet = true)]
		public Guid RecipeId { get; set; } = Guid.Empty;
		public Recipe Recipe { get; set; } = new();
		private readonly RecipeService.RecipeServiceClient _recipeServiceClient;

		public DeleteModel(RecipeService.RecipeServiceClient recipeServiceClient) =>
				_recipeServiceClient = recipeServiceClient;

		public async Task<IActionResult> OnGet()
		{
			try
			{
				Recipe.Id = RecipeId.ToString();
				var response = await _recipeServiceClient.ReadRecipeAsync(Recipe);
				if (response == null)
					return NotFound();
				Recipe = response;
				return Page();
			}
			catch (RpcException ex)
			{
				ActionResult = ex.Status.Detail;
				return RedirectToPage("./List");
			}
			catch (Exception)
			{
				ActionResult = "Something went wrong please try again later";
				return RedirectToPage("./List");
			}
		}

		public async Task<IActionResult> OnPostAsync()
		{
			try
			{
				Recipe.Id = RecipeId.ToString();
				var response = await _recipeServiceClient.DeleteRecipeAsync(Recipe);
				ActionResult = "Successfully Deleted";
			}
			catch (RpcException ex)
			{
				ActionResult = ex.Status.Detail;
				return RedirectToPage("./List");
			}
			catch (Exception)
			{
				ActionResult = "Something went wrong please try again later";
				return RedirectToPage("./List");
			}
			return RedirectToPage("./List");
		}
	}
}
