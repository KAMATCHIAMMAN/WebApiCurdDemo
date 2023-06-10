using CrudOperationsDemo.Models;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Polly;

namespace CrudOperationsDemo.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BrandController : ControllerBase
	{
		private readonly BrandContext context;
		public BrandController(BrandContext dbContext)
		{
			context = dbContext;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Brand>>> GetBrands()
		{
			if (context.Brands == null)
			{
				return NotFound();
			}
			return await context.Brands.ToListAsync();
		}
		[HttpGet("{id}")]
		public async Task<ActionResult<Brand>> GetBrands(int id)
		{
			if (context.Brands == null)
			{
				return NotFound();
			}
			var brand = await context.Brands.FindAsync(id);
			if (brand == null)
			{
				return NotFound();
			}
			return brand;
		}
		[HttpPost]
		public async Task<ActionResult<Brand>>PostBrand(Brand brand)
		{
			context.Brands.Add(brand);
			await context.SaveChangesAsync();
			return CreatedAtAction(nameof(GetBrands), new { id = brand.ID }, brand);
		}

		[HttpPut]
		public async Task<IActionResult>PutBrand(int id,Brand brand)
		{
			if(id!=brand.ID)
			{
				return BadRequest();
			}
			context.Entry(brand).State = EntityState.Modified;
			try
			{
				await context.SaveChangesAsync();
			}
			catch(DbUpdateConcurrencyException)
			{
				if (BrandAvailabe(id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}
			return Ok();
		}
		private bool BrandAvailabe(int id)
		{
			return (context.Brands?.Any(x => x.ID == id)).GetValueOrDefault();
		}

		[HttpDelete]
		public async Task<IActionResult>DeleteBrand(int id)
		{
			if (context.Brands == null)
			{
				return NotFound();
			}
			var brand =  await context.Brands.FindAsync(id);
			if(brand==null)
			{
				return NotFound();
			}
			context.Brands.Remove(brand);
			await context.SaveChangesAsync();
			return Ok();
		}
	}
}
