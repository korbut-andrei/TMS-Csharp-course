using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Lesson_12_Web_View.Models;
using Lesson_12_Web_View.Services;
using Lesson_12_Web_View.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection;

namespace Lesson_12_Web_View.Controllers;

//[Route("[controller]/[action]")]
//[ApiController]
public class WarehouseController : Controller
{
    private readonly ItemService itemService;
    private readonly WarehouseService warehouseService;

    public WarehouseController(ItemService itemService, WarehouseService warehouseService)
    {
        this.itemService = itemService;
        this.warehouseService = warehouseService;
    }

    [HttpGet]
    public IActionResult AddWarehouse()
    {
        return View();
    }

    [HttpPost]
    public IActionResult AddWarehouse([Bind("Name")] AddWarehouse addWarehouse)
    {
        if (ModelState.IsValid)
        {
            var result = warehouseService.AddWarehouse(new AddWarehouse { Name = addWarehouse.Name });

            if (result.Success)
            {
                return RedirectToAction("GetWarehouses"); // Redirect to the warehouse list or another action
            }

            ModelState.AddModelError(string.Empty, result.Message);
        }

        return View(addWarehouse);
    }

    [HttpGet]
    public IActionResult GetWarehouses()
    {
        var result = warehouseService.GetWarehouses();

        var viewModel = new WarehouseList
        {
            Warehouses = result.Warehouses,
            Count = result.Count
        };

        return View(viewModel);
    }


    [HttpGet]
    public IActionResult GetWarehouseActions()
    {
        var actionNames = GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance)
                                    .Where(m => m.IsDefined(typeof(HttpGetAttribute)) || m.IsDefined(typeof(HttpPostAttribute)))
                                    .Select(m => m.Name)
                                    .ToList();

        return View(actionNames);
    }



    [HttpGet]
    public IActionResult AddItemToWarehouseByIdAndQuantity()
    {
        var warehouses = warehouseService.Warehouses;
        var items = itemService.Items;

        var viewModel = new CombinedItemModel
        {
            WarehouseList = GetWarehouseSelectList(),
            ItemList = GetItemSelectList(),
            WarehouseModel = new Warehouse(),
        };

        return View(viewModel);
    }

    [HttpPost]
    public IActionResult AddItemToWarehouseByIdAndQuantity(CombinedItemModel viewModel)
    {
        var result = warehouseService.AddItemToWarehouseByIdAndQuantity(
            viewModel.ItemModel.ItemId,
            viewModel.WarehouseModel.WarehouseId,
            viewModel.Quantity
        );

        if (result.Success)
        {
            return Ok(result);
        }
        else
        {
            // Handle errors appropriately
            return BadRequest(result);
        }
    }

    private List<SelectListItem> GetWarehouseSelectList()
    {
        // Retrieve the list of warehouses from the service
        var warehouses = warehouseService.GetWarehouses().Warehouses;

        // Create a list of SelectListItem from the warehouses
        var selectList = warehouses.Select(warehouse => new SelectListItem
        {
            Value = warehouse.WarehouseId.ToString(),
            Text = warehouse.Name
        }).ToList();

        return selectList;
    }

    private List<SelectListItem> GetItemSelectList()
    {
        // Retrieve the list of items from the service
        var items = itemService.GetItems().Items;

        // Create a list of SelectListItem from the items
        var selectList = items.Select(item => new SelectListItem
        {
            Value = item.ItemId.ToString(),
            Text = $"{item.Name} ({item.Color})"
        }).ToList();

        return selectList;
    }

    [HttpGet]
    public IActionResult WarehouseItems()
    {
        return View();
    }

    [HttpGet]
    public IActionResult RemoveItemFromWarehouseByIdAndQuantity()
    {
        var viewModel = new RemoveItemModel
        {
            WarehouseList = GetWarehouseSelectList(),
            ItemList = GetItemSelectList()
        };

        return View(viewModel);
    }

    [HttpPost]
    public  IActionResult RemoveItemFromWarehouseByIdAndQuantity(RemoveItemModel model)
    {

        var result = warehouseService.RemoveItemFromWarehouseByIdAndQuantity(model.SelectedItemId, model.SelectedWarehouseId, model.Quantity);

        if (result.Success)
        {
            return Ok(result);
        }
        else
        {
            return BadRequest(result);
        }
    }

    [HttpGet]
    public IActionResult WarehouseItemsByID()
    {
        var viewModel = new WarehouseItemsViewModel
        {
            WarehouseList = GetWarehouseSelectList()
        };

        return View(viewModel);
    }

    [HttpPost]
    public IActionResult WarehouseItemsByID(int selectedWarehouseId)
    {
        var items = warehouseService.GetItemsFromWarehouse(selectedWarehouseId);

        if (items != null)
        {
            var viewModel = new WarehouseItemsViewModel
            {
                WarehouseList = GetWarehouseSelectList(),
                SelectedWarehouseId = selectedWarehouseId,
                ItemsStored = items
            };

            return View(viewModel);
        }
        else
        {
            ModelState.AddModelError("WarehouseId", "Warehouse items not found.");
            return View(new WarehouseItemsViewModel
            {
                WarehouseList = GetWarehouseSelectList()
            });
        }
    }
}
