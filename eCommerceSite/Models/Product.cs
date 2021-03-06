﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerceSite.Models
{
    /// <summary>
    /// A salable product
    /// </summary>
    public class Product
    {
        [Key]  //  Make Primary Key in database
        public int ProductId { get; set; }

        /// <summary>
        /// The consumer facing name of the product
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The retail price as US currency
        /// </summary>
        [DataType(DataType.Currency)]
        // [Display(Name = "New Name")] Renaming Price
        public double Price { get; set; }

        /// <summary>
        /// Category product falls under. ex. Electronics, Furniture, etc...
        /// </summary>
        public string Category { get; set; }
    }
}
