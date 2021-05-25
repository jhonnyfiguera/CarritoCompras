﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using tp_nt1.DataBase;
using tp_nt1.Models;

namespace tp_nt1.Controllers
{
    [Authorize(Roles = nameof(Rol.Cliente))]
    public class CarritosController : Controller
    {


        private readonly CarritoDbContext _context;



        public CarritosController(CarritoDbContext context)
        {
            _context = context;
        }



        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var carritoDbContext = _context.Carritos.Include(c => c.Cliente);
            return View(await carritoDbContext.ToListAsync());
        }



        [HttpGet]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carrito = await _context.Carritos
                .Include(c => c.Cliente)
                .FirstOrDefaultAsync(m => m.Id == id || m.ClienteId == id);

            if (carrito == null)
            {
                return NotFound();
            }

            var listaProductos = _context.CarritoItems.Select(c => c.Subtotal).ToList();
            var precioTotal = (decimal) 0;
            foreach (var aux in listaProductos)
            {
                precioTotal += aux;
            }

            carrito.Subtotal = precioTotal;
            await _context.SaveChangesAsync();

            return View(carrito);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carrito = await _context.Carritos.FindAsync(id);
            if (carrito == null)
            {
                return NotFound();
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Apellido", carrito.ClienteId);
            return View(carrito);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Activo,ClienteId,Subtotal")] Carrito carrito)
        {
            if (id != carrito.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(carrito);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarritoExists(carrito.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Apellido", carrito.ClienteId);
            return View(carrito);
        }



        [HttpGet]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carrito = await _context.Carritos
                .Include(c => c.Cliente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (carrito == null)
            {
                return NotFound();
            }

            return View(carrito);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var carrito = await _context.Carritos.FindAsync(id);
            _context.Carritos.Remove(carrito);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// //////////////////////////
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> VaciarCarrito(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carrito = await _context.Carritos
                .Include(c => c.Cliente)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (carrito == null)
            {
                return NotFound();
            }

            return View(carrito);
        }

        [HttpPost, ActionName("VaciarCarrito")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VaciarCarrito(Guid id)
        {

            var carrito = await _context.Carritos.FindAsync(id);
            _context.Carritos.Select(c => c.CarritosItems).ToList();
            if (carrito.CarritosItems != null)
            {
                carrito.CarritosItems.Clear();
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index), "Productos");

        }


        private bool CarritoExists(Guid id)
        {
            return _context.Carritos.Any(e => e.Id == id);
        }



        // GET: Carritos/Create
        //public IActionResult Create()
        //{
        //    ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Apellido");
        //    return View();
        //}


        //// POST: Carritos/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Activo,ClienteId,Subtotal")] Carrito carrito)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        carrito.Id = Guid.NewGuid();
        //        _context.Add(carrito);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Apellido", carrito.ClienteId);
        //    return View(carrito);
        //}
    }
}