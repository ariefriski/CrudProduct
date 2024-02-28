using CrudProducts.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CrudProducts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductContext _productContext;

        public ProductController(ProductContext productContext)
        {
            _productContext = productContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            if(_productContext.Products == null) 
            {
                Console.WriteLine("Data tidak ditemukan");
                return NotFound();
            }
            var list_data =  await _productContext.Products.ToListAsync();
            foreach (var data in list_data)
            {
                Console.WriteLine("Id : " + data.Id);
                Console.WriteLine("Name : " + data.Name);
                Console.WriteLine("Price : " + data.Price);
                Console.WriteLine("=====");
            }
            return list_data;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            if (_productContext.Products == null)
            {
                Console.WriteLine("Data tidak ditemukan");
                return NotFound();
            }
            var data = await _productContext.Products.FindAsync(id);
            if(data is null)
            {
                Console.WriteLine("Data tidak ditemukan");
                return NotFound();
            }
            Console.WriteLine("Id : " + data.Id);
            Console.WriteLine("Name : " + data.Name);
            Console.WriteLine("Price : " + data.Price);
            return data;
        }

        [HttpPost]

        public async Task<ActionResult<Product>> CreateProduct(Product data)
        {
            _productContext.Products.Add(data);
            await _productContext.SaveChangesAsync();
            Console.WriteLine("Sukses Tambah Data !");
            return CreatedAtAction(nameof(GetProduct), new { id = data.Id }, data);
        }
    }
}
