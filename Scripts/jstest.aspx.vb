
Imports System
Imports System.Data
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Data.SqlClient
Imports System.Text
Imports System.Threading.Tasks
Imports System.Web.Script.Serialization


Namespace core


    Public Class macnames

        Public Property names As String


    End Class

    Partial Class UTILITIES_chartmaster
        Inherits System.Web.UI.Page

        Public Shared maxDate As DateTime
        Public Shared minDate As DateTime
        Public Shared machinenames As Array
        Public Shared teststring As String

        Public Shared equipmentArray As String


        Sub Page_load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

            Dim dtaker = New dtaker
            Dim mgroups As New mgroups
            Dim mgtable As DataTable = mgroups.getmachinenames
            Dim names As New List(Of String)
            Dim row As DataRow
            Dim serializer As New JavaScriptSerializer()
            Dim cs As ClientScriptManager = Page.ClientScript
            Dim chartdates As Tuple(Of DateTime, DateTime)
            Dim i As Integer = 0



            For Each row In mgtable.Rows
                'names.Add(New macnames() With {.names = rows.Item(0)})
                equipmentArray &= """" & row.Item(0) & ""","
            Next
            equipmentArray = equipmentArray.TrimEnd(",")

            machinenames = names.ToArray()
            teststring = "testing"


            'For Each rows In mgtable.Rows
            '    cs.RegisterArrayDeclaration("mnames", machinenames(i).ToString)
            '    i += 1
            'Next



        End Sub


    End Class



End Namespace
