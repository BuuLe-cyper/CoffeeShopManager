﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DataAccess.DataContext;
using DataAccess.Models;
using AutoMapper;
using BussinessObjects.Services;
using CoffeeShop.ViewModels;

namespace CoffeeShop.Pages.SizeTest
{
    public class DeleteModel : PageModel
    {
        private readonly ISizeService _sizeService;
        private readonly IMapper _mapper;
        public DeleteModel(ISizeService sizeService, IMapper mapper)
        {
            _sizeService = sizeService;
            _mapper = mapper;
        }
        [BindProperty]
        public SizeVM Size { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var size = await _sizeService.GetSize((int)id);

            if (size == null)
            {
                return NotFound();
            }
            else
            {
                Size = _mapper.Map<SizeVM>(size);
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var size = await _sizeService.GetSize((int)id);
            if (size != null)
            {
                Size = _mapper.Map<SizeVM>(size);
                var isRemove =  await _sizeService.SoftDeleteSize(size.SizeID);
                if (!isRemove)
                {
                    ModelState.AddModelError(string.Empty, "Unable to delete size. Please try again.");
                    return Page();
                }
            }

            return RedirectToPage("./Index");
        }
    }
}
