Imports System.Data
Imports System.Data.SqlClient


Namespace core



    Partial Class PictureVB
        Inherits System.Web.UI.Page
        Dim Inspect As New InspectionUtilityDAO
        Dim ImageList As Array
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Request.QueryString("DefectID") IsNot Nothing And IsNumeric(Request.QueryString("DefectID")) = True Then
                Dim DefectId As Integer = Convert.ToInt64(Request.QueryString("DefectID"))
                CheckCache(DefectId)
                If ImageList Is Nothing Then
                    Dim ImageObj = Inspect.GetDefectImage(DefectId)
                    If IsNothing(ImageObj) = False Then
                        ImageList = ImageObj.ToArray()

                        If ImageList(0).DefectImage.Length > 100 And Len(ImageList(0).DefectImage_Filename.ToString()) > 1 Then
                            InsertIntoCache(DefectId, ImageList)
                        End If
                    End If
                End If
                If ImageList IsNot Nothing Then
                    Dim bytes() As Byte = CType(ImageList(0).DefectImage, Byte())
                    Response.Buffer = True
                    Response.Charset = ""
                    Response.Cache.SetCacheability(HttpCacheability.NoCache)
                    Response.ContentType = "image/jpeg"
                    Response.AddHeader("content-disposition", "attachment;filename=" & ImageList(0).DefectImage_Filename)
                    Response.BinaryWrite(bytes)
                    Response.Flush()
                    Response.End()
                End If
            End If
        End Sub
        Public Function CheckCache(ByVal DefectId As Integer) As Array
            Dim CacheObject As Object = Cache("CacheImageDefectId_" + DefectId.ToString())
            If Not CacheObject Is Nothing Then
                ImageList = CacheObject
            End If
        End Function
        Public Sub InsertIntoCache(ByVal DefectId As Integer, ByVal ImageListInput As Array)
            If ImageListInput(0).Defectimage.Length > 100 Then
                Context.Cache.Insert("CacheImageDefectId_" + DefectId.ToString(), ImageList, Nothing, Now.AddDays(100), System.Web.Caching.Cache.NoSlidingExpiration)
            End If
        End Sub
        Public Function GetData(ByVal cmd As SqlCommand) As DataTable
            Dim dt As New DataTable
            Dim dl As New dlayer

            Dim strConnString As String = dl.CtxConnectionString
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