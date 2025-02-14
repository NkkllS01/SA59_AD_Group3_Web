using SingNature.Data;
using SingNature.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace SingNature.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryDAO _categoryDAO;

        public CategoryController()
        {
            _categoryDAO = new CategoryDAO();
        }

        [HttpGet]
        public ActionResult<List<Category>> GetCategories()
        {
            var categories = _categoryDAO.GetAllCategories();
            if (categories == null || categories.Count == 0)
            {
                return NotFound(new { message = "No categories found" });
            }
            return Ok(categories);
        }
    }
}
