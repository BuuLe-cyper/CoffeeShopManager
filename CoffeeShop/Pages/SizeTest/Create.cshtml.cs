﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DataAccess.DataContext;
using DataAccess.Models;
using AutoMapper;
using BussinessObjects.Services;
using System.Drawing;
using BussinessObjects.DTOs;
using CoffeeShop.Helper;

namespace CoffeeShop.Pages.SizeTest
{
    public class CreateModel : PageModel
    {
        private readonly ISizeService _sizeService;
        private readonly IMapper _mapper;

        public CreateModel(ISizeService sizeService, IMapper mapper)
        {
            _sizeService = sizeService;
            _mapper = mapper;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public DataAccess.Models.Size Size { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            ArgumentNullException.ThrowIfNull(nameof(Size.SizeName));
            Size.SizeName = Size.SizeName.Trim();
            bool isValidData = Validations.IsString(Size.SizeName);
            if (isValidData)
            {
                SizeDto sizeDto = _mapper.Map<SizeDto>(Size);
                var isAdd = await _sizeService.AddSize(sizeDto);
                if (!isAdd)
                {
                    ModelState.AddModelError(string.Empty, "Unable to create size, size name existed. Please try again.");
                    return Page();
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Size Name Must Be String. Please try again.");
                return Page();
            }
            return RedirectToPage("./Index");
        }
    }
}
