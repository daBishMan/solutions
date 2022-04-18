using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Solutions.Dotnet.API.DTO;
using Solutions.Dotnet.Core.Entities;
using Solutions.Dotnet.Core.Interfaces;
using Solutions.Dotnet.Core.Specifications;

namespace Solutions.Dotnet.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
  private readonly IGenericRepository<Product> productsRepo;
  private readonly IGenericRepository<ProductBrand> productBrandRepo;
  private readonly IGenericRepository<ProductType> productTypeRepo;
  private readonly IMapper mapper;

  public ProductsController(
    IGenericRepository<Product> productsRepo,
    IGenericRepository<ProductBrand> productBrandRepo,
    IGenericRepository<ProductType> productTypeRepo,
    IMapper mapper)
  {
    this.productsRepo = productsRepo;
    this.productBrandRepo = productBrandRepo;
    this.productTypeRepo = productTypeRepo;
    this.mapper = mapper;
  }

  [HttpGet]
  public async Task<ActionResult<IReadOnlyList<ProductToReturnDTO>>> GetProducts()
  {
    var spec = new ProductsWithTypesAndBrandsSpecification();
    var products = await this.productsRepo.ListAsync(spec);

    return Ok(this.mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDTO>>(products));

    // return products.Select(product => new ProductToReturnDTO
    // {
    //   Id = product.Id,
    //   Name = product.Name,
    //   Description = product.Description,
    //   Price = product.Price,
    //   PictureUrl = product.PictureUrl,
    //   ProductType = product.ProductType.Name,
    //   ProductBrand = product.ProductBrand.Name
    // }).ToList();
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<ProductToReturnDTO>> GetProduct(int id)
  {
    var spec = new ProductsWithTypesAndBrandsSpecification(id);
    var product = await this.productsRepo.GetEntityWithSpec(spec);

    return this.mapper.Map<Product, ProductToReturnDTO>(product);

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
