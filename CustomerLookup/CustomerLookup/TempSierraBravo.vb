Public Class TempSierraBravo
    Dim pdb As New SierraBravo.PickDB.JetKit
    Public lastresult As String = ""
    Public lastparams As New Hashtable
    Public lastprogram As String = ""

    Public Sub New()
        If IO.File.Exists("c:\pick\sierrabravo.ini") Then
            pdb.Server = IO.File.ReadAllText("c:\pick\sierrabravo.ini")
            Try
                Dim cmdLine = AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData(0)
                '    MsgBox(cmdLine)
                If cmdLine.Split("?")(1).Split("=")(0) = "config" Then
                    pdb.Account = cmdLine.Split("?")(1).Split("=")(1)

                Else
                    pdb.Account = "JOHNSTONE_DEMO"
                End If


            Catch ex As Exception
                pdb.Account = "JOHNSTONE_DEMO"
            End Try

            pdb.Port = "8787"
            pdb.Timeout = 50000
            pdb.Reset()


        Else
            Throw New Exception("Sierra Bravo Can't Connect")
        End If

    End Sub
    Public Function Run(ByVal pgname As String, Optional ByVal params As Hashtable = Nothing) As String
        pdb.Reset()
        pdb.Program = pgname
        If Not params Is Nothing Then
            For Each item As DictionaryEntry In params
                pdb.AddVar(item.Key.ToString, item.Value.ToString)
            Next
        End If
        pdb.Execute()
        lastprogram = pgname
        lastparams = params
        lastresult = pdb.GetResult

        Dim debugmsg As String = "Program:" & pgname & vbCrLf & vbCrLf
        Try
            For Each item As DictionaryEntry In params

                debugmsg += "Param:" & item.Key & " = " & item.Value & vbCrLf
            Next

        Catch ex As Exception

        End Try
        debugmsg += vbCrLf & "Result:" & lastresult
        If lastresult = "" Then debugmsg = "Since there is a blank result, its possible that maxusers have been met." & vbCrLf & vbCrLf & debugmsg

        IO.File.WriteAllText("c:\pick\sb.debug.txt", debugmsg)
        Return lastresult
    End Function
    Public Function ReadItem(ByVal filename As String, ByVal itemid As String) As String
        Dim params As New Hashtable
        params.Add("FILENAME", filename.Trim)
        params.Add("ITEMID", itemid.Trim)
        Return Run("COM.READITEM", params)
    End Function
    Public Function GETDT(ByVal SELECTS As String, ByVal FILENAME As String, ByVal DICTS As String) As Data.DataTable
        Dim params As New Hashtable
        params.Add("FILE", FILENAME.Trim)
        params.Add("SELECTS", SELECTS)
        params.Add("DICTS", DICTS)
        Dim ds As New Data.DataSet
        ds.ReadXml(New IO.StringReader(Run("COM.GETSELECT", params)))
        Return ds.Tables(0)
    End Function
    Public Function GETDT2(ByVal SELECTS As String, ByVal FILENAME As String, ByVal DICTS As String) As Data.DataTable
        Dim params As New Hashtable
        params.Add("FILE", FILENAME.Trim)
        params.Add("SELECTS", SELECTS)
        params.Add("DICTS", DICTS)
        Dim tbstring = Run("COM.GETSELECTTAB", params)
        tbstring = tbstring.Replace("&", "&amp;")
        Dim xmlstring As String = "<DATASET>" & vbCrLf
        For rownum = 0 To tbstring.Split(Chr(13)).Length - 2
            If tbstring.Split(Chr(13))(rownum).Replace(" ", "").Replace(vbTab, "") <> "" Then
                xmlstring = xmlstring & "<DATATABLE>" & vbCrLf
                For i As Integer = 0 To DICTS.Split(" ").Length - 1
                    xmlstring = xmlstring & "<" & DICTS.Split(" ")(i) & ">"
                    xmlstring = xmlstring & tbstring.Split(Chr(13))(rownum).Split(vbTab)(i)
                    xmlstring = xmlstring & "</" & DICTS.Split(" ")(i) & ">"
                Next
                xmlstring = xmlstring & "</DATATABLE>" & vbCrLf
            End If

        Next
        xmlstring = xmlstring & "</DATASET>" & vbCrLf
        Dim ds As New Data.DataSet
        ds.ReadXml(New IO.StringReader(xmlstring))
        Return ds.Tables(0)
    End Function

End Class
