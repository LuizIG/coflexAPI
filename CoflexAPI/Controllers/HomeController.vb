Public Class HomeController
    Inherits System.Web.Mvc.Controller

    Function Index() As ActionResult
        ViewData("Title") = "Coflex API"
        Return View()
    End Function
End Class
