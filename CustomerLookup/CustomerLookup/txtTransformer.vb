Public Class txtTransformer
    Public Shared Function TabToDT(ByVal data As String) As Data.DataTable

        Try


            Dim filelines As String() = data.Replace(vbLf, "").Split(vbCr)
            Dim dt As New Data.DataTable
            Dim filecolumns As String() = filelines(0).Split(vbTab)
            For i As Integer = 0 To filecolumns.Length - 1

                Dim dc As New Data.DataColumn(filecolumns(i).Replace(".", "#"))
                dc.DataType = System.Type.GetType("System.String")
                If dc.ColumnName.ToLower.Contains("amt") Or dc.ColumnName.ToLower.Contains("tot") _
                Or dc.ColumnName.ToLower.Contains("chg") Or dc.ColumnName.ToLower.Contains("$") _
                Or dc.ColumnName.ToLower.Contains("credit") Or dc.ColumnName.ToLower.Contains("debit") Or dc.ColumnName.ToLower.Contains("net") Then
                    dc.DataType = System.Type.GetType("System.Decimal")
                End If
                If dc.ColumnName.ToLower.Contains("dt") Or dc.ColumnName.ToLower.Contains("date") Then
                    dc.DataType = System.Type.GetType("System.DateTime")
                    dc.AllowDBNull = True
                End If
                If dc.ColumnName.ToLower.Contains("seq") Then
                    dc.DataType = System.Type.GetType("System.Int32")
                End If
                dt.Columns.Add(dc)

            Next
            '    Dim theserows(filelines.Length) As dt
            If filelines.Length > 1 Then
                For i As Integer = 1 To filelines.Length - 2
                    If Not filelines(i).Replace(vbTab, "").Replace(" ", "") = "" Then


                        Dim dr As Data.DataRow = dt.NewRow
                        Dim drt As String() = filelines(i).Split(vbTab)
                        For o As Integer = 0 To filecolumns.Length - 1



                            Try
                                'If Not (drt(o) = "" OrElse drt(o).Trim = "") Then
                                If dr(o).GetType.ToString = "System.DateTime" Then

                                End If
                                dr(o) = drt(o).Trim
                                'End If

                            Catch ex As Exception
                                '  dr(o) = ""
                            End Try

                        Next
                        dt.Rows.Add(dr)
                    End If
                Next
            End If
            Return dt

        Catch ex As Exception
            Return New Data.DataTable

        End Try

    End Function
End Class
