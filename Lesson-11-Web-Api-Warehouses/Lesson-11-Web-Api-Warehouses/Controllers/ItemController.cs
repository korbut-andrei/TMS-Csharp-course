using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Lesson_11_Web_Api_Warehouses.Models;
using Lesson_11_Web_Api_Warehouses.Services;

namespace Lesson_11_Web_Api_Warehouses.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class ItemController : ControllerBase
{
    private readonly ItemService itemService;

    public ItemController(ItemService itemService)
    {
        this.itemService = itemService;
    }

    [HttpPost]
    public IActionResult AddItem([FromBody] AddItem item)
    {
        // Create a new Item instance using the itemService and add it to the items collection
        var result = itemService.AddItem(item);

        if (result.Success)
        {
            return Ok(result);
        }
        else
        {
            return BadRequest(result);
        }
    }

    [HttpPost]
    public IActionResult GetItemData(int itemId)
    {
        // Create a new Item instance using the itemService and add it to the items collection
        var result = itemService.GetItemById(itemId);

        if (result != null)
        {
            return Ok(result);
        }
        else
        {
            return BadRequest(result);
        }
    }

    [HttpPost]
    public ItemList GetItems()
    {
        var result = itemService.GetItems();

        return result;
    }
}