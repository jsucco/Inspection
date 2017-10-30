<%@ WebHandler Language="VB" Class="core.SPC_MaintFlag_Handler" %>
Imports App.Utilities.Web.Handlers
Imports System.Web.Script.Serialization

Namespace core

    Public Class SPC_MaintFlag_Handler
        Inherits BaseHandler
        Private Util As New Utilities
        Private DAO As New DAOFactory

        Dim ErrorMessage As String
        Dim ErrorFlag As Boolean

        Public Function GetShiftinfo() As String
            Dim returnstring As String
            Dim ShiftArray As New List(Of ShiftTime)()

            ShiftArray = GetShiftnTime(DateTime.Now)
            Dim jser As New JavaScriptSerializer
            If ShiftArray.Count > 0 Then
                returnstring = jser.Serialize(ShiftArray.ToArray())
            Else
                returnstring = Nothing
            End If

            Return returnstring

        End Function

        Public Function GetShiftnTime(ByVal InputDate As DateTime) As List(Of ShiftTime)

            Dim ShiftList As New List(Of ShiftTime)()
            Dim Today As DateTime = DateTime.Today
            Dim DateShift As String

            DateShift = Today.ToString("d")

            Select Case InputDate

                Case Today.AddHours(8) To Today.AddHours(16)
                    ShiftList.Add(New ShiftTime With {.shiftNum = 1, .StartHour = Convert.ToString(Today.AddHours(8)), .EndHour = Convert.ToString(Today.AddHours(16))})
                Case Today.AddHours(16) To Today.AddHours(24)
                    ShiftList.Add(New ShiftTime With {.shiftNum = 2, .StartHour = Convert.ToString(Today.AddHours(16)), .EndHour = Convert.ToString(Today.AddHours(24))})
                Case Today To Today.AddHours(8)
                    ShiftList.Add(New ShiftTime With {.shiftNum = 2, .StartHour = Convert.ToString(Today), .EndHour = Convert.ToString(Today.AddHours(8))})

            End Select

            Return ShiftList

        End Function

    End Class

    Public Class ShiftTime

        Public Property shiftNum As Integer
        Public Property StartHour As String
        Public Property EndHour As String

    End Class
End Namespace
