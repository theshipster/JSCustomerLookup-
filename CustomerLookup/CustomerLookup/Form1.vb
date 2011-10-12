Public Class Form1
    Dim sbbp As New TempSierraBravo
    Dim WithEvents backgroundworker As New System.ComponentModel.BackgroundWorker
    Dim dt As Data.DataTable
    Private Sub BackgroundWorker_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles backgroundworker.DoWork
        dt = txtTransformer.TabToDT(sbbp.Run("com.getallcustomers"))

    End Sub
    Private Sub BackgroundWorker_RunWorkerCompleted(ByVal sender As Object, _
            ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) _
            Handles backgroundworker.RunWorkerCompleted
        ProgressBar1.Visible = False
        Label1.Visible = False
        RadGridView1.DataSource = dt
    End Sub
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        ProgressBar1.Style = ProgressBarStyle.Marquee

        backgroundworker = New System.ComponentModel.BackgroundWorker
        backgroundworker.RunWorkerAsync()





    End Sub

    Private Sub RadGridView1_CellClick(ByVal sender As Object, ByVal e As Telerik.WinControls.UI.GridViewCellEventArgs) Handles RadGridView1.CellClick
        Try
            Clipboard.SetText(e.Row.Cells(0).Value)
        Catch ex As Exception

        End Try

    End Sub

  


    Private Sub RadGridView1_DataBindingComplete(ByVal sender As Object, ByVal e As Telerik.WinControls.UI.GridViewBindingCompleteEventArgs) Handles RadGridView1.DataBindingComplete
        For Each dc As Telerik.WinControls.UI.GridViewDataColumn In RadGridView1.Columns

        Next


    End Sub

 

    Private Sub RadGridView1_ContextMenuOpening(ByVal sender As Object, ByVal e As Telerik.WinControls.UI.ContextMenuOpeningEventArgs) Handles RadGridView1.ContextMenuOpening
        If e.ContextMenu.Items.Count = 16 AndAlso e.ContextMenu.Items(0).Text = "No filter" Then
            For i As Integer = e.ContextMenu.Items.Count - 1 To 5 Step -1
                e.ContextMenu.Items.RemoveAt(i)
            Next
        End If
    End Sub

    Private Sub RadGridView1_CellDoubleClick(ByVal sender As Object, ByVal e As Telerik.WinControls.UI.GridViewCellEventArgs) Handles RadGridView1.CellDoubleClick
        Try
            Dim att As Atwin2k2.AccuTerm = GetObject(, "Atwin2k2.AccuTerm")
            'Clipboard.SetText(e.Row.Cells(0).Value)
            att.ActiveSession.WriteText(e.Row.Cells(0).Value & vbCr)
            Me.WindowState = FormWindowState.Minimized


        Catch ex As Exception

        End Try
    End Sub
End Class
