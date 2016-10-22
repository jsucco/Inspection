<%@ WebHandler Language="VB" Class="core.DisplayBoard" %>

Imports System
Imports System.Web
Imports App.Utilities.Web.Handlers
Imports System.Data
Imports System.Web.Script.Serialization

Namespace core
    


    Public Class DisplayBoard
        Inherits BaseHandler
    
        Private DSB As New DisplayBoardDAO
        Private Util As New Utilities
        Private DAO As New DAOFactory
        
        Public Function GetDisplayBoardSlide(ByVal ShowId As Integer, ByVal SlideOrder As Integer) As String
            
            Dim returnstring As String
            
            returnstring = DSB.GetDSBSlide(ShowId, SlideOrder)
            
            If Not returnstring = "NoData" Then
                Return returnstring
            Else
                Return "Error"
            End If
            
        End Function
        
        Public Function GetChartData() As String
            
            Dim returnstring As String
            
            returnstring = DSB.GetUtilityChart()
            
            If Not returnstring = "NoData" Then
                Return returnstring
            Else
                Return "Error"
            End If
            
        End Function
        
        Public Function GetTotalSlideNumber(ByVal ShowId As Integer) As Integer
            
            Dim sqlstring As String
            Dim ScheduleSet As DataSet = New DataSet
            Dim jser As New JavaScriptSerializer
            
            sqlstring = "select count(*) as count from dbo.DashBoardSchedule where ShowId = 1"

            If Not Util.FillTrnDataSet(ScheduleSet, "ScheduleSet", sqlstring, 2) Then
                Return False
            End If
            Return ScheduleSet.Tables(0).Rows(0)("count")
            
        End Function

    End Class
    
    
End Namespace
