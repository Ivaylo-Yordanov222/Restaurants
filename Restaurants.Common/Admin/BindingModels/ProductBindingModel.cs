using Microsoft.AspNetCore.Http;
using Restaurants.Common.Admin.ViewModels;
using Restaurants.Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurants.Common.Admin.BindingModels
{
    public class ProductBindingModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = BussinessLogicConstants.RequiredField)]
        [Display(Name = BussinessLogicConstants.NameDisplayMessage)]
        [StringLength(60, MinimumLength = 4, ErrorMessage = BussinessLogicConstants.StringLengthOfProduct)]
        [RegularExpression(BussinessLogicConstants.ProductAndDescRegexString, ErrorMessage = BussinessLogicConstants.ProductNameValidationRegex)]
        public string Name { get; set; }

        [Required(ErrorMessage = BussinessLogicConstants.RequiredField)]
        [Display(Name = BussinessLogicConstants.SlugDisplayMessage)]
        [RegularExpression(BussinessLogicConstants.SlugRegexString, ErrorMessage = BussinessLogicConstants.SlugValidationRegex)]
        public string Slug { get; set; }

        [Required(ErrorMessage = BussinessLogicConstants.RequiredField)]
        [Display(Name = BussinessLogicConstants.PictureDisplayMessage)]
        public IFormFile ImageUrl { get; set; }

        public string OldPictureUrl { get; set; }

        public string OldTubmPictureUrl { get; set; }

        [Required(ErrorMessage = BussinessLogicConstants.RequiredField)]
        [Display(Name = BussinessLogicConstants.DescriptionDisplayMessage)]
        [StringLength(500, MinimumLength = 3, ErrorMessage = BussinessLogicConstants.StringLengthOfDescription)]
        [RegularExpression(BussinessLogicConstants.ProductAndDescRegexString, ErrorMessage = BussinessLogicConstants.DescriptionValidationRegex)]
        public string Description { get; set; }

        [Required(ErrorMessage = BussinessLogicConstants.RequiredField)]
        [Display(Name = BussinessLogicConstants.CategoryDisplayMessage)]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = BussinessLogicConstants.RequiredCustomDecimal)]
        [Display(Name = BussinessLogicConstants.PriceDisplayMessage)]
        public decimal Price { get; set; }

        //[Required(ErrorMessage = BussinessLogicConstants.RequiredCustomDecimal)]
        [Required]
        [Display(Name = BussinessLogicConstants.PromotionalPriceDisplayMessage)]
        public decimal PromotionalPrice { get; set; }

        public IEnumerable<CategoryConciseViewModel> Categories { get; set; }
    }
}
