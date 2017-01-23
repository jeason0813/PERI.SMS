# GHL.PaymentSolutions WebUi

The purpose of this project is to provide a generic web UI for GHL.PaymentSolutions.

You may need to understand the nature of MVC before going through the entire solution. Because of abstraction and frequent changing of schema, we choose MVC, for it suits best.

## People to blame

The following personnel are responsible for managing this project.
- Alvin.Chua@ghl.com

## Developer's Guide
Here are some facts:
- Design pattern: **MVC5** (.Net framework 4.6.1)
- Uses **EntityFramework 6** for data manipulation
- UI uses the default **GHL template** though some css/scripts were heavily modified
- Most UI manipulations were done by **JQuery/BootStrap**
- Uses **Nlog** for logging series of events

Solution structure:
- ```GHL.PaymentSolutions.WebUi.Core```
	- Contains the common methods and functions of the system
- ```GHL.PaymentSolutions.WebUi.EF```
	- Contains the EntityFramework module
	- All of the data-manipulations were done here
- ```GHL.PaymentSolutionsWebUi.Mvc```
	- This is the main project.
- ```GHL.PaymentSolutions.WebUi.Mvc.Bll```
	- Contains the POCO classes used by the main project
- ```GHL.PaymentSolutions.WebUi.Mvc.Test```
	- UnitTest project
- ```GHL.PaymentSolutions.WebUi.Mvc.XU```
	- Xunit ~ for extended unit testing

## Want to help?

If you've found a problem, or want a feature, or have any suggestions or criticisms, please file a GitHL issue.