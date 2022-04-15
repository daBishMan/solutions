using Microsoft.AspNetCore.Mvc;
using Solutions.Dotnet.Core.Entities;
using Solutions.Dotnet.Core.Interfaces;

namespace Solutions.Dotnet.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
  private readonly IGenericRepository<Product> productsRepo;
  private readonly IGenericRepository<ProductBrand> productBrandRepo;
  private readonly IGenericRepository<ProductType> productTypeRepo;

  public ProductsController(
    IGenericRepository<Product> productsRepo,
    IGenericRepository<ProductBrand> productBrandRepo,
    IGenericRepository<ProductType> productTypeRepo)
  {
    this.productsRepo = productsRepo;
    this.productBrandRepo = productBrandRepo;
    this.productTypeRepo = productTypeRepo;
  }

  [HttpGet]
  public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts()
  {
    var products = await this.productsRepo.ListAllAsync();
    return Ok(products);
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<Product>> GetProduct(int id)
  {
    return await this.productsRepo.GetByIdAsync(id);
  }

  [HttpGet("brands")]
  public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
  {
    return Ok(await this.productBrandRepo.ListAllAsync());
  }

  [HttpGet("types")]
  public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
  {
    return Ok(await this.productTypeRepo.ListAllAsync());
  }
}
