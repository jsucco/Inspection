Imports System
Imports System.Collections.Specialized
Imports System.Text
Imports System.Web
Imports System.Web.UI.Page
Imports System.Data.SqlClient


Namespace core

    Public Class Environment
        Dim _context As HttpContext
        'Class specific variables
        Public Sub New(ByRef context As HttpContext)
            _context = context
        End Sub
        Public ReadOnly Property AppIsAvailable() As Boolean
            Get
                If Me.GetAppSetting("ApplicationAvailable").ToUpper() = "TRUE" Then
                    Return True
                Else
                    Return False
                End If
            End Get
        End Property
        Public Shared ReadOnly Property BusinessObjectDirectory() As String
            Get
                Return "/CTX/APP/BusObj/"
            End Get
        End Property
        Public Shared ReadOnly Property CommonDirectory() As String
            Get
                Return "/CTX/APP/Common/"
            End Get
        End Property
        Public Shared ReadOnly Property DataAccessDirectory() As String
            Get
                Return "/CTX/APP/DataAccess/"
            End Get
        End Property
        Public Shared ReadOnly Property DataSetDirectory() As String
            Get
                Return "/CTX/APP/Report/DataSets/"
            End Get
        End Property
        Public Shared ReadOnly Property ImageDirectory() As String
            Get
                Return "/CTX/IMG/"
            End Get
        End Property
        Public Shared ReadOnly Property MainAppDirectory() As String
            Get
                Return "/CTX/APP/"
            End Get
        End Property
        Public Shared ReadOnly Property MenuDirectory() As String
            Get
                Return "/CTX/APP/Common/Menu/"
            End Get
        End Property
        Public Shared ReadOnly Property NotificationDirectory() As String
            Get
                Return "/CTX/NTF/"
            End Get
        End Property
        Public Shared ReadOnly Property PresentationMainDirectory() As String
            Get
                Return "/CTX/APP/Presentation/"
            End Get
        End Property
        Public Shared ReadOnly Property PresentationDataEntryDirectory() As String
            Get
                Return "/CTX/APP/Presentation/DataEntry/"
            End Get
        End Property
        Public Shared ReadOnly Property PresentationFormsDirectory() As String
            Get
                Return "/CTX/APP/Presentation/Forms/"
            End Get
        End Property
        Public Shared ReadOnly Property PresentationMasterFilesDirectory() As String
            Get
                Return "/CTX/APP/Presentation/MasterFiles/"
            End Get
        End Property
        Public Shared ReadOnly Property PresentationUtilitiesDirectory() As String
            Get
                Return "/CTX/APP/Presentation/Utilities/"
            End Get
        End Property
        Public Shared ReadOnly Property PresentationUtilitiesDownloadDirectory() As String
            Get
                Return "/CTX/APP/Presentation/Utilities/CTXDownloads/"
            End Get
        End Property
        Public Shared ReadOnly Property ReportDirectory() As String
            Get
                Return "/CTX/APP/Report/"
            End Get
        End Property
        Public Shared ReadOnly Property ReportDocumentDirectory() As String ' PJL 1/13/06
            Get
                Return "/CTX/APP/Report/RPT/"
            End Get
        End Property
        Public Shared ReadOnly Property PresentationReportDirectory() As String
            Get
                Return "/CTX/APP/Presentation/Reports/"
            End Get
        End Property
        Public Shared ReadOnly Property PresentationCommonDirectory() As String
            Get
                Return "/CTX/APP/Presentation/Common/"
            End Get
        End Property
        Public Shared ReadOnly Property RootDirectory() As String
            Get
                Return "/CTX/"
            End Get
        End Property
        Public Shared ReadOnly Property StyleSheetDirectory() As String
            Get
                Return "/CTX/CSJ/"
            End Get
        End Property
        Public Shared ReadOnly Property HandHeldWebServiceDirectory() As String
            Get
                Return "/CTX/WS/HHws/"
            End Get
        End Property
        Public Shared ReadOnly Property PresentationInquiryDirectory() As String    'added by bms 10/15/04
            Get
                Return "/CTX/APP/Presentation/Inquiry/"
            End Get
        End Property
        Public Shared ReadOnly Property WindowsTempDirectory() As String    ' DJP 6/22/12
            Get
                Return "C:\Windows\Temp\"
            End Get
        End Property
        Public Function GetAppSetting(ByVal setting As String) As String
            '*************************************************************************
            ' Returns settings in the <appSettings> section of web.config
            '*************************************************************************
            Dim SettingsCollection As NameValueCollection
            GetAppSetting = ""
            SettingsCollection = _context.GetSection("appSettings")
            If Not (SettingsCollection Is Nothing) Then
                Try
                    GetAppSetting = SettingsCollection(setting).ToString()
                Catch ex As Exception     'If setting was not found
                End Try
            End If
        End Function
        
    End Class

End Namespace
