
Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks
Imports System.Data.SqlClient
Imports System.Data
Imports System.Globalization
Imports System.IO
Imports System.Web
Imports System.Net

Namespace core


    Public Class Milnor

        Public Property Timestamp As DateTime
        Public Property Milnor_hotw_gal As Single
        Public Property Milnor_coldw_gal As Single

    End Class

    Public Class Main

        Public Property Timestamp As DateTime
        Public Property Main_Hot_Water_g As Single
        Public Property Main_Cold_Water_g As Single

    End Class

    Public Class usage

        Public Property Timestamp As DateTime
        Public Property Usage1 As Single
        Public Property Usage2 As Single

    End Class

    Public Class Mainelec

        Public Property Timestamp As DateTime
        Public Property Main_Power_W As Single


    End Class

    Public Class Mainnatgas

        Public Property Timestamp As DateTime
        Public Property Main_Gas_Usage As Single


    End Class

    Public Class SummedWater

        Public Property Milnor_hotw_gal As Single
        Public Property Milnor_coldw_gal As Single
        Public Property Main_Hot_Water_g As Single
        Public Property Main_Cold_Water_g As Single

    End Class
    Public Class SummWater

        Public Property category As String
        Public Property gallons As Single


    End Class


    Public Class dtaker

        Public Shared MilnorWater As Array

        Dim datestring As String

        Dim dataReader As SqlDataReader
        Dim dateTable As DataTable
        Dim datecmdBuilder As SqlCommandBuilder
        Dim datecmd As SqlCommand
        Dim dataConnection As SqlConnection

        Dim dtMilnorstring As String
        Dim dtMilnorReader As SqlDataReader
        Dim dtMilnorcmdBuilder As SqlCommandBuilder
        Dim dtMilnorcmd As SqlCommand
        Dim dtMilnorConnection As SqlConnection

        Dim Mainstring As String
        Dim MainReader As SqlDataReader
        Dim MaincmdBuilder As SqlCommandBuilder
        Dim Maincmd As SqlCommand
        Dim MainConnection As SqlConnection


        '' Summary
        '' Overrides the connection timeout


        Public Property ConnectionTimeout As Int32 = 20

        '' summary
        '' Property Used to override the name of the application

        Public Property ApplicationName As String = "ChartMaster"
        Public Shared chartsdrawcnt As Integer
        Public Shared chartsdrawcnt2 As Integer
        Public Shared specdrawcnt As Integer
        Public Shared specdrawcnt2 As Integer
        Public Shared melecdrawcnt As Integer
        Public Shared melecdrawcnt2 As Integer
        Public Shared mgasdrawcnt As Integer
        Public Shared mgasdrawcnt2 As Integer
        Public Shared barchartsdrawcnt As Integer
        Public Shared barchartsdrawcnt2 As Integer
        Public Shared chartselector As Integer
        Public Shared schselector As Integer

        Public Function getdatestable(ByVal schnum As Int16) As Tuple(Of DateTime, DateTime)


            Dim dl = New dlayer
            Dim rowscnt As Integer = 0
            Dim colcnt As Integer = 0
            Dim datelist As New List(Of String)
            Dim retVal = New Tuple(Of DateTime, DateTime)(DateTime.Now, DateTime.Now)

            'If schnum = 1 Then
            '    datestring = "select MIN([TimeStamp]) AS MinTimeStamp, MAX([TimeStamp]) AS MaxTimeStamp from dbo.ST_DL_sch1 where id > 2000 order by TimeStamp asc"
            'ElseIf schnum = 2 Then
            '    datestring = "select [TimeStamp] from dbo.ST_DL_sch2 where id > 2000 order by TimeStamp asc"
            'End If

            datestring = "select MIN([Timestamp]) AS MinTimeStamp, MAX([Timestamp]) AS MaxTimeStamp from dbo.DI_Main_1A_Cd"
            dataConnection = New SqlConnection(dl.COREDemoConnectionString)
            dataConnection.Open()
            datecmd = New SqlCommand(datestring, dataConnection)
            dataReader = datecmd.ExecuteReader(CommandBehavior.CloseConnection)
            If dataReader.Read Then
                retVal = New Tuple(Of DateTime, DateTime)(Convert.ToDateTime(dataReader("MinTimeStamp")), Convert.ToDateTime(dataReader("MaxTimeStamp")))
            End If
            dataReader.Close()



            Return retVal
        End Function
        Public Function getMilnorWaterData(ByVal fromdate As DateTime, ByVal ToDate As DateTime) As List(Of Milnor)   'ByVal fromdate As DateTime, ByVal ToDate As DateTime

            Dim reval As New List(Of Milnor)()
            Dim dl As New dlayer
            Dim milnor As New Milnor
            Dim record As IDataRecord
            Dim TimeStamp As DateTime
            Dim Milnorhot As Single
            Dim Milnorcold As Single
            ' Dim dtMilnorstring2 As String
            ' Dim fromdatetest As String = fromdate(0)

            dtMilnorstring = "select [Timestamp] as Timestamp, [Milnor_HW_Usage] as Milnor_hot_water_gal,[Milnor_CW_Usage] as Milnor_cold_water_gal from dbo.DI_Milnor_1A_Cd where Timestamp > '" + fromdate.ToString("yyyy-MM-dd hh:mm:ss") + "' and Timestamp < '" + ToDate.ToString("yyyy-MM-dd hh:mm:ss") + "' order by Timestamp asc"
            'dtMilnorstring = "select [TimeStamp] as TimeStamp, [Milnor_hot_water_gal] as Milnor_hot_water_gal,[Milnor_cold_water_gal] as Milnor_cold_water_gal  from dbo.ST_DL_sch1 where TimeStamp > '2014-01-23 21:28:00' and Timestamp < '2014-01-29 12:52:00' order by Timestamp asc"
            dtMilnorConnection = New SqlConnection(dl.CrConnectionString)
            dtMilnorConnection.Open()
            dtMilnorcmd = New SqlCommand(dtMilnorstring, dtMilnorConnection)
            dtMilnorReader = dtMilnorcmd.ExecuteReader(CommandBehavior.CloseConnection)

            While dtMilnorReader.Read
                record = CType(dtMilnorReader, IDataRecord)
                TimeStamp = Date.Parse(record(0), CultureInfo.InvariantCulture)
                Milnorhot = Single.Parse(record(1), CultureInfo.InvariantCulture)
                Milnorcold = Single.Parse(record(2), CultureInfo.InvariantCulture)

                reval.Add(New Milnor() With {.Timestamp = TimeStamp, .Milnor_hotw_gal = Milnorhot, .Milnor_coldw_gal = Milnorcold})
            End While


            dtMilnorReader.Close()


            'MilnorWater = reval.ToArray()
            Return reval

        End Function

        Public Function getMainWaterData(ByVal fromdate As DateTime, ByVal ToDate As DateTime) As List(Of Main)   'ByVal fromdate As DateTime, ByVal ToDate As DateTime

            Dim reval As New List(Of Main)()
            Dim dl As New dlayer
            Dim Main As New Main
            Dim record As IDataRecord
            Dim TimeStamp As DateTime
            Dim Mainhot As Single
            Dim Maincold As Single
            ' Dim dtMilnorstring2 As String
            ' Dim fromdatetest As String = fromdate(0)

            Mainstring = "select [Timestamp] as Timestamp, [Mains_HW_Usage] as Main_Hot_Water_g,[Mains_CW_Usage] as Main_Cold_Water_g  from dbo.DI_Main_1A_Cd where Timestamp > '" + fromdate.ToString("yyyy-MM-dd hh:mm:ss") + "' and Timestamp < '" + ToDate.ToString("yyyy-MM-dd hh:mm:ss") + "' order by Timestamp asc"
            'dtMilnorstring = "select [TimeStamp] as TimeStamp, [Main_Hot_Water_g] as Main_Hot_Water_g,[Main_Cold_Water_g] as Main_Cold_Water_g  from dbo.ST_DL_sch2 where TimeStamp > '2014-01-23 21:28:00' and Timestamp < '2014-01-29 12:52:00' order by Timestamp asc"
            MainConnection = New SqlConnection(dl.CrConnectionString)
            MainConnection.Open()
            Maincmd = New SqlCommand(Mainstring, MainConnection)
            MainReader = Maincmd.ExecuteReader(CommandBehavior.CloseConnection)

            While MainReader.Read
                record = CType(MainReader, IDataRecord)
                TimeStamp = Date.Parse(record(0), CultureInfo.InvariantCulture)
                Mainhot = Single.Parse(record(1), CultureInfo.InvariantCulture)
                Maincold = Single.Parse(record(2), CultureInfo.InvariantCulture)

                reval.Add(New Main() With {.Timestamp = TimeStamp, .Main_Hot_Water_g = Mainhot, .Main_Cold_Water_g = Maincold})
            End While


            MainReader.Close()


            'MilnorWater = reval.ToArray()
            Return reval

        End Function

        Public Function getsummedwater(ByVal fromdate As DateTime, ByVal ToDate As DateTime) As List(Of SummWater)

            Dim MainArray As Array = getMainWaterData(fromdate, ToDate).ToArray()
            Dim Milnorarray As Array = getMilnorWaterData(fromdate, ToDate).ToArray()
            Dim num As Integer = MainArray.Rank
            Dim reval As New List(Of SummWater)()
            Dim Maintothnumber As Main
            Dim Maintothnumber2 As Main
            Dim Maintotcnumber As Main
            Dim Maintotcnumber2 As Main
            Dim Milnortothnumber As Milnor
            Dim Milnortothnumber2 As Milnor
            Dim Milnortotcnumber As Milnor
            Dim Milnortotcnumber2 As Milnor
            Dim ndiff As Single = 0
            Dim ndiff2 As Single = 0
            Dim ndiff3 As Single = 0
            Dim ndiff4 As Single = 0
            Dim totMainhot As Single = 0
            Dim totMaincold As Single = 0
            Dim totMilnorhot As Single = 0
            Dim totMilnorcold As Single = 0
            Dim catarray() As String = {"Main_hot", "Main_cold", "Milnor_hot", "Milnor_cold"}
            Dim galarray() As Single = {0, 0, 0, 0}


            For indexA As Integer = 1 To MainArray.Length - 1
                Maintothnumber = MainArray(indexA)
                Maintothnumber2 = MainArray(indexA - 1)
                ndiff = Maintothnumber.Main_Hot_Water_g - Maintothnumber2.Main_Hot_Water_g
                If ndiff > 0 Then
                    totMainhot = totMainhot + ndiff
                End If

                Maintotcnumber = MainArray(indexA)
                Maintotcnumber2 = MainArray(indexA - 1)
                ndiff2 = Maintotcnumber.Main_Cold_Water_g - Maintotcnumber2.Main_Cold_Water_g
                If ndiff2 > 0 Then
                    totMaincold = totMaincold + ndiff2
                End If
            Next
            For indexb As Integer = 1 To Milnorarray.Length - 1
                Milnortothnumber = Milnorarray(indexb)
                Milnortothnumber2 = Milnorarray(indexb - 1)
                ndiff3 = Milnortothnumber.Milnor_hotw_gal - Milnortothnumber2.Milnor_hotw_gal
                If ndiff3 > 0 Then
                    totMilnorhot = totMilnorhot + ndiff3
                End If

                Milnortotcnumber = Milnorarray(indexb)
                Milnortotcnumber2 = Milnorarray(indexb - 1)
                ndiff4 = Milnortotcnumber.Milnor_coldw_gal - Milnortotcnumber2.Milnor_coldw_gal
                If ndiff4 > 0 Then
                    totMilnorcold = totMilnorcold + ndiff4
                End If
            Next

            galarray(0) = totMainhot
            galarray(1) = totMaincold
            galarray(2) = totMilnorhot
            galarray(3) = totMilnorcold

            For i = 0 To 3
                reval.Add(New SummWater() With {.category = catarray(i), .gallons = galarray(i)})
            Next

            Return reval


        End Function

        Public Function getMainelecData(ByVal fromdate As DateTime, ByVal ToDate As DateTime) As List(Of Mainelec)   'ByVal fromdate As DateTime, ByVal ToDate As DateTime

            Dim reval As New List(Of Mainelec)()
            Dim dl As New dlayer
            Dim Mainelec As New Mainelec
            Dim record As IDataRecord
            Dim TimeStamp As DateTime
            Dim Mainelecs As Single

            ' Dim dtMilnorstring2 As String
            ' Dim fromdatetest As String = fromdate(0)

            Mainstring = "select [Timestamp] as Timestamp, [Mains_Power] as Main_Power_W from dbo.DI_Main_1A_Cd where Timestamp > '" + fromdate.ToString("yyyy-MM-dd hh:mm:ss") + "' and Timestamp < '" + ToDate.ToString("yyyy-MM-dd hh:mm:ss") + "' order by Timestamp asc"
            'dtMilnorstring = "select [TimeStamp] as TimeStamp, [Main_Power_W] as Main_Power_W from dbo.ST_DL_sch2 where TimeStamp > '2014-01-23 21:28:00' and Timestamp < '2014-02-09 12:52:00' order by Timestamp asc"
            MainConnection = New SqlConnection(dl.CrConnectionString)
            MainConnection.Open()
            Maincmd = New SqlCommand(Mainstring, MainConnection)
            MainReader = Maincmd.ExecuteReader(CommandBehavior.CloseConnection)

            While MainReader.Read
                record = CType(MainReader, IDataRecord)
                TimeStamp = Date.Parse(record(0), CultureInfo.InvariantCulture)
                Mainelecs = (Single.Parse(record(1), CultureInfo.InvariantCulture)) / 1000


                reval.Add(New Mainelec() With {.Timestamp = TimeStamp, .Main_Power_W = Mainelecs})
            End While


            MainReader.Close()


            'MilnorWater = reval.ToArray()
            Return reval

        End Function

        Public Function getMainnatgasData(ByVal fromdate As DateTime, ByVal ToDate As DateTime) As List(Of Mainnatgas)   'ByVal fromdate As DateTime, ByVal ToDate As DateTime

            Dim reval As New List(Of Mainnatgas)()
            Dim dl As New dlayer
            Dim Mainnatgas As New Mainnatgas
            Dim record As IDataRecord
            Dim TimeStamp As DateTime
            Dim Mainnats As Single

            ' Dim dtMilnorstring2 As String
            ' Dim fromdatetest As String = fromdate(0)

            Mainstring = "select [Timestamp] as Timestamp, [Mains_NG_Usage] as Mains_nat_gas_ccf from dbo.DI_Main_1A_Cd where Timestamp > '" + fromdate.ToString("yyyy-MM-dd hh:mm:ss") + "' and Timestamp < '" + ToDate.ToString("yyyy-MM-dd hh:mm:ss") + "' order by Timestamp asc"
            'dtMilnorstring = "select [TimeStamp] as TimeStamp, [Mains_nat_gas_ccf] as Mains_nat_gas_ccf from dbo.ST_DL_sch2 where TimeStamp > '2014-01-23 21:28:00' and Timestamp < '2014-02-09 12:52:00' order by Timestamp asc"
            MainConnection = New SqlConnection(dl.CrConnectionString)
            MainConnection.Open()
            Maincmd = New SqlCommand(Mainstring, MainConnection)
            MainReader = Maincmd.ExecuteReader(CommandBehavior.CloseConnection)

            While MainReader.Read
                record = CType(MainReader, IDataRecord)
                TimeStamp = Date.Parse(record(0), CultureInfo.InvariantCulture)
                Mainnats = Single.Parse(record(1), CultureInfo.InvariantCulture)


                reval.Add(New Mainnatgas() With {.Timestamp = TimeStamp, .Main_Gas_Usage = Mainnats})
            End While


            MainReader.Close()


            'MilnorWater = reval.ToArray()
            Return reval

        End Function

        Public Function getmgasinterval(ByVal fromdate As DateTime, ByVal ToDate As DateTime) As List(Of Mainnatgas)
            Dim MainArray As Array = getMainnatgasData(fromdate, ToDate).ToArray()
            Dim Maingasnumber As Mainnatgas
            Dim Maingasnumber2 As Mainnatgas
            Dim Maingasnumber3 As Mainnatgas
            Dim ndiff As Single = 0
            Dim totMaingas As Single = 0
            Dim timestamp As DateTime
            Dim timestamp2 As DateTime
            Dim hour As Integer
            Dim hour2 As Integer
            Dim shortdate As String
            Dim hourdate As DateTime
            Dim mainglist As New List(Of Mainnatgas)()

            For indexA As Integer = 1 To MainArray.Length - 1
                Maingasnumber = MainArray(indexA)

                Maingasnumber3 = MainArray(indexA - 1)
                timestamp = Maingasnumber.Timestamp

                If indexA = MainArray.Length - 1 Then
                    timestamp2 = timestamp
                    GoTo nextline
                End If
                Maingasnumber2 = MainArray(indexA + 1)
                timestamp2 = Maingasnumber2.Timestamp
nextline:

                hour = timestamp.Hour
                hour2 = timestamp2.Hour
                shortdate = timestamp.ToShortDateString + " " + hour.ToString + ":00:00"
                hourdate = Convert.ToDateTime(shortdate)
                ndiff = Maingasnumber.Main_Gas_Usage - Maingasnumber3.Main_Gas_Usage
                If ndiff >= 0 Then
                    totMaingas = totMaingas + ndiff
                End If
                If hour <> hour2 Then
                    mainglist.Add(New Mainnatgas With {.Timestamp = hourdate, .Main_Gas_Usage = totMaingas})
                    totMaingas = 0
                End If


            Next

            Return mainglist

        End Function

        Public Function getchartdata(ByVal fromdate As DateTime, ByVal ToDate As DateTime, ByVal chartname As String) As Object

            Dim reval As String = ""
            Dim maingaslst As New List(Of Mainnatgas)()
            Dim mainwatlst As New List(Of Main)()
            Dim mainelelst As New List(Of Mainelec)()

            If chartname = "maingas" Then
                maingaslst = getmgasinterval(fromdate, ToDate)
                Return maingaslst
            ElseIf chartname = "mainwater" Then
                mainwatlst = getMainWaterData(fromdate, ToDate)
                Return mainwatlst
            ElseIf chartname = "electric" Then
                mainelelst = getMainelecData(fromdate, ToDate)
                Return mainelelst
            Else
                Return Nothing
            End If

        End Function

        Public Function gethotptdata(ByVal fromdate As DateTime, ByVal ToDate As DateTime) As List(Of Mainelec)   'ByVal fromdate As DateTime, ByVal ToDate As DateTime

            Dim reval As New List(Of Mainelec)()
            Dim dl As New dlayer
            Dim Mainelec As New Mainelec
            Dim record As IDataRecord
            Dim TimeStamp As DateTime
            Dim Mainelecs As Single

            ' Dim dtMilnorstring2 As String
            ' Dim fromdatetest As String = fromdate(0)

            Mainstring = "select [Timestamp] as Timestamp, [Mains_Power] as Main_Power_W from dbo.DI_Main_1A_Cd where Timestamp > '" + fromdate.ToString("yyyy-MM-dd hh:mm:ss") + "' and Timestamp < '" + ToDate.ToString("yyyy-MM-dd hh:mm:ss") + "' order by Timestamp asc"
            'dtMilnorstring = "select [TimeStamp] as TimeStamp, [Main_Power_W] as Main_Power_W from dbo.ST_DL_sch2 where TimeStamp > '2014-01-23 21:28:00' and Timestamp < '2014-02-09 12:52:00' order by Timestamp asc"
            MainConnection = New SqlConnection(dl.CrConnectionString)
            MainConnection.Open()
            Maincmd = New SqlCommand(Mainstring, MainConnection)
            MainReader = Maincmd.ExecuteReader(CommandBehavior.CloseConnection)

            While MainReader.Read
                record = CType(MainReader, IDataRecord)
                TimeStamp = Date.Parse(record(0), CultureInfo.InvariantCulture)
                Mainelecs = (Single.Parse(record(1), CultureInfo.InvariantCulture)) / 1000


                reval.Add(New Mainelec() With {.Timestamp = TimeStamp, .Main_Power_W = Mainelecs})
            End While


            MainReader.Close()


            'MilnorWater = reval.ToArray()
            Return reval

        End Function

        Public Function getspecData(ByVal fromdate As DateTime, ByVal ToDate As DateTime, ByVal index As Integer, ByVal schedule As Integer) As List(Of usage)   'ByVal fromdate As DateTime, ByVal ToDate As DateTime

            Dim reval As New List(Of usage)()
            Dim dl As New dlayer
            Dim Main As New Main
            Dim record As IDataRecord
            Dim TimeStamp As DateTime
            Dim Mainhot As Single
            Dim Maincold As Single
            Dim fieldname() As String = {"Hot_water_temp", "Cold_water_temp", "Milnor_PH1A", "Milnor_HW_Usage", "Milnor_CW_Usage", "Milnor_Steam_Usage", "Milnor_HW_BTUUsage"}
            Dim Mainfieldname() As String = {"Mains_CW_Usage", "Mains_NG_Usage", "Mains_HW_Usage", "Mains_PH1A", "Mains_Power", "Mains_HW_BTUUsage"}


            If schedule = 1 Then
                Mainstring = "select [Timestamp] as Timestamp, [" + fieldname(index) + "] as " + fieldname(index) + " from dbo.DI_Milnor_1A_Cd where Timestamp > '" + fromdate.ToString("yyyy-MM-dd hh:mm:ss") + "' and Timestamp < '" + ToDate.ToString("yyyy-MM-dd hh:mm:ss") + "' order by Timestamp asc"
            Else
                Mainstring = "select [Timestamp] as Timestamp, [" + Mainfieldname(index) + "] as " + Mainfieldname(index) + " from dbo.DI_Main_1A_Cd where Timestamp > '" + fromdate.ToString("yyyy-MM-dd hh:mm:ss") + "' and Timestamp < '" + ToDate.ToString("yyyy-MM-dd hh:mm:ss") + "' order by Timestamp asc"

            End If
            'dtMilnorstring = "select [TimeStamp] as TimeStamp, [Main_Hot_Water_g] as Main_Hot_Water_g,[Main_Cold_Water_g] as Main_Cold_Water_g  from dbo.ST_DL_sch2 where TimeStamp > '2014-01-23 21:28:00' and Timestamp < '2014-01-29 12:52:00' order by Timestamp asc"
            MainConnection = New SqlConnection(dl.CrConnectionString)
            MainConnection.Open()
            Maincmd = New SqlCommand(Mainstring, MainConnection)
            MainReader = Maincmd.ExecuteReader(CommandBehavior.CloseConnection)

            While MainReader.Read
                record = CType(MainReader, IDataRecord)
                TimeStamp = Date.Parse(record(0), CultureInfo.InvariantCulture)
                Mainhot = Single.Parse(record(1), CultureInfo.InvariantCulture)

                If fieldname(index) = "Mains_CW_Usage" Then
                    Mainhot = Mainhot * 10
                End If

                reval.Add(New usage() With {.Timestamp = TimeStamp, .Usage1 = Mainhot})
            End While


            MainReader.Close()


            'MilnorWater = reval.ToArray()
            Return reval

        End Function
    End Class


End Namespace