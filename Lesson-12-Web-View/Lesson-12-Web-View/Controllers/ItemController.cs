using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Lesson_12_Web_View.Models;
using Lesson_12_Web_View.Services;
using System.Reflection;

namespace Lesson_12_Web_View.Controllers;

//[Route("[controller]/[action]")]
//[ApiController]
public class ItemController : Controller
{
    private readonly ItemService itemService;

    public ItemController(ItemService itemService)
    {
        this.itemService = itemService;
    }

    [HttpGet]
    public IActionResult AddItem()
    {
        return View("AddItem");
    }

    [HttpPost]
    public IActionResult AddItem([Bind("Name,Color,HasRecliner")] AddItem addItem)
    {
        if (ModelState.IsValid)
        {
            var result = itemService.AddItem(new AddItem
            {
                Name = addItem.Name,
                Color = addItem.Color,
                HasRecliner = addItem.HasRecliner,
            });

            if (result.Success)
            {
                return RedirectToAction("GetItems"); // Redirect to the item list or another action
            }

            ModelState.AddModelError(string.Empty, result.Message);
        }

        return View(addItem);
    }

    [HttpGet]
    public IActionResult GetItems()
    {
        var result = itemService.GetItems();

        var viewModel = new ItemList
        {
            Items = result.Items,
            Count = result.Count
        };

        return View(viewModel);
    }


    [HttpGet]
    public IActionResult GetItemActions()
    {
        var actionNames = GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance)
                                    .Where(m => m.IsDefined(typeof(HttpGetAttribute)) || m.IsDefined(typeof(HttpPostAttribute)))
                                    .Select(m => m.Name)
                                    .ToList();

        return View(actionNames);
    }



    [HttpGet]
    public IActionResult ItemDetails()
    {
        return View();
    }

    [HttpPost]
    public IActionResult ItemDetails(int itemId)
    {
        var result = itemService.GetItemById(itemId);

        if (result != null)
        {
            var viewModel = new Item
            {
                ItemId = result.ItemId,
                Name = result.Name,
                Color = result.Color,
                HasRecliner = result.HasRecliner
            };

            return View(viewModel);
        }
        else
        {
            ModelState.AddModelError("ItemId", "Item not found");
            return View();
        }
    }
}


