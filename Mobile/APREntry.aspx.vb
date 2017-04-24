


Namespace core



    Partial Class Mobile_APREntry
        Inherits System.Web.UI.Page

        Public CID As Integer
        Public CID_Print As String

        Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

            Dim CIDList As New List(Of CID)
            CIDList = Me.Session("CID_Info")
            Dim CIDarray = CIDList.ToArray()
            If CIDarray.Length > 0 Then
                CID_Print = CIDarray(0).CID_Print
            End If
        End Sub



    End Class

End Namespace
