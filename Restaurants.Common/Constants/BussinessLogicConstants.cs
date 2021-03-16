namespace Restaurants.Common.Constants
{
    public static class BussinessLogicConstants
    {
        //Resource file paths
        public const string ProductsPath = @"Resources\SeedFiles\products.json";
        public const string CategoryPath = @"Resources\SeedFiles\category.json";
        //Status
        public const string sentStatus = "Sent";
        public const string InProgressStatus = "InProgress";
        public const string Delivered = "Delivered";
        //Validation and display attributes messages
        public const string ProductNameValidationRegex = "Product must have only alphabetic characters and empty spaces!";
        public const string CategoryNameValidationRegexMessage = "Category must have only alphabetic characters and empty spaces!";
        public const string UserNameValidationRegex = "Username must have only alphabetic characters and numbers!";
        public const string DescriptionValidationRegex = "Description must have only alphabetic characters and empty spaces!";
        public const string FullNameValidationRegex = "Fullname must have only alphabetic characters and empty spaces!";
        public const string SlugValidationRegex = "The slug must be only latin alphabetic characters and slashes";

        public const string StringLengthOfProduct = "The product name must have min length of 4 and max length of 60 characters!";
        public const string StringLengthOfCategory = "The category name must have min length of 4 and max length of 60 characters!";
        public const string StringLengthOfDescription = "Product description must be min 3 characters and max 500 characters of length!";
        public const string StringLengthCategory = "The category name must have min length of {0} and max length of {1} characters!";
        public const string QuantityRangeErrorMessage = "Please enter a value bigger than {0}";


        public const string DecimalValidationMessage = "{0} must be a decimal/number between {1} and {2}.";
        public const string EndDateMustBeAfteStartDateMessage = "The end date must be after start date!";
        public const string DiscountValidationMessage = "{0} can only be between {1} and {2} percentages";
        public const string MilisecondsToTakeDiscoutValidatingMessage = "Type miliseconds for order.One second is 1000 miliseconds one our is 3600000 miliseconds.";
        public const string DisplayItemsPerRowValidatingMessage = "Items per row can only be between {1} and {2}!";
        public const string MaxNumberOfOrdersPerTableValidatingMessage = "Max number of active orders for one table must be between {1} and {2}!";
        public const string MaxNumberOfItemsInBagValidatingMessage = "Max number of items in bag can only be between {1} and {2}";
        public const string StringLengthPasswordValidatingMessage = "The {0} must be at least {2} and at max {1} characters long.";
        public const string ConfirmPasswordCompareValidatingMessage = "The password and confirmation password do not match.";

        public const string RequiredField = "The field {0} is required";
        public const string RequiredCustomDecimal = "Please enter a value less than or equal to 9999.99.";
        public const string RequiredNumber = "Please choose a number";
        public const string RequiredUser = "Select user!";
        public const string RequiredOrderType = "Please choose an order type";
        public const string RequiredStartDateMessage = "Please select start date!";
        public const string RequiredEndDateMessage = "Please select end date!";
        public const string NumberFieldMessage = "The field {0} must be a number.";


        public const string MilisecondsToTakeDiscoutDisplayMessage = "Time for order to be delivered without discount";
        public const string DisplayItemsPerRowDisplayMessage = "Display items per row";
        public const string MaxNumberOfOrdersPerTableDisplayMessage = "Max number of active orders for one table";
        public const string MaxNumberOfItemsInBagDisplayMessage = "Max number of items in bag";

        public const string EmailDisplayMessage ="Email";
        public const string UserNameDisplayMessage = "Username";
        public const string FullNameDisplayMessage = "Fullname";
        public const string PasswordDisplayMessage = "Password";
        public const string ConfirmPasswordDisplayMessage = "Confirm password";
        public const string CategoryDisplayMessage = "Category";
        public const string PriceDisplayMessage = "Price";
        public const string PromotionalPriceDisplayMessage = "Promotional Price";
        public const string QuantityDisplayMessage = "Quantity";
        public const string DescriptionDisplayMessage = "Description";
        public const string NameDisplayMessage = "Name";
        public const string SlugDisplayMessage = "Slug";
        public const string PictureDisplayMessage = "Picture";
        public const string RoleDisplayMessage = "Role";
        public const string DiscountDisplayMessage = "Discount";
        public const string BgPriceIndicator = "lv.";
        public const string PreferencesTitle = "Preferences";
        public const string CookersTitle = "Cookers";
        //Order view model
        public const string OrderIdMessage = "Order Id:";
        public const string TotalMessage = "Total";
        public const string QuantityShortMessage = "Qty x";
        public const string AcceptMessage = "Accept";
        public const string DeliveredMessage = "Delivered";
        public const string WaitingForPaimentMessage = "Waiting for paiment";
        public const string StatusMessage = "Status:";
        public const string PriceWithMessage = "Price with";
        public const string DiscountDobleDotMessage = "discount:";
        public const string OrderPriceMessage = "Order price:";
        public const string HoursMessage = "hr.";
        public const string ActiveOrdersMessage = "Orders in progress";
        public const string MyOrdersMessage = "Orders";
        public const string HomeMessage = "Home";
        public const string PrivacyMessage = "Privacy";
        public const string AdminMessage = "Admin";
        public const string CookerMessage = "Cooker";
        //Regular Expressions
        public const string SlugRegexString = @"^(([a-z]+)(-[a-z]+)*)?$";
        public const string ProductAndDescRegexString = @"^[АаБбВвГгДдЕеЖжЗзИиЙйКкЛлМмНнОоПпРрСсТтУуФфХхЦцЧчШшЩщЪъьЮюЯя a-zA-Z]+";
        public const string UserNameRegexString = @"^[a-zA-Z1-9]+";
        public const string FullNameRegexString = @"^[a-zA-Z ]+";
        public const string DescriptionRegexString = @"^[a-zA-Z ]+";
        //Other strings messages
        //Admin area products
        public const string PreferencesTempDataMessage = "Preferences was successfully changed";
        public const string DeleteProductTempDataMessage = "Product was successfully deleted";
        public const string AddProductTempDataMessage = "{0} was add successfully to {1}";
        public const string EditingProductTempDataMessage = "The product was successfully updated";
        //Admin area categories
        public const string AddCategoryMessage = "Category {0} is successfully added to categories!";
        public const string AddCategoryExistsMessage = "{0} already exists!";
        public const string DeleteCategoryMessage = "The category '{0}' was successfully deleted.";
        //Admin area orders
        public const string LastOrdersMessage = "Last {0} orders";
        public const string MostSoldProductsMessage = "Most solds products from {0} to {1} order by {2}";
        public const string MostSoldForTablesMessage = "Most solds for tables from {0} to {1}";
        public const string FoundOrdersForUserMessage = "Searched orders for {0} from {1} to {2}";
        //Table area 
        public const string MaxNumberOfItemsInBagMessage = "Max allowed size of bag is {0} items!";
        public const string MaxNumberOfOrdersPerUserMessage = "You can have only {0} active orders simontaniously!";
        //Identity area
        public const string PhoneNumberMessage = "Phone number";
        public const string SaveMessage = "Save";
        public const string ChangeYourAccountMessage = "Manage your account";
        public const string ChangeAccontPreferences = "Change your account settings";
        public const string ProfileMessage = "Profile";
        public const string ManageEmailMessage = "Manage Email";
        public const string NewEmailMessage = "New email";
        public const string ChangeMessage = "Change";
        public const string ChangePasswordMessage = "Change password";
        public const string CurrentPasswordMessage = "Current password";
        public const string NewPasswordMessage = "New password";
        public const string ConfirmNewPasswordMessage = "Confirm new password";
        public const string UpdatePasswordMessage = "Update password";
        public const string PersonalDataMessage = "Personal Data";
        public const string PersonalDataWarningMessage = "Your account contains personal data that you have given us. This page allows you to download or delete that data.";
        public const string DeletingPersonalDataWarningMessage = "Deleting this data will permanently remove your account, and this cannot be recovered.";
        public const string DeleteMessage = "Delete";
        public const string DownloadMessage = "Download";
        public const string DeletePersonalDataMessage = "Delete Personal Data";
        public const string DeleteDataAndCloseAccountMessage = "Delete data and close my account";
        public const string PhoneNumberValidationMessage = "The {0} field is not a valid phone number.";
        public const string PasswordStringLengthValidationMessage = "The {0} must be at least {2} and at max {1} characters long.";
        public const string ManageExternalLoginsMessage = "Manage your external logins";
        public const string AddServiceForLoginMessage = "Add another service to log in.";
        public const string RegisteredLoginsMessage = "Registered Logins";
        public const string LogInMessage = "Log in";
        public const string RememberMeMessage = "Remember me?";
        public const string InvalidLoginAttempt = "Invalid login attempt.";
        public const string CreateNewAccountMessage = "Create a new account.";
        public const string RegisterMessage = "Register";
        public const string AccessDeniedMessage = "Access denied";
        public const string PermissionsForResourceMessage = "You do not have access to this resource.";
        //Identity custom error localizer
        public const string DefaultError = "An unknown failure has occurred."; 
        public const string DefaultErrorBG = "Незнайно пропадане се появи";
        public const string ConcurrencyFailure = "Optimistic concurrency failure, object has been modified.";
        public const string ConcurrencyFailureBG = "Съвпадение на не успех, обектът е бил променен.";
        public const string PasswordMismatch = "Incorrect password.";
        public const string PasswordMismatchBG = "Невалидна парола.";
        public const string InvalidToken = "Invalid token.";
        public const string InvalidTokenBG = "Невалиден токън";
        public const string LoginAlreadyAssociated = "A user with this login already exists.";
        public const string LoginAlreadyAssociatedBG = "Потребител с този акаунт вече съществува.";
        public const string InvalidUserName = "User name '{0}' is invalid, can only contain letters or digits.";
        public const string InvalidUserNameBG = "Потребителското име '{0}' е невалидно, може да съдържа само букви и цифри.";
        public const string InvalidEmail = "Email '{0}' is invalid.";
        public const string InvalidEmailBG = "Имейлът '{0}' е невалиден.";
        public const string DuplicateUserName = "User Name '{0}' is already taken.";
        public const string DuplicateUserNameBG = "Потребителското име '{0}' е вече заето.";
        public const string DuplicateEmail = "Email '{0}' is already taken.";
        public const string DuplicateEmailBG = "Имейлът '{0}' е вече зает.";
        public const string InvalidRoleName = "Role name '{0}' is invalid.";
        public const string InvalidRoleNameBG = "Ролята '{0}' е невалидна.";
        public const string DuplicateRoleName = "Role name '{0}' is already taken.";
        public const string DuplicateRoleNameBG = "Ролята '{0}' е вече заета.";
        public const string UserAlreadyHasPassword = "User already has a password set.";
        public const string UserAlreadyHasPasswordBG = "Потребителят вече има записана парола.";
        public const string UserLockoutNotEnabled = "Lockout is not enabled for this user.";
        public const string UserLockoutNotEnabledBG = "Заключването на акаунта не е отменено за този потребител.";
        public const string UserAlreadyInRole = "User already in role '{0}'.";
        public const string UserAlreadyInRoleBG = "Потребителят е вече в роля '{0}'.";
        public const string UserNotInRole = "User is not in role '{0}'.";
        public const string UserNotInRoleBG = "Потребителят не е в роля '{0}'.";
        public const string PasswordTooShort = "Passwords must be at least {0} characters.";
        public const string PasswordTooShortBG = "Паролата трябва да е поне {0} символи.";
        public const string PasswordRequiresNonAlphanumeric = "Passwords must have at least one non alphanumeric character.";
        public const string PasswordRequiresNonAlphanumericBG = "Паролите трябва да имат поне един небуквено-цифров знак.";
        public const string PasswordRequiresDigit = "Passwords must have at least one digit ('0'-'9').";
        public const string PasswordRequiresDigitBG = "Паролите трябва да съдържат поне едно число ('0'-'9').";
        public const string PasswordRequiresLower = "Passwords must have at least one lowercase ('a'-'z').";
        public const string PasswordRequiresLowerBG = "Паролите трябва да съдържат поне една малка буква ('a'-'z').";
        public const string PasswordRequiresUpper = "Passwords must have at least one uppercase ('A'-'Z').";
        public const string PasswordRequiresUpperBG = "Паролите трябва да съдържат поне една главна буква ('A'-'Z').";

        //Image paths
        public static string ProductsImageFolderPathWithSlash = @"\Images\Products\";
        public static string ProductsTumbnailsFolderPathWithSlash = @"\Images\Tumbs\";

        public static string ProductsImageFolderPathWithoutSlash = @"Images\Products\";
        public static string ProductsTumbnailsFolderPathWithoutSlash = @"Images\Tumbs\";

        //Available roles
        public const string CookerRole = "Cooker";
        public const string AdminRole = "Administrator";
        public const string TableRole = "Table";

        //Areas
        public const string AdminArea = "Admin";
        public const string CookerArea = "Cooker";

        //Default users
        public const string AdminDefaultUsername = "admin";
        public const string AdminDefaultEmail = "admin@mail.bg";
        public const string AdminDefaultPassword = "admin123";
        public const string CookerDefaultUsername = "cooker";
        public const string CookerDefaultEmail = "cooker@mail.bg";
        public const string CookerDefaultPassword = "cooker123";

        //Session key
        public const string SessionKeyForCartProductAdding = "__cartProducts";
        public const string MessageKey = "__Message";
    }
}
