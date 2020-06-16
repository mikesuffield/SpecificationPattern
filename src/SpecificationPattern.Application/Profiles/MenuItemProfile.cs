using AutoMapper;
using SpecificationPattern.Application.DTOs;
using SpecificationPattern.Application.ViewModels;
using SpecificationPattern.Core.Models;
using SpecificationPattern.Shared.Enums;
using SpecificationPattern.Shared.Extensions;
using System;

namespace SpecificationPattern.Application.Profiles
{
    public class MenuItemProfile : Profile
    {
        public MenuItemProfile()
        {
            // Map from Source -> Destination == CreateMap<Source, Destination>();

            CreateMap<string, MealType>().ConvertUsing(s => s.ToEnum<MealType>());
            CreateMap<string, AllergenType>().ConvertUsing(s => s.ToEnum<AllergenType>());
            CreateMap<MealType, string>().ConvertUsing(s => s.ToString());
            CreateMap<AllergenType, string>().ConvertUsing(s => s.ToString());

            CreateMap<MenuItem, MenuItemDto>();
            CreateMap<CreateMenuItemViewModel, MenuItemDto>();

            CreateMap<Allergen, AllergenDto>();
            CreateMap<string, AllergenDto>().ConvertUsing(a => new AllergenDto { Id = Guid.NewGuid(), AllergenType = a.ToEnum<AllergenType>() });
            CreateMap<AllergenDto, string>().ConvertUsing(a => a.AllergenType.DisplayName());

            CreateMap<double, string>().ConvertUsing(d => $"£{d:F}");

            CreateMap<MenuItemDto, ShowMenuItemViewModel>();
            CreateMap<MenuItemDto, MenuItem>();
            CreateMap<AllergenDto, Allergen>();
        }
    }
}
