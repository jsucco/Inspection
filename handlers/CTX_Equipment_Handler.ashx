<%@ WebHandler Language="VB" Class="core.CTX_Equipment_Handler" %>

Imports System
Imports System.Web
Imports App.Utilities.Web.Handlers
Imports System.Web.Script.Serialization
Imports System.Data.SqlClient
Imports System.Data
Imports System.Globalization

Namespace core

    Public Class CTX_Equipment_Handler
        Inherits BaseHandler

        Private DAO As New DAOFactory
        Private Util As New Utilities

        Public Function GetMaintGroupNames(ByVal level As Integer, ByVal item As String) As String

            Dim sqlstring As String
            Dim returnstring As String
            Dim Equipment As New List(Of Equipment)()
            Dim jser As New JavaScriptSerializer
            Dim result As Boolean

            Select Case level
                Case 0
                    sqlstring = "select Distinct MG_Name" & vbCrLf &
                            "from Maintenance_Schedule MS" & vbCrLf &
                            "Inner Join Machines M" & vbCrLf &
                            "ON M.MM_Id = MS.MM_Id" & vbCrLf &
                            "Inner Join Maintenance_Types MT" & vbCrLf &
                            "ON MT.MT_Id = MS.MT_Id" & vbCrLf &
                            "Inner Join Maintenance_Groups MG" & vbCrLf &
                            "ON MG.MG_Id = MT.MG_Id"
                Case 1
                    sqlstring = "select Distinct MM_Name, MM_Number" & vbCrLf &
                            "from Maintenance_Schedule MS" & vbCrLf &
                            "Inner Join Machines M" & vbCrLf &
                            "ON M.MM_Id = MS.MM_Id" & vbCrLf &
                            "Inner Join Maintenance_Types MT" & vbCrLf &
                            "ON MT.MT_Id = MS.MT_Id" & vbCrLf &
                            "Inner Join Maintenance_Groups MG" & vbCrLf &
                            "ON MG.MG_Id = MT.MG_Id" & vbCrLf &
                            "where MG_Name = '" & Convert.ToString(item) & "' and Mach_Image1 is not null" & vbCrLf &
                            "order by MM_Number desc;"
                Case Else
                    sqlstring = "Nothing"

            End Select

            Equipment = DAO.getEquipment(sqlstring)
            returnstring = jser.Serialize(Equipment.ToArray())

            Return returnstring

        End Function

    End Class

End Namespace
