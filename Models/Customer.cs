//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TravelExperts_Wrkshp_5.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Customer
    {
        public int CustomerId { get; set; }

        [Display(Name = "First Name:")]
        [Required(ErrorMessage = "First name is required")]
        [StringLength(25)]
        public string CustFirstName { get; set; }

        [Display(Name = "Last Name:")]
        [Required(ErrorMessage = "Last name is required")]
        [StringLength(25)]
        public string CustLastName { get; set; }

        [Display(Name = "Home Address:")]
        [Required(ErrorMessage = "Address is required")]
        [StringLength(75)]
        public string CustAddress { get; set; }

        [Display(Name = "City:")]
        [Required(ErrorMessage = "Your current city is required")]
        [StringLength(50)]
        public string CustCity { get; set; }

        [Display(Name = "Province:")]
        [Required(ErrorMessage = "Your current province is required")]
        [StringLength(2)]
        public string CustProv { get; set; }

        [Display(Name = "Postal Code:")]
        [Required(ErrorMessage = "Postal code is required")]
        //[DataType(DataType.PostalCode)]
        //[RegularExpression(@"/[A-Za-z]\d[A-Za-z] ?\d[A-Za-z]\d/", ErrorMessage = "Invalid Postal Code")]
        [StringLength(7)]
        public string CustPostal { get; set; }

        [Display(Name = "Country:")]
        [Required(ErrorMessage = "Your current country is required")]
        [StringLength(25)]
        public string CustCountry { get; set; }

        [Display(Name = "Home Phone:")]
        //[Required(ErrorMessage = "Home Phone number is required")]
        [StringLength(20)]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number eg 709-XXX-XXXX")]
        public string CustHomePhone { get; set; }

        [Display(Name = "Business Phone")]
        //[Required(ErrorMessage = "Business phone number is required")]
        [StringLength(20)]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number eg 709-XXX-XXXX")]
        public string CustBusPhone { get; set; }

        [Display(Name = "Email:")]
        [Required(ErrorMessage = "Your Email is required")]
        [StringLength(50)]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string CustEmail { get; set; }

        [Display(Name = "Agent Number:")]
        //[Required(ErrorMessage = "Your Agent Code is required")]
        public Nullable<int> AgentId { get; set; }

        [Display(Name = "Username:")]
        [Required(ErrorMessage = "Your username is required")]
        [StringLength(50)]
        public string CustUsername { get; set; }

        [Display(Name = "Password:")]
        [Required(ErrorMessage = "Your Password is required")]
        [DataType(DataType.Password)]
        public string CustPassword { get; set; }
    }
}
