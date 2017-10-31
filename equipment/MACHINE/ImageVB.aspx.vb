Imports System.Data
Imports System.Data.SqlClient

Namespace core

    Partial Class PictureVB
        Inherits System.Web.UI.Page

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Request.QueryString("ImageID") IsNot Nothing Then
                Dim strQuery As String = "SELECT [MM_Id]" & vbCrLf &
                ",[MM_Name]" & vbCrLf &
                ",[MM_Description]" & vbCrLf &
                ",[Mach_Filename1]" & vbCrLf &
                ",[Mach_Image1]" & vbCrLf &
                "FROM Machines" & vbCrLf &
                "where MM_Id = @id"
                Dim cmd As SqlCommand = New SqlCommand(strQuery)
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = Convert.ToInt32(Request.QueryString("ImageID"))
                Dim dt As DataTable = GetData(cmd)
                If dt IsNot Nothing Then
                    Dim bytes() As Byte = CType(dt.Rows(0)("Mach_Image1"), Byte())
                    Response.Buffer = True
                    Response.Charset = ""
                    Response.Cache.SetCacheability(HttpCacheability.NoCache)
                    Response.ContentType = "image/jpeg"
                    Response.AddHeader("content-disposition", "attachment;filename=" & dt.Rows(0)("MM_Name").ToString())
                    Response.BinaryWrite(bytes)
                    Response.Flush()
                    Response.End()
                End If
            End If
        End Sub
        Public Function GetData(ByVal cmd As SqlCommand) As DataTable
            Dim dt As New DataTable
            Dim dl As New dlayer

            Dim strConnString As String = dl.GetCtxConnectionString_("")
            Dim con As New SqlConnection(strConnString)
            Dim sda As New SqlDataAdapter
            cmd.CommandType = CommandType.Text
            cmd.Connection = con
            Try
                con.Open()
                sda.SelectCommand = cmd
                sda.Fill(dt)
                Return dt
            Catch ex As Exception
                Response.Write(ex.Message)
                Return Nothing
            Finally
                con.Close()
                sda.Dispose()
                con.Dispose()
            End Try
        End Function

    End Class




End Namespace