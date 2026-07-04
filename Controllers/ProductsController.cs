using Microsoft.AspNetCore.Mvc;
using ProductManagementAPI.Models;
using ProductManagementAPI.Services;

namespace ProductManagementAPI.Controllers
{
    /// <summary>
    /// Manages product resources.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductsController(IProductService service)
        {
            _service = service;
        }

        /// <summary>
        /// Retrieves all products.
        /// </summary>
        /// <param name="cancellationToken">Propagates notification that the request has been cancelled.</param>
        /// <returns>A list of all products.</returns>
        /// <response code="200">Returns the full list of products.</response>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<Product>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var products = await _service.GetAllProducts(cancellationToken);
            return Ok(ApiResponse<IReadOnlyList<Product>>.SuccessResult(products));
        }

        /// <summary>
        /// Retrieves a single product by ID.
        /// </summary>
        /// <param name="id">The unique identifier of the product.</param>
        /// <param name="cancellationToken">Propagates notification that the request has been cancelled.</param>
        /// <returns>The product with the specified ID.</returns>
        /// <response code="200">Returns the requested product.</response>
        /// <response code="400">The supplied ID is not a positive integer.</response>
        /// <response code="404">No product found with the given ID.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<Product>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<Product>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<Product>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            if (id <= 0)
                return BadRequest(ApiResponse<Product>.FailResult("ID must be a positive integer."));

            var product = await _service.GetProductById(id, cancellationToken);

            if (product is null)
                return NotFound(ApiResponse<Product>.FailResult($"Product with ID {id} was not found."));

            return Ok(ApiResponse<Product>.SuccessResult(product));
        }

        /// <summary>
        /// Creates a new product.
        /// </summary>
        /// <param name="product">The product data to create.</param>
        /// <param name="cancellationToken">Propagates notification that the request has been cancelled.</param>
        /// <returns>The newly created product with its assigned ID.</returns>
        /// <response code="201">Product created successfully.</response>
        /// <response code="400">The request body failed model validation.</response>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<Product>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(
            [FromBody] Product product,
            CancellationToken cancellationToken)
        {
            var created = await _service.CreateProduct(product, cancellationToken);

            return CreatedAtAction(
                nameof(GetById),
                new { id = created.Id },
                ApiResponse<Product>.SuccessResult(created, "Product created successfully."));
        }

        /// <summary>
        /// Updates an existing product.
        /// </summary>
        /// <param name="id">The unique identifier of the product to update.</param>
        /// <param name="product">The updated product data.</param>
        /// <param name="cancellationToken">Propagates notification that the request has been cancelled.</param>
        /// <returns>The updated product.</returns>
        /// <response code="200">Returns the updated product.</response>
        /// <response code="400">The supplied ID is invalid or the request body failed model validation.</response>
        /// <response code="404">No product found with the given ID.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<Product>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] Product product,
            CancellationToken cancellationToken)
        {
            if (id <= 0)
                return BadRequest(ApiResponse<Product>.FailResult("ID must be a positive integer."));

            var updated = await _service.UpdateProduct(id, product, cancellationToken);

            if (updated is null)
                return NotFound(ApiResponse<Product>.FailResult($"Product with ID {id} was not found."));

            return Ok(ApiResponse<Product>.SuccessResult(updated, "Product updated successfully."));
        }

        /// <summary>
        /// Deletes a product by ID.
        /// </summary>
        /// <param name="id">The unique identifier of the product to delete.</param>
        /// <param name="cancellationToken">Propagates notification that the request has been cancelled.</param>
        /// <response code="204">Product deleted successfully.</response>
        /// <response code="400">The supplied ID is not a positive integer.</response>
        /// <response code="404">No product found with the given ID.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            if (id <= 0)
                return BadRequest(ApiResponse<object>.FailResult("ID must be a positive integer."));

            var deleted = await _service.DeleteProduct(id, cancellationToken);

            if (!deleted)
                return NotFound(ApiResponse<object>.FailResult($"Product with ID {id} was not found."));

            return NoContent();
        }

        /// <summary>
        /// Searches for products by category.
        /// </summary>
        /// <param name="category">The category name to filter by. Case-sensitive.</param>
        /// <param name="cancellationToken">Propagates notification that the request has been cancelled.</param>
        /// <returns>
        /// A list of products whose category exactly matches the supplied value.
        /// Returns an empty list when no products match — not a 404.
        /// </returns>
        /// <response code="200">Returns the matching products (may be an empty list).</response>
        /// <response code="400">The category query parameter is missing or empty.</response>
        [HttpGet("search")]
        [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<Product>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Search(
            [FromQuery] string category,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(category))
                return BadRequest(ApiResponse<object>.FailResult(
                    "The 'category' query parameter is required."));

            var products = await _service.SearchByCategory(category, cancellationToken);
            return Ok(ApiResponse<IReadOnlyList<Product>>.SuccessResult(products));
        }
    }
}