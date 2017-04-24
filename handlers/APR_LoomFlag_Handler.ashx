<%@ WebHandler Language="VB" Class="core.APR_LoomFlag_Handler" %>

Imports System
Imports System.Web
Imports App.Utilities.Web.Handlers
Imports System.Web.Script.Serialization
Imports System.Data.SqlClient
Imports System.Data
Imports System.Globalization

Namespace core

    Public Class APR_LoomFlag_Handler
        Inherits BaseHandler
        
        Private LPC As New LoomPickCountDAO
        Private Util As New Utilities
        Private DAO As New DAOFactory
    
        Public Function GetLoomStatsGrid() As String
            
            Dim returnstring As String
            
            returnstring = LPC.GetLSGrid()
            
            If Not returnstring = "NoData" Then
                Return returnstring
            Else
                Return "Error"
            End If
            
            
        End Function
        
        Public Function GetCurrPicks() As String
            
            Dim returnstring As String
            
            returnstring = LPC.GetCurrPC()
            
            If Not returnstring = "NoData" Then
                Return returnstring
            Else
                Return "Error"
            End If
            
            
        End Function
        
        Public Function GetLastStop(ByVal LMN As Integer) As String
            
            Dim returnstring As String
            
            returnstring = LPC.GetLastStop(LMN)
            
            If Not returnstring = "NoData" Then
                Return returnstring
            Else
                Return "Error"
            End If
            
            
        End Function
        
        Public Function GetLoomPicksGrid(ByVal No As Integer) As String
            
            Dim returnstring As String
            Dim GridArray As Array
            Dim ReturnArray As New List(Of LoomPickGrid)()
            Dim counter As Integer = 0
            Dim field As String
            Dim val As Decimal
            Dim jser As New JavaScriptSerializer
            
            GridArray = LPC.GetPCGrid(No)
            
            If GridArray.Length > 0 Then
                For Each LoomPick As LoomPickStats In GridArray
                    
                    For i = 0 To 2
                    
                        Select Case counter
                            Case 0
                                field = "STOPS/MMPicks"
                                val = LoomPick.PickCount_Curr
                            Case 1
                                field = "Shift Avg."
                                val = LoomPick.PickCount_ShiftAvg
                            Case 2
                                field = "Shift Max"
                                val = LoomPick.PickCount_Max
                            Case Else
                                field = "Err"
                                val = 99
                        End Select
                    
                        ReturnArray.Add(New LoomPickGrid() With {.field = field, .value = val})
                
                        counter += 1
                        
                    Next
                        
                Next
            End If
            
            If ReturnArray.Count > 1 Then
                Try
                    returnstring = jser.Serialize(ReturnArray.ToArray())
                    Return returnstring
                Catch ex As Exception

                End Try
            Else
                    
            End If
     
            returnstring = "Error"
            Return returnstring
            
        End Function

    End Class
    
End Namespace
