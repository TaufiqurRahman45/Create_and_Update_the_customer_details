using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using Nop.Admin.Models.Customers;
using Nop.Core.Domain.Customers;
using Nop.Data;
using Nop.Services.Directory;
using Nop.Services.Localization;
using Nop.Web.Framework.Validators;
using System.Text.RegularExpressions;

namespace Nop.Admin.Validators.Customers
{
    public partial class CustomerValidator : BaseNopValidator<CustomerModel>
    {
        public CustomerValidator(ILocalizationService localizationService,
            IStateProvinceService stateProvinceService,
            CustomerSettings customerSettings,
            IDbContext dbContext)
        {
            //form fields
            if (customerSettings.CountryEnabled && customerSettings.CountryRequired)
            {
                RuleFor(x => x.CountryId)
                    .NotEqual(0)
                    .WithMessage(localizationService.GetResource("Account.Fields.Country.Required"));
            }
            if (customerSettings.CountryEnabled &&
                customerSettings.StateProvinceEnabled &&
                customerSettings.StateProvinceRequired)
            {
                Custom(x =>
                {
                    //does selected country have states?
                    var hasStates = stateProvinceService.GetStateProvincesByCountryId(x.CountryId).Any();
                    if (hasStates)
                    {
                        //if yes, then ensure that a state is selected
                        if (x.StateProvinceId == 0)
                        {
                            return new ValidationFailure("StateProvinceId", localizationService.GetResource("Account.Fields.StateProvince.Required"));
                        }
                    }
                    return null;
                });
            }
            if (customerSettings.CompanyRequired && customerSettings.CompanyEnabled)
                RuleFor(x => x.StateID).NotEmpty().WithMessage(localizationService.GetResource("Admin.Customers.Customers.Fields.Company.Required"));
           
            if (customerSettings.StreetAddressRequired && customerSettings.StreetAddressEnabled) 
                RuleFor(x => x.StreetAddress).NotEmpty().WithMessage(localizationService.GetResource("Admin.Customers.Customers.Fields.StreetAddress.Required"));
            if (customerSettings.StreetAddress2Required && customerSettings.StreetAddress2Enabled)
                RuleFor(x => x.StreetAddress2).NotEmpty().WithMessage(localizationService.GetResource("Admin.Customers.Customers.Fields.StreetAddress2.Required"));
            if (customerSettings.ZipPostalCodeRequired && customerSettings.ZipPostalCodeEnabled)
                RuleFor(x => x.CountryEnabled).NotEmpty().WithMessage(localizationService.GetResource("Admin.Customers.Customers.Fields.ZipPostalCode.Required"));
            if (customerSettings.CityRequired && customerSettings.CityEnabled)
                RuleFor(x => x.City).NotEmpty().WithMessage(localizationService.GetResource("Admin.Customers.Customers.Fields.City.Required"));
            if (customerSettings.PhoneRequired && customerSettings.PhoneEnabled)
                RuleFor(x => x.Phone).NotEmpty().WithMessage(localizationService.GetResource("Admin.Customers.Customers.Fields.Phone.Required"));
           

            if (customerSettings.FaxRequired && customerSettings.FaxEnabled) 
                RuleFor(x => x.Fax).NotEmpty().WithMessage(localizationService.GetResource("Admin.Customers.Customers.Fields.Fax.Required"));

            // Password Validation
            Custom(x =>
            {
                if (x.Password == null || string.IsNullOrEmpty(x.Password) || x.Password.Length < 6)
                {
                    return new ValidationFailure("Password", localizationService.GetResource("Account.Fields.Password.Required"));
                }
                return null;
            });

            // IC/Passport Validation

            Custom(x =>
                {
                    if (x.ICPassportNo == null || string.IsNullOrWhiteSpace(x.ICPassportNo))
                    {
                        return new ValidationFailure("ICPassportNo", localizationService.GetResource("Account.Fields.ICPassportNo.Required"));
                    }
                    return null;
                });

            

            // Zip Validation
            Custom(x =>
            {
                if (x.ZipPostalCode == null || string.IsNullOrWhiteSpace(x.ZipPostalCode))
                {
                    return new ValidationFailure("ZipPostalCode", localizationService.GetResource("address.fields.ZipPostalCode.required"));
                }
                return null;
            });

            // Email Validation

            Custom(x =>
            {
                if (x.Email == null || string.IsNullOrEmpty(x.Email))
                {
                    //string emailRegex = @"^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$";
                    //Regex re = new Regex(emailRegex);
                    return new ValidationFailure("Email", localizationService.GetResource("address.fields.Email.required"));
                }
                
                return null;

            });

            // First Name Validation

            Custom(x =>
            {
                if (x.FirstName == null || string.IsNullOrEmpty(x.FirstName))
                {
                    return new ValidationFailure("FirstName", localizationService.GetResource("address.fields.Firstname.required"));
                }
                
               
                return null;
            });

            // Last Name Validation

            Custom(x =>
            {
                if (x.LastName == null || string.IsNullOrEmpty(x.LastName))
                {
                    return new ValidationFailure("LastName", localizationService.GetResource("address.fields.LastName.required"));
                }
                return null;
            });

           

            // Nationality Validation

            Custom(x =>
            {
                if (x.Nationality == null || string.IsNullOrEmpty(x.Nationality))
                {
                    return new ValidationFailure("Nationality", localizationService.GetResource("account.fields.nationality.required"));
                }
                return null;
            });

            // Address Validation

            Custom(x =>
            {
                if (x.StreetAddress == null || string.IsNullOrEmpty(x.StreetAddress))
                {
                    return new ValidationFailure("StreetAddress", localizationService.GetResource("account.fields.streetaddress.required"));
                }
                return null;
            });

            // Country Validation

            Custom(x =>
            {
                if (x.CountryName == null || string.IsNullOrEmpty(x.CountryName))
                {
                    return new ValidationFailure("CountryName", localizationService.GetResource("account.fields.country.required"));
                }
                return null;
            });

            // City Validation

            Custom(x =>
            {
                if (x.City == null || string.IsNullOrEmpty(x.City))
                {
                    return new ValidationFailure("City", localizationService.GetResource("account.fields.city.required"));
                }
                return null;
            });

            // State Validation

            Custom(x =>
            {
                if (x.StateID == null|| string.IsNullOrEmpty(x.StateID))
                {
                    return new ValidationFailure("StateID", localizationService.GetResource("account.fields.stateprovince.required"));
                }
                return null;
            });

            // Bank Name Validation
            Custom(x =>
            {
                if (x.BankName == null || string.IsNullOrEmpty(x.BankName))
                {
                    return new ValidationFailure("BankName", localizationService.GetResource("account.fields.BankName.required"));
                }
                return null;
            });

            // Account Name Validation

            Custom(x =>
            {
                if (x.BankAccountName == null || string.IsNullOrEmpty(x.BankAccountName))
                {
                    return new ValidationFailure("BankAccountName", localizationService.GetResource("account.fields.BankAccountName.required"));
                }
                return null;
            });


            // Ref Name Validation

            Custom(x =>
            {
                if (x.ReferralName == null || string.IsNullOrEmpty(x.ReferralName))
                {
                    return new ValidationFailure("ReferralName", localizationService.GetResource("address.fields.ReferralName.required"));
                }
                return null;
            });

            

  
  
            SetStringPropertiesMaxLength<Customer>(dbContext);
        }
    }
}