using Catalog.API.Interfaces.Manager;
using Catalog.API.Models;
using CoreApiResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.Collections;
using System.Net;

namespace Catalog.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CatalogController : BaseController
    {
        private IProductManager _productManager;
        public CatalogController(IProductManager productManager)
        {
            _productManager= productManager;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public IActionResult GetProducts() 
        {
            try
            {
                var products = _productManager.GetAll();
                return CustomResult("Data loaded successfully.", products);
            }
            catch(Exception ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.BadRequest);
            }
        }


        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public IActionResult GetByCategory(string category)
        {
            try
            {
                var products = _productManager.GetByCategory(category);
                return CustomResult("Data loaded successfully.", products);
            }
            catch (Exception ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.BadRequest);
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public IActionResult GetByID(string id)
        {
            try
            {
                var product = _productManager.GetById(id);
                return CustomResult("Data loaded successfully.", product);
            }
            catch (Exception ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.BadRequest);
            }
        }


        [HttpPost]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.Created)]
        public IActionResult CreateProduct([FromBody]Product product)
        {
            try
            {
                product.Id = ObjectId.GenerateNewId().ToString();
                bool isSaved = _productManager.Add(product);
                if(isSaved)
                {
                    return CustomResult("Product saved successfully.", product, HttpStatusCode.Created);
                }
                return CustomResult("Product failed to saved.", product, HttpStatusCode.BadGateway);
            }
            catch (Exception ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.BadRequest);
            }
        }


        [HttpPut]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public IActionResult UpdateProduct([FromBody] Product product)
        {
            try
            {
                if (string.IsNullOrEmpty(product.Id))
                {
                    return CustomResult("No data found.", HttpStatusCode.NotFound);
                }
                bool isUpdated = _productManager.Update(product.Id,product);
                if (isUpdated)
                {
                    return CustomResult("Product updated successfully.", product, HttpStatusCode.OK);
                }
                return CustomResult("Product failed to update.", product, HttpStatusCode.BadGateway);
            }
            catch (Exception ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.BadRequest);
            }
        }


        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public IActionResult DeleteProduct( string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return CustomResult("No data found.", HttpStatusCode.NotFound);
                }
                bool isDeleted = _productManager.Delete(id);
                if (isDeleted)
                {
                    return CustomResult("Product deleted successfully.", HttpStatusCode.OK);
                }
                return CustomResult("Product failed to delete.", HttpStatusCode.BadGateway);
            }
            catch (Exception ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.BadRequest);
            }
        }

    }
}
